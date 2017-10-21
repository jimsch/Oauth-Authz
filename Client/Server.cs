using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Oauth = Com.AugustCellars.CoAP.OAuth;
using PeterO.Cbor;
using Com.AugustCellars.COSE;

using Com.AugustCellars.WebToken;

namespace Client
{
    class Server
    {
        public enum ErrorCodes { Invalid_Request = 0, Unsupported_Grant_Type = 4, Invalid_Scope };


        public byte[] PostToken(byte[] requestData)
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

            OneKey sharedKey = new OneKey();
            sharedKey.Add(CoseKeyKeys.KeyType, GeneralValues.KeyType_Octet);
            sharedKey.Add(CoseKeyParameterKeys.Octet_k, CBORObject.FromObject(new byte[] {2, 3, 4, 5, 6, 7, 8, 9, 10, 11}));
            sharedKey.Add(CoseKeyKeys.KeyIdentifier, CBORObject.FromObject(Encoding.UTF8.GetBytes("ACE_KID1")));

            Confirmation cnf = new Confirmation(sharedKey);

            CWT token = new CWT() {
                Cnf = cnf
            };
            token.SetClaim(ClaimId.Audience, userRequest.Audience);

            OneKey serverKey = new OneKey();
            serverKey.Add(CoseKeyKeys.KeyType, GeneralValues.KeyType_Octet);
            serverKey.Add(CoseKeyParameterKeys.Octet_k, CBORObject.FromObject(new byte[] {2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17}));
            serverKey.Add(CoseKeyKeys.KeyIdentifier, CBORObject.FromObject(Encoding.UTF8.GetBytes("SERVER_KID")));
            serverKey.Add(CoseKeyKeys.Algorithm, AlgorithmValues.AES_CCM_64_128_128);

            token.EncryptionKey = serverKey;

            Oauth.Response userResponse = new Oauth.Response(userRequest) {
                [(CBORObject) Oauth.Oauth_Parameter.Profile] = CBORObject.FromObject("coap_dtls"),
                [(CBORObject) Oauth.Oauth_Parameter.Cnf] = cnf.AsCBOR,
                [(CBORObject) Oauth.Oauth_Parameter.Access_Token] = CBORObject.FromObject(token.EncodeToBytes()),
                [(CBORObject) Oauth.Oauth_Parameter.Expires_In] = CBORObject.FromObject(3600)
            };

            return userResponse.EncodeToBytes();
        }



        private byte[] CreateError(ErrorCodes code)
        {
            CBORObject error = CBORObject.NewMap();

            error["error"] = CBORObject.FromObject(code);

            return error.EncodeToBytes();
        }
    }
}
