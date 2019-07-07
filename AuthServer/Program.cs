using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using Com.AugustCellars.CoAP.DTLS;
using Com.AugustCellars.CoAP.Log;
using Com.AugustCellars.CoAP.Net;
using Com.AugustCellars.CoAP.OSCOAP;
using Com.AugustCellars.CoAP.Server;
using Com.AugustCellars.COSE;
using PeterO.Cbor;

namespace AuthServer
{
    class Program
    {
        public static OneKey AS_InternalKey;
        public static List<OneKey> ClientKeys = new List<OneKey>();
        public static List<OneKey> ResourceServerKeys = new List<OneKey>();

        public static SqlConnection Connection;

        static void Main(string[] args)
        {
            TlsKeyPairSet mySignKeys = new TlsKeyPairSet();
            KeySet mySharedKeys = new KeySet();
            SecurityContextSet myOscoreKeys = new SecurityContextSet();

            LogManager.Level = LogLevel.All;
            LogManager.Instance = new FileLogManager(Console.Out);


            try {
                //  Open the Database to be used
                Connection = new SqlConnection(@"server=.\SQLExpress; Database=ACE; Integrated Security=true");
                Connection.Open();

                //  Load all of the keys 

                SqlCommand cmd = new SqlCommand("SELECT * FROM KeyTable", Connection);
                SqlDataReader reader = cmd.ExecuteReader();

                int ordKeyValue = reader.GetOrdinal("KeyValue");
                int ordConnection = reader.GetOrdinal("ASConnection");
                int ordPivLast = reader.GetOrdinal("PIV_Last");
                int ordHitLast = reader.GetOrdinal("HitTest_Last");

                while (reader.Read()) {
                    string type = reader.GetString(ordConnection);
                    byte[] keyData = reader.GetSqlBinary(ordKeyValue).Value;

                    OneKey key = new OneKey(CBORObject.DecodeFromBytes(keyData));
                    key.UserData = reader["KeyNo"];

                    switch (type) {
                    case "DTLS":
                        mySharedKeys.AddKey(key);
                        ClientKeys.Add(key);
                        break;

                    case "OSCORE":
                        Int64 piv = reader.GetInt32(ordPivLast);
                        Int64 hitTest = reader.GetInt32(ordHitLast);

                        SecurityContext ctx = BuildContextFromKey(key);
                        // ctx.RestoreState(piv, hitTest);
                        myOscoreKeys.Add(ctx);
                        ctx.UserData = key;
                        ClientKeys.Add(key);

                        SecurityContextSet.AllContexts.Add(ctx);
                        break;

                    case "AS-DTLS":
                        ResourceServerKeys.Add(key);
                        mySignKeys.AddKey(new TlsKeyPair(key, key));
                        break;

                    case "CWT": // These are used for encrypting tokens not sessions.
                        break;

                    default:
                        Console.WriteLine($"Processing key table found an unknown ASConnection of {type}");
                        break;
                    }
                }

                reader.Close();
            }
            catch (SqlException e) {
                Console.WriteLine($"Error when trying to read key table {Connection.DataSource} - {e}");
                Environment.Exit(-1);
            }

            AS_InternalKey = new OneKey();
            AS_InternalKey.Add(CoseKeyKeys.KeyType, GeneralValues.KeyType_Octet);
            AS_InternalKey.Add(CoseKeyParameterKeys.Octet_k, CBORObject.FromObject(new byte[] {22, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 }));
            AS_InternalKey.Add(CoseKeyKeys.KeyIdentifier, CBORObject.FromObject(Encoding.UTF8.GetBytes("OAUTH_KID")));
            AS_InternalKey.Add(CoseKeyKeys.Algorithm, AlgorithmValues.AES_CCM_64_128_128);

            CoapServer server = new CoapServer(5688);
            CoAPEndPoint ep = new DTLSEndPoint(mySignKeys, mySharedKeys, 5689 /* 5684*/ );
            server.AddEndPoint(ep);
            server.Add(new OauthResource());

            server.Add(new IntrospectResource("introspect"));
            
            server.Start();

            Console.ReadKey();
        }


        public static SecurityContext BuildContextFromKey(OneKey oneKey)
        {
            byte[] salt = null;
            if (oneKey.ContainsName("slt")) salt = oneKey[CBORObject.FromObject("slt")].GetByteString();
            CBORObject alg = null;
            if (oneKey.ContainsName(CoseKeyKeys.Algorithm)) alg = oneKey[CoseKeyKeys.Algorithm];
            CBORObject kdf = null;
            if (oneKey.ContainsName(CBORObject.FromObject("kdf"))) kdf = oneKey[CBORObject.FromObject("kdf")];

            SecurityContext oscoapContext = SecurityContext.DeriveContext(
                oneKey[CoseKeyParameterKeys.Octet_k].GetByteString(),
                null,
                oneKey[CBORObject.FromObject(7)].GetByteString(),
                oneKey[CBORObject.FromObject(6)].GetByteString(),
                salt, alg, kdf);
            return oscoapContext;
        }
    }
}
