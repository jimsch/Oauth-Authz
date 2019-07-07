using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using Com.AugustCellars.CoAP;
using Com.AugustCellars.CoAP.OSCOAP;
using Com.AugustCellars.CoAP.Server.Resources;
using Com.AugustCellars.COSE;
using PeterO.Cbor;
using Oauth = Com.AugustCellars.CoAP.OAuth;
using Com.AugustCellars.WebToken;

namespace AuthServer
{
    public enum ProfileValues
    {
        NoProfile = 0,
        OscoreAS = 1,
        Oscore = 2,
        DtlsAS = 3,
        DtlsPsk = 4,
        DtlsRpk = 5
    };

    class OauthResource : Resource
    {
        bool _allowTls = true;
        bool _allowOscore = true;

        public static Dictionary<byte[], CWT> KeysForIntrospection = new Dictionary<byte[], CWT>(new SecurityContext.ByteArrayComparer());

        public OauthResource() : base("token")
        {
            // RequireSecurity = true;
        }

        protected override void DoPost(CoapExchange exchange)
        {
            try {
                StatusCode status;
                OneKey user = null;

                //  Who is doing the request?

                if (_allowTls && exchange.Request.Session is ISecureSession) {
                    ISecureSession secSession = (ISecureSession) exchange.Request.Session;
                    OneKey tlsKey = secSession.AuthenticationKey;

                    foreach (OneKey x in Program.ClientKeys) {
                        if (x.Equals(tlsKey)) {
                            user = x;
                            break;
                        }
                    }
                }
                else if (_allowOscore && exchange.Request.OscoapContext != null) {
                    foreach (OneKey x in Program.ClientKeys) {
                        if (x == exchange.Request.OscoapContext.UserData) {
                            user = x;
                            break;
                        }
                    }
                }
                else {
                    exchange.Respond(StatusCode.Forbidden);
                    return;
                }

                if (user == null) {
                    //  There is not a security context that is acceptable, return an unauthorized
                    // Response.
                    exchange.Respond(StatusCode.Unauthorized);
                    return;
                }

                exchange.Accept();

                Oauth.Request userRequest;
                bool fJson = false;
                if (exchange.Request.ContentType == MediaType.ApplicationJson) {
                    userRequest = Oauth.Request.FromJSON(exchange.Request.PayloadString);
                    fJson = true;
                }
                else if (exchange.Request.ContentType == MediaType.ApplicationAceCbor || exchange.Request.ContentType == MediaType.ApplicationCbor) {
                    userRequest = Oauth.Request.FromCBOR(exchange.Request.Payload);
                }
                else {
                    exchange.Respond(StatusCode.BadRequest, "Bad Content Type");
                    return;
                }

                byte[] responseData = ProcessRequest(userRequest, user, fJson, out status);
                exchange.Respond(status, responseData, MediaType.ApplicationCbor);
            }
            catch (OauthException e) {
                exchange.Respond(e.Error, e.Body);
            }
            catch (SqlException e)
            {
                Console.WriteLine($"SQL ERROR FOUND = {e}");
                exchange.Respond(StatusCode.BadRequest);
            }
            catch (Exception e) {
                exchange.Respond(StatusCode.BadRequest, e.ToString());
            }
        }


        public enum ErrorCodes { Invalid_Request = 0, Unsupported_Grant_Type = 3, Invalid_Scope, Unauthorized_Client = 4 };

