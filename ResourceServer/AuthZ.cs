using Com.AugustCellars.CoAP.Server.Resources;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Com.AugustCellars.CoAP;
using Com.AugustCellars.CoAP.DTLS;
using Com.AugustCellars.CoAP.OAuth;
using Com.AugustCellars.COSE;
using PeterO.Cbor;
using Com.AugustCellars.WebToken;

namespace ResourceServer
{
    class AuthZ : Resource
    {
        private readonly KeySet _myKeys;
        private readonly KeySet _asSigningKeys;

        public AuthZ(KeySet myKeys, KeySet asSigningKeys) : base("authz-info")
        {
            _myKeys = myKeys;
            _asSigningKeys = asSigningKeys;
        }

        protected override void DoPost(CoapExchange exchange)
        {
            try {
                CWT cwt = CWT.Decode(exchange.Request.Payload, _myKeys, _asSigningKeys);

                OneKey key = cwt.Cnf.Key;

                Program.AddNewCWT(cwt);

                exchange.Respond(StatusCode.Created);
            }
            catch (CoseException e) {
                //
                //  Maybe we should do introspection?
                //

                byte[] rgb = TryIntrospection(exchange);

                //  If we get here then something good went wrong
                exchange.Respond(StatusCode.Unauthorized, "cose error");
            }
            catch (CwtException e) {
                //
                //  Maybe we should do introspection?
                //

                byte[] rgb = TryIntrospection(exchange);

                //  If we get here then something good went wrong
                exchange.Respond(StatusCode.Unauthorized, "cwt error");
            }
            catch (Exception e) {
                //  If we get here then something bad went wrong
                exchange.Respond(StatusCode.Unauthorized, "other error");
            }

            
        }

        private byte[] TryIntrospection(CoapExchange exchange)
        {
            //  Ack the message to make the client be quite
            exchange.Accept();

            //

            IntrospectRequest request = new IntrospectRequest() {
                Token = exchange.Request.Payload
            };

            OneKey key = new OneKey();
            key.Add(CoseKeyKeys.KeyType, GeneralValues.KeyType_Octet);
            key.Add(CoseKeyParameterKeys.Octet_k, CBORObject.FromObject(new byte[] { 15, 12, 2, 3, 4, 5, 6, 7, 8, 9, 10 }));
            key.Add(CoseKeyKeys.KeyIdentifier, CBORObject.FromObject(Encoding.UTF8.GetBytes("AS_RS")));


            CoapClient client = new CoapClient(new Uri("coaps://localhost:5689/introspect"));
            client.EndPoint = new DTLSClientEndPoint(key);
            client.EndPoint.Start();

            Com.AugustCellars.CoAP.Response response = client.Post(request.EncodeToBytes(), MediaType.ApplicationOctetStream);

            if (response.StatusCode != StatusCode.Content) {
                ;
                exchange.Respond(StatusCode.BadRequest);
                return null;
            }

            IntrospecResponse iResponse = new IntrospecResponse(response.Payload);
            if (!iResponse.Active) {
                exchange.Respond(StatusCode.Unauthorized);
                return null;
            }



            OneKey newKey = iResponse.Cnf.Key;

            byte[] kid = newKey[CoseKeyKeys.KeyIdentifier].GetByteString();

            foreach (OneKey keyX in Program.DtlsKeySet)
            {
                if (keyX.HasKid(kid)) {
                    // Need an update to deal with this

                    exchange.Respond(StatusCode.BadGateway);
                    return null;
                }

            }

            Program.DtlsKeySet.AddKey(newKey);

            if (iResponse.HasKey(CBORObject.FromObject("client_token"))) {
                exchange.Respond(StatusCode.Created, iResponse.ClientToken, MediaType.ApplicationOctetStream);
            }
            else exchange.Respond(StatusCode.Created);

            return null;
        }
    }
}
