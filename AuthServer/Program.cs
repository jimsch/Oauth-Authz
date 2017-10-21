using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Com.AugustCellars.CoAP.DTLS;
using Com.AugustCellars.CoAP.Net;
using Com.AugustCellars.CoAP.Server;
using Com.AugustCellars.COSE;
using PeterO.Cbor;

namespace AuthServer
{
    class Program
    {
        public static OneKey AS_InternalKey;
        static void Main(string[] args)
        {
            KeySet mySignKeys = null;
            KeySet mySharedKeys = new KeySet();

            OneKey key = new OneKey();
            key.Add(CoseKeyKeys.KeyType, GeneralValues.KeyType_Octet);
            key.Add(CoseKeyParameterKeys.Octet_k, CBORObject.FromObject(new byte[] { 2, 3, 4, 5, 6, 7, 8, 9, 10, 11 }));
            key.Add(CoseKeyKeys.KeyIdentifier, CBORObject.FromObject(Encoding.UTF8.GetBytes("ACE_KID1")));
            key.UserData = "Level1";
            mySharedKeys.AddKey(key);

            key = new OneKey();
            key.Add(CoseKeyKeys.KeyType, GeneralValues.KeyType_Octet);
            key.Add(CoseKeyParameterKeys.Octet_k, CBORObject.FromObject(new byte[] { 12, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11 }));
            key.Add(CoseKeyKeys.KeyIdentifier, CBORObject.FromObject(Encoding.UTF8.GetBytes("ACE_KID2")));
            mySharedKeys.AddKey(key);

            AS_InternalKey = new OneKey();
            AS_InternalKey.Add(CoseKeyKeys.KeyType, GeneralValues.KeyType_Octet);
            AS_InternalKey.Add(CoseKeyParameterKeys.Octet_k, CBORObject.FromObject(new byte[] {22, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 }));
            AS_InternalKey.Add(CoseKeyKeys.KeyIdentifier, CBORObject.FromObject(Encoding.UTF8.GetBytes("OAUTH_KID")));
            AS_InternalKey.Add(CoseKeyKeys.Algorithm, AlgorithmValues.AES_CCM_64_128_128);

            key = new OneKey();
            key.Add(CoseKeyKeys.KeyType, GeneralValues.KeyType_Octet);
            key.Add(CoseKeyParameterKeys.Octet_k, CBORObject.FromObject(new byte[] { 15, 12, 2, 3, 4, 5, 6, 7, 8, 9, 10 }));
            key.Add(CoseKeyKeys.KeyIdentifier, CBORObject.FromObject(Encoding.UTF8.GetBytes("AS_RS")));
            mySharedKeys.AddKey(key);



            CoapServer server = new CoapServer();
            CoAPEndPoint ep = new DTLSEndPoint(mySignKeys, mySharedKeys, 5689);
            server.AddEndPoint(ep);
            server.Add(new OauthResource());

            server.Add(new IntrospectResource("introspect"));
            
            server.Start();

            Console.ReadKey();
        }
    }
}