        private byte[] ProcessRequest(Oauth.Request userRequest, OneKey user, bool emitJson, out StatusCode status)
        {
            SqlDataReader keyReader = null;
            try {

                //  Get the database key number

                int clientId = (int) user.UserData;

                //  Query for all of the database information

                SqlCommand cmd = new SqlCommand(
                    "SELECT EntityTable.Name, EntityTable.Type, EntityTable.Attributes, EntityTable.DefaultAudience, " +
                    "EntityTable.DefaultScope, EntityTable.Profiles, KeyTable.*" +
                    "FROM EntityTable INNER JOIN KeyTable ON EntityTable.EntityNo = KeyTable.EntityID  " +
                    $"WHERE KeyTable.KeyNo = {clientId};",
                    Program.Connection);
                keyReader = cmd.ExecuteReader();

                //  We should always have at least one - but never more than one
                if (!keyReader.HasRows) {
                    keyReader.Close();
                    throw new OauthException(StatusCode.Forbidden, ErrorCodes.Unauthorized_Client);
                }

                keyReader.Read();

                //  Get the profiles that the client supports

                int ordX = keyReader.GetOrdinal("Profiles");
                string userProfiles = keyReader.GetString(ordX);

                //  User is NOT required to ask for a grant type.
                //  Default to client token if not is given
                //  We currently only implement the client token type at present.

                if (userRequest.ContainsKey(Oauth.Oauth_Parameter.Grant_Type)) {
                    if (userRequest.Grant_Type != Oauth.Request.GrantType_ClientToken) {
                        throw new OauthException(StatusCode.BadRequest, ErrorCodes.Unsupported_Grant_Type,
                            "Grant type != Client Token");
                    }
                }

                //  Get the audience - it might be a default from the Database

                int ord;
                if (userRequest.Audience == null) {
                    ord = keyReader.GetOrdinal("KeyDefaultAudience");
                    if (!keyReader.IsDBNull(ord) && keyReader.GetString(ord) != "") {
                        userRequest.Audience = keyReader.GetString(ord);
                    }
                    else {
                        ord = keyReader.GetOrdinal("DefaultAudience");
                        if (!keyReader.IsDBNull(ord) && keyReader.GetString(ord) != "") {
                            userRequest.Audience = keyReader.GetString(ord);
                        }
                        else {
                            throw new OauthException(StatusCode.BadRequest, ErrorCodes.Invalid_Request, "No Audience");
                        }
                    }
                }

                //  Get the scope - it might be a default from the Database

                if (userRequest.Scope == null) {
                    ord = keyReader.GetOrdinal("KeyDefaultScope");
                    byte[] data = null;
                    if (!keyReader.IsDBNull(ord)) {
                        data = keyReader.GetSqlBinary(ord).Value;
                    }

                    if (data == null && !keyReader.IsDBNull(ord)) {
                        ord = keyReader.GetOrdinal("DefaultScope");
                        data = keyReader.GetSqlBinary(ord).Value;
                    }

                    if (data != null) {
                        userRequest.Scope = CBORObject.DecodeFromBytes(data);
                    }
                    else {
                        throw new OauthException(StatusCode.BadRequest, ErrorCodes.Invalid_Scope,
                                                 "Scope not of legal type");
                    }
                }

                PermissionSet requestedPermissions;
                if (userRequest.Scope.Type == CBORType.ByteString) {
                    requestedPermissions =
                        new PermissionSet(CBORObject.DecodeFromBytes(userRequest.Scope.GetByteString()));
                }
                else if (userRequest.Scope.Type == CBORType.TextString) {
                    requestedPermissions = new PermissionSet(userRequest.Scope.AsString());
                }
                else {
                    throw new OauthException(StatusCode.BadRequest, ErrorCodes.Invalid_Scope,
                                             "Scope not of legal type");
                }

                // Grab the user's permission expression
                CBORObject permissions = CBORObject.NewMap();

                ord = keyReader.GetOrdinal("Attributes");
                if (!keyReader.IsDBNull(ord)) {
                    if (!keyReader.IsDBNull(ord)) {
                        permissions.Add("EntityAttributes", CBORObject.DecodeFromBytes(keyReader.GetSqlBinary(ord).Value));
                    }
                }

                ord = keyReader.GetOrdinal("KeyAttributes");
                if (!keyReader.IsDBNull(ord)) {
                    if (!keyReader.IsDBNull(ord)) {
                        permissions.Add("KeyAttributes", CBORObject.DecodeFromBytes(keyReader.GetSqlBinary(ord).Value));
                    }
                }

                string subjectKeyName = keyReader.GetString(keyReader.GetOrdinal("KeyName"));

                keyReader.Close();

                //  Now query for possible answers

                cmd.CommandText = $"SELECT * FROM ResourceTable WHERE Audience='{userRequest.Audience}';";
                keyReader = cmd.ExecuteReader();
                if (!keyReader.HasRows) {
                    throw new OauthException(StatusCode.BadRequest, ErrorCodes.Invalid_Request,
                                             "Nothing to match the Audience");
                }

                // Convert the scope to something useful

                int ordRules = keyReader.GetOrdinal("Rules");
                int ordResourceId = keyReader.GetOrdinal("ResourceID");
                int resourceId = 0;
                Scope responseScope = null;
                while (keyReader.Read()) {
                    CBORObject rules;

                    if (keyReader.IsDBNull(ordRules)) continue;
                    rules = CBORObject.DecodeFromBytes(keyReader.GetSqlBinary(ordRules).Value);

                    if (MakeGrantDecision(requestedPermissions, permissions, rules, subjectKeyName, out responseScope)) {
                        resourceId = keyReader.GetInt32(ordResourceId);
                        break;
                    }
                }

                if (responseScope == null) {
                    throw new OauthException(StatusCode.BadRequest, ErrorCodes.Invalid_Scope, "Access Denied");
                }

                keyReader.Close();

                cmd.CommandText =
                    "SELECT EntityTable.EntityNo, EntityTable.Profiles, KeyTable.KeyValue, KeyTable.ASConnection, ResourceMap.ResourceId " +
                    "FROM (EntityTable INNER JOIN KeyTable ON EntityTable.EntityNo = KeyTable.EntityID) INNER JOIN ResourceMap ON EntityTable.EntityNo = ResourceMap.EntityId " +
                    $"WHERE(ResourceMap.ResourceId = {resourceId});";
                keyReader = cmd.ExecuteReader();
                if (!keyReader.HasRows) {
                    throw new OauthException(StatusCode.BadRequest, ErrorCodes.Invalid_Request,
                                             "Nothing to match the Audience");
                }


                // Determine what profile should be used.

                ProfileValues[] userProfileIds;
                ProfileValues useProfileId = ProfileValues.NoProfile;

                userProfileIds = ProfileValuesFromText(userProfiles);

                while (useProfileId == ProfileValues.NoProfile && keyReader.Read()) {
                    ProfileValues[] rsProfileIds = ProfileValuesFromText(keyReader.GetString(1));
                    foreach (ProfileValues u1 in userProfileIds) {
                        foreach (ProfileValues r1 in rsProfileIds) {
                            if (u1 == r1) {
                                useProfileId = u1;

                                break;
                            }
                        }

                        if (useProfileId != ProfileValues.NoProfile) {
                            break;
                        }
                    }
                }

                int rsID = keyReader.GetInt32(0);

                keyReader.Close();

                //  Figure out what key we are going to use to encrypt the CWT token
                cmd.CommandText =
                    "SELECT KeyTable.KeyValue " +
                    "FROM KeyTable " +
                    $"WHERE((ASConnection = 'CWT') AND (EntityId = {rsID}));";
                keyReader = cmd.ExecuteReader();
                if (!keyReader.HasRows) {
                    throw new OauthException(StatusCode.BadRequest, ErrorCodes.Invalid_Request,
                                             "No key for encrypting token");
                }

                keyReader.Read();

                CBORObject cbor = CBORObject.DecodeFromBytes(keyReader.GetSqlBinary(0).Value);
                OneKey cwtKey = new OneKey(cbor);

                keyReader.Close();

                //  Check for which profile we are asking for

                CWT token = new CWT();

                // M00TODO - check with configuration for the RS and see if the profile is acceptable.

                token.SetClaim(ClaimId.Audience, userRequest.Audience);
                token.SetClaim(ClaimId.Scope, CBORObject.FromObject(responseScope.AsCbor().EncodeToBytes()));

                //  If they did not ask for a profile, select one.

                Confirmation clientCnf = null;

                if (useProfileId == ProfileValues.DtlsRpk) {
                    token.Profile = (int)Oauth.ProfileIds.Coap_Dtls;

                    OneKey sharedKey = null;
                    OneKey clientKey = null;

                    if (!SelectDtlsKeys(clientId, rsID, out sharedKey, out clientKey)) {
                        throw new OauthException(StatusCode.BadRequest, ErrorCodes.Invalid_Request,
                                                 "Could not match DTLS keys");
                    }

                    token.Cnf = new Confirmation(clientKey);
                    clientCnf = new Confirmation(sharedKey);
                }
                else if (useProfileId == ProfileValues.DtlsPsk) {
                    token.Profile = (int) Oauth.ProfileIds.Coap_Dtls;

                    OneKey sharedKey = new OneKey();

                    sharedKey.Add(CoseKeyKeys.KeyType, GeneralValues.KeyType_Octet);
                    sharedKey.Add(CoseKeyParameterKeys.Octet_k,
                                  CBORObject.FromObject(Encoding.UTF8.GetBytes("DTLS goes here")));
                    sharedKey.Add(CoseKeyKeys.KeyIdentifier, CBORObject.FromObject(Encoding.UTF8.GetBytes("psk_kid")));

                    token.Cnf = new Confirmation(sharedKey);
                    clientCnf = new Confirmation(sharedKey);
                }
                else if (useProfileId == ProfileValues.Oscore) {
                    token.Profile = (int)Oauth.ProfileIds.Coap_Oscore;

                    //  Generate a random key to use
                    CBORObject sharedKey = CBORObject.NewMap();

                    sharedKey.Add(1, CBORObject.FromObject(Encoding.UTF8.GetBytes("OSCORE goes here")));
                    sharedKey.Add(2, CBORObject.FromObject(Encoding.UTF8.GetBytes("sid")));
                    sharedKey.Add(3, CBORObject.FromObject(Encoding.UTF8.GetBytes("rid")));
                    CBORObject objTemp = CBORObject.NewMap();
                    objTemp.Add(Confirmation.ConfirmationIds.COSE_OSCORE, sharedKey);

                    token.Cnf = new Confirmation(objTemp);

                    CBORObject clientKey = CBORObject.NewMap();

                    clientKey.Add(1, sharedKey[CBORObject.FromObject(1)]);
                    clientKey.Add(2, sharedKey[CBORObject.FromObject(3)]);
                    clientKey.Add(3, sharedKey[CBORObject.FromObject(2)]);
                    objTemp = CBORObject.NewMap();
                    objTemp.Add(Confirmation.ConfirmationIds.COSE_OSCORE, clientKey);
                    clientCnf = new Confirmation(objTemp);
                }
                else {
                    status = StatusCode.BadRequest;
                    throw new OauthException(StatusCode.BadRequest, ErrorCodes.Invalid_Request,
                                             "Invalid profile requested");
                }

                //  How long is this good for?

                int timeSpan = 3600;
                token.ExperationTime = DateTime.Now + new TimeSpan(0, 0, 0, timeSpan);


                byte[] accessToken;

                if (false /*scope.Introspection*/) {
                    status = StatusCode.Created;
                    byte[] key = new byte[4];
                    Random r = new Random();
                    while (true) {
                        r.NextBytes(key);
                        if (!KeysForIntrospection.ContainsKey(key)) break;
                    }

                    KeysForIntrospection.Add(key, token);

                    accessToken = key;

                }
                else {
                    token.EncryptionKey = cwtKey;

                    accessToken = token.EncodeToBytes();
                }

                Oauth.ProfileIds returnedProfile;
                switch (useProfileId) {
                    case ProfileValues.Oscore:
                        returnedProfile = Oauth.ProfileIds.Coap_Oscore;
                        break;

                    case ProfileValues.DtlsPsk:
                        returnedProfile = Oauth.ProfileIds.Coap_Dtls;
                        break;

                    default:
                        throw new Exception("Error setting the profile on return");
                }

                Oauth.Response userResponse = new Oauth.Response(userRequest) {
                    [Oauth.Oauth_Parameter.Profile] = CBORObject.FromObject(returnedProfile),
                    [Oauth.Oauth_Parameter.Access_Token] = CBORObject.FromObject(accessToken),
                    [Oauth.Oauth_Parameter.Expires_In] = CBORObject.FromObject(timeSpan),
                };

                if (clientCnf != null) {
                    userResponse[Oauth.Oauth_Parameter.Cnf] = clientCnf.AsCBOR;
                }

                keyReader.Close();

                status = StatusCode.Created;
                if (emitJson) {
                    return Encoding.UTF8.GetBytes(userResponse.EncodeToJson());
                }
                return userResponse.EncodeToBytes();
            }
            catch {
                if (keyReader != null) {
                    keyReader.Close();
                }

                throw;
            }
        }


