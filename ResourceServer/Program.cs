using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

using Com.AugustCellars.CoAP;
using Com.AugustCellars.CoAP.DTLS;
using Com.AugustCellars.CoAP.OSCOAP;
using Com.AugustCellars.CoAP.Server;
using Com.AugustCellars.COSE;
using Com.AugustCellars.WebToken;
using PeterO.Cbor;

namespace ResourceServer
{
    class Program
    {
        public static KeySet DtlsKeySet = new KeySet();
        public static SecurityContextSet OscoapContexts;

        static void Main(string[] args)
        {
            KeySet myDecryptKeySet = new KeySet();
            OneKey key = new OneKey();

            key.Add(CoseKeyKeys.KeyType, GeneralValues.KeyType_Octet);
            key.Add(CoseKeyParameterKeys.Octet_k, CBORObject.FromObject(new byte[] {2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17}));
            key.Add(CoseKeyKeys.KeyIdentifier, CBORObject.FromObject(Encoding.UTF8.GetBytes("SERVER_KID")));
            key.Add(CoseKeyKeys.Algorithm, AlgorithmValues.AES_CCM_64_128_128);

            myDecryptKeySet.AddKey(key);



            CoapServer server = new CoapServer();
            server.Add(new AuthZ(myDecryptKeySet, null));
            server.Add(new EchoResource());
            OscoapContexts = SecurityContextSet.AllContexts;

            server.Start();


            CoapServer dtlsServer = new CoapServer();
            DTLSEndPoint ep = new DTLSEndPoint(null, DtlsKeySet, 5684);

            dtlsServer.AddEndPoint(ep);
            dtlsServer.Add(new AuthZ(myDecryptKeySet, null));
            dtlsServer.Add(new EchoResource());

            dtlsServer.Start();


            Console.ReadKey();
        }

        public static bool AddNewCWT(CWT cwt)
        {
            OneKey newKey = cwt.Cnf.Key;

            if (cwt.Profile == "coap_oscoap") {
                OneKey oneKey = cwt.Cnf.Key;
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


                SecurityContext ctx = SecurityContext.DeriveContext(newKey[CoseKeyParameterKeys.Octet_k].GetByteString(),
                    newKey[CBORObject.FromObject("rid")].GetByteString(), newKey[CBORObject.FromObject("sid")].GetByteString(),
                    newKey[CBORObject.FromObject("salt")].GetByteString(),
                    newKey[CoseKeyKeys.Algorithm], newKey[CBORObject.FromObject("kdf")]);

                OscoapContexts.Add(ctx);

                return true;
            }


            byte[] kid = newKey[CoseKeyKeys.KeyIdentifier].GetByteString();

            foreach (OneKey key in DtlsKeySet) {
                if (key.HasKid(kid)) {
                    // Need an update to deal with this
                    return false;
                }

            }

            DtlsKeySet.AddKey(newKey);

            return true;

        }
    }
}
