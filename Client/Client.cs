using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

using Com.AugustCellars.CoAP;
using Com.AugustCellars.CoAP.DTLS;
using Com.AugustCellars.CoAP.OSCOAP;
using Com.AugustCellars.CoAP.Util;
using Com.AugustCellars.COSE;
using Com.AugustCellars.WebToken;
using PeterO.Cbor;
using OAuth = Com.AugustCellars.CoAP.OAuth;

namespace Client
{
    class Client
    {
        private static string AS_URL = "coap://localhost/token";

        private static string Resource_URL = "coap://localhost/echo";

        static void Main(string[] args)
        {
           // Console.WriteLine("Test for - AS use DTLS, RS use DTLS");
           // RunTest(true, true);

            Console.WriteLine("Test for - AS use DTLS, RS use OSCOAP");
            RunTest(true, false);
        }

        static void RunTest(bool fUseDtlsToAS, bool fUseDtlsToRS)
        { 
            //  Make a request to the resource server.

            OneKey asKey = new OneKey();
            asKey.Add(CoseKeyKeys.KeyType, GeneralValues.KeyType_Octet);
            asKey.Add(CoseKeyParameterKeys.Octet_k, CBORObject.FromObject(new byte[] { 2, 3, 4, 5, 6, 7, 8, 9, 10, 11 }));
            asKey.Add(CoseKeyKeys.KeyIdentifier, CBORObject.FromObject(Encoding.UTF8.GetBytes("ACE_KID1")));

            CoapClient client = new CoapClient(new Uri(Resource_URL)) {
      //          Timeout = 1000 * 4,
            };
            Response response = client.Get();
            if ((response.StatusCode != StatusCode.Unauthorized) ||
                (response.ContentFormat != 65008)) {
                Console.WriteLine("Unexpected response gotten from the server");
                Debug.Assert(false, "Unexpected response gotten from the server");
            }

            OAuth.AsInfo info = new OAuth.AsInfo(response.Payload);

            CoapClient authServer = new CoapClient(new Uri(info.ASServer));
            if (fUseDtlsToAS) {
                authServer.EndPoint = new DTLSClientEndPoint(asKey);
                authServer.EndPoint.Start();
            }

            OAuth.Request myrequest = new OAuth. Request("client_credentials") {
               Audience = "localhost",
               Scope = "foobar"
            };

            if (fUseDtlsToRS) {
                myrequest.Profile = "coap_dtls";
            }
            else {
                myrequest.Profile = "coap_oscoap";
            }

            byte[] xxx = myrequest.EncodeToBytes();

            response = authServer.Post(xxx, 0);
            xxx = response.Payload;

            OAuth.Response myResponse = new OAuth.Response(xxx);

            if (myResponse.Profile != myrequest.Profile) {
                Console.WriteLine("Profile mismatch");
                return;
            }


            client.Uri = new Uri("coap://localhost/authz-info");
            response = client.Post(myResponse[(CBORObject) OAuth.Oauth_Parameter.Access_Token].GetByteString(), 62);


            Confirmation cnf = myResponse.Confirmation;

            if (myResponse.Profile == "coap_dtls") {
                OneKey oneKey = cnf.Key;
                DTLSClientEndPoint endPoint = new DTLSClientEndPoint(oneKey);
                endPoint.Start();
                client.EndPoint = endPoint;

                client.Uri = new Uri("coaps://localhost/echo");
            }
            else {
                OneKey oneKey = cnf.Key;
                byte[] salt = null;
                if (oneKey.ContainsName("slt")) salt = oneKey[CBORObject.FromObject("slt")].GetByteString();
                CBORObject alg = null;
                if (oneKey.ContainsName(CoseKeyKeys.Algorithm)) alg = oneKey[CoseKeyKeys.Algorithm];
                CBORObject kdf = null;
                if (oneKey.ContainsName(CBORObject.FromObject("kdf"))) kdf = oneKey[CBORObject.FromObject("kdf")];

                SecurityContext oscoapContext = SecurityContext.DeriveContext(oneKey[CoseKeyParameterKeys.Octet_k].GetByteString(),
                    oneKey[CBORObject.FromObject("sid")].GetByteString(),
                    oneKey[CBORObject.FromObject("rid")].GetByteString(),
                    salt, alg, kdf);

     //          client.Oscoap = oscoapContext;      // M00BUG
            }

            response = client.Get();

            Console.WriteLine(Utils.ToString(response));

        }
    }
}