        //
        //  We need to pass in the following information:
        //  1.  The client id
        //  2.  The Resource Server id
        //  3.  The key used to validate to the AS
        //  4.  The scope asked for
        //  5.  The audience asked for
        //  6.  Client attributes
        //  7.  RS attributes

        bool MakeGrantDecision(PermissionSet requestedPermissions, CBORObject entityAttributes, CBORObject resourceRules, string subjectKeyName, out Scope responseScope)
        {
            AccessControl ac = new AccessControl(resourceRules);

            responseScope = ac.Evaluate(subjectKeyName);
            return responseScope != null;
        }

        public static  ProfileValues[] ProfileValuesFromText(string profiles)
        {
            List<ProfileValues> lst = new List<ProfileValues>();
            foreach (string s in profiles.Split(' ')) {
                switch (s.ToUpper()) {
                case "OSCORE":
                    lst.Add(ProfileValues.Oscore);
                    break;

                case "OSCORE-AS":
                    lst.Add(ProfileValues.OscoreAS);
                    break;

                case "DTLS":
                case "DTLS-RPK":
                    lst.Add(ProfileValues.DtlsRpk);
                    break;

                case "DTLS-PSK":
                    lst.Add(ProfileValues.DtlsPsk);
                    break;

                case "DTLS-AS":
                    lst.Add(ProfileValues.DtlsAS);
                    break;

                default:
                    Console.WriteLine($"ERROR: Unsupported Oauth Profile - {s}");
                    break;
                }
            }

            return lst.ToArray();
        }

