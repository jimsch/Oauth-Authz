using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Com.AugustCellars.CoAP;
using Com.AugustCellars.CoAP.OAuth;
using Com.AugustCellars.CoAP.Server.Resources;
using Com.AugustCellars.COSE;
using Com.AugustCellars.WebToken;
using PeterO.Cbor;

namespace AuthServer
{
    class IntrospectResource : Resource
    {
        private OneKey _serverKey;
        public IntrospectResource(string name) : base(name)
        {
            // RequireSecurity = true;

            _serverKey = new OneKey();
            _serverKey.Add(CoseKeyKeys.KeyType, GeneralValues.KeyType_Octet);
            _serverKey.Add(CoseKeyParameterKeys.Octet_k, CBORObject.FromObject(new byte[] { 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17 }));
            _serverKey.Add(CoseKeyKeys.KeyIdentifier, CBORObject.FromObject(Encoding.UTF8.GetBytes("SERVER_KID")));
            _serverKey.Add(CoseKeyKeys.Algorithm, AlgorithmValues.AES_CCM_64_128_128);
        }

        protected override void DoPost(CoapExchange exchange)
        {
            exchange.Accept();

            IntrospectRequest requestIn = new IntrospectRequest(exchange.Request.Payload);

            //  Turn the token into a CWT

            KeySet _myKeys = new KeySet();
            _myKeys.AddKey(Program.AS_InternalKey);
            KeySet _asSigningKeys = null;

            try {
                if (!OauthResource.KeysForIntrospection.ContainsKey(requestIn.Token)) {
                    throw new OauthException(StatusCode.BadRequest, OauthResource.ErrorCodes.Invalid_Request, "Unrecognized Token");
                }

                CWT cwt = OauthResource.KeysForIntrospection[requestIn.Token];

                IntrospecResponse response = new IntrospecResponse();
                response.Active = true;

                if (cwt.ExperationTime < DateTime.Now) {
                    response.Active = false;
                }

                response.Scope = cwt.GetClaim(ClaimId.Scope);
                response.Audience = cwt.GetClaim(ClaimId.Audience).AsString();
                response.Profile = (int) ProfileIds.Coap_Dtls;
                response.Cnf = cwt.Cnf;
                
                exchange.Respond(StatusCode.Content, response.EncodeToBytes(), MediaType.ApplicationOctetStream);

            }
            catch (OauthException e) {
                exchange.Respond(e.Error, e.Body);
            }
            catch (Exception e) {
                //  Something went wrong 
                exchange.Respond(StatusCode.BadRequest);
            }
        }
    }
}
