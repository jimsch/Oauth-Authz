using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Com.AugustCellars.CoAP;
using Com.AugustCellars.CoAP.Server;
using Com.AugustCellars.CoAP.Server.Resources;
using Com.AugustCellars.COSE;
using PeterO.Cbor;
using Oauth = Com.AugustCellars.CoAP.OAuth;
using Com.AugustCellars.WebToken;

namespace AuthServer
{
    class OauthResource : Resource
    {
        public OauthResource() : base("token")
        {
            // RequireSecurity = true;
        }

        protected override void DoPost(CoapExchange exchange)
        {
            try {
                byte[] responseData = ProcessRequest(exchange.Request.Payload);
                exchange.Respond(StatusCode.Created, responseData, 0);
            }
            catch {
                exchange.Respond(StatusCode.BadRequest);
            }
        }


        public enum ErrorCodes { Invalid_Request = 0, Unsupported_Grant_Type = 4, Invalid_Scope };

        private byte[] CreateError(ErrorCodes code)
        {
            CBORObject error = CBORObject.NewMap();

            error["error"] = CBORObject.FromObject(code);

            return error.EncodeToBytes();
        }

        private byte[] ProcessRequest(byte[] requestData)
        {
            Oauth.Request userRequest = new Oauth.Request(requestData);

            if (!userRequest.Grant_Type.HasValue) {
                return CreateError(ErrorCodes.Invalid_Request);
            }

            if (userRequest.Grant_Type != Oauth.Request.GrantType_ClientToken) {
                return CreateError(ErrorCodes.Unsupported_Grant_Type);
            }

            if (userRequest.Scope != "foobar") {
                return CreateError(ErrorCodes.Invalid_Scope);
            }

            //  Check for which profile we are asking for

            CWT token = new CWT();

            if (userRequest.Profile == "coap_dtls") {

                OneKey sharedKey = new OneKey();
                sharedKey.Add(CoseKeyKeys.KeyType, GeneralValues.KeyType_Octet);
                sharedKey.Add(CoseKeyParameterKeys.Octet_k, CBORObject.FromObject(new byte[] {2, 3, 4, 5, 6, 7, 8, 9, 10, 11}));
                sharedKey.Add(CoseKeyKeys.KeyIdentifier, CBORObject.FromObject(Encoding.UTF8.GetBytes("ACE_KID1")));

                token.Cnf = new Confirmation(sharedKey);

            }
            else if (userRequest.Profile == "coap_oscoap") {
                OneKey sharedKey = new OneKey();
                sharedKey.Add(CoseKeyKeys.KeyType, GeneralValues.KeyType_Octet);
                sharedKey.Add(CoseKeyParameterKeys.Octet_k, CBORObject.FromObject(Encoding.UTF8.GetBytes("OSCOAP goes here")));
                sharedKey.Add(CBORObject.FromObject("sid"), CBORObject.FromObject(Encoding.UTF8.GetBytes("sid")));
                sharedKey.Add(CBORObject.FromObject("rid"), CBORObject.FromObject(Encoding.UTF8.GetBytes("rid")));

                token.Cnf = new Confirmation(sharedKey);

                token.Profile = "coap_oscoap";

            }
            else {
                return CreateError(ErrorCodes.Invalid_Request);
            }

            token.SetClaim(ClaimId.Audience, userRequest.Audience);

            OneKey serverKey = new OneKey();
            serverKey.Add(CoseKeyKeys.KeyType, GeneralValues.KeyType_Octet);
            serverKey.Add(CoseKeyParameterKeys.Octet_k, CBORObject.FromObject(new byte[] { 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17 }));
            serverKey.Add(CoseKeyKeys.KeyIdentifier, CBORObject.FromObject(Encoding.UTF8.GetBytes("SERVER_KID")));
            serverKey.Add(CoseKeyKeys.Algorithm, AlgorithmValues.AES_CCM_64_128_128);

            token.EncryptionKey = serverKey;
           // token.EncryptionKey = Program.AS_InternalKey;

            Oauth.Response userResponse = new Oauth.Response(userRequest) {
                [(CBORObject) Oauth.Oauth_Parameter.Profile] = CBORObject.FromObject(userRequest.Profile),
                [(CBORObject) Oauth.Oauth_Parameter.Cnf] = token.Cnf.AsCBOR,
                [(CBORObject) Oauth.Oauth_Parameter.Access_Token] = CBORObject.FromObject(token.EncodeToBytes()),
                [(CBORObject) Oauth.Oauth_Parameter.Expires_In] = CBORObject.FromObject(3600)
            };

            return userResponse.EncodeToBytes();

        }
    }
}