        private class AlgMatcher
        {
            public readonly string KeyValue;
            public AlgMatcher(string keyValue)
            {
                KeyValue = keyValue;
            }

            public bool Matches(AlgMatcher other)
            {
                return true;
            }
        }

        bool SelectDtlsKeys(int clientId, int rsID, out OneKey sharedKey, out OneKey clientKey)
        {
            SqlCommand cmd = new SqlCommand(
                "SELECT KeyTable.KeyValue, EntityTable.EntityID, KeyTable.ASConnection " +
                "FROM EntityTable INNER JOIN KeyTable ON EntityTable.EntityId = KeyTable.EntityID " +
                $"WHERE(((EntityTable.EntityID) = {clientId}) AND((KeyTable.ASConnection) = 'DTLS'));",
                Program.Connection);

            SqlDataReader rdr = cmd.ExecuteReader();
            List<AlgMatcher> clientList = new List<AlgMatcher>();

            while (rdr.Read()) {
                string a = rdr.GetString(0);

                clientList.Add(new AlgMatcher(a));
            }

            rdr.Close();

            cmd.CommandText = "SELECT KeyTable.KeyValue, EntityTable.EntityID, KeyTable.ASConnection " +
                              "FROM EntityTable INNER JOIN KeyTable ON EntityTable.EntityId = KeyTable.EntityID " +
                              $"WHERE(((EntityTable.EntityID) = {rsID}) AND((KeyTable.ASConnection) = \"DTLS\"));";

            rdr = cmd.ExecuteReader();
            List<AlgMatcher> rsList = new List<AlgMatcher>();

            while (rdr.Read()) {
                string a = rdr.GetString(0);

                rsList.Add(new AlgMatcher(a));
            }

            rdr.Close();

            foreach (AlgMatcher rs in rsList) {
                foreach (AlgMatcher cl in clientList) {
                    if (rs.Matches(cl)) {
                        CBORObject cbor = CBORDiagnostics.Parse(rs.KeyValue);

                        sharedKey = new OneKey(cbor);

                        cbor = CBORDiagnostics.Parse(cl.KeyValue);
                        clientKey = new OneKey(cbor);
                    }
                }
            }


            sharedKey = null;
            clientKey = null;
            return false;
        }

    }
}
