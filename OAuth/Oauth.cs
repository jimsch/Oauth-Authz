using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Com.AugustCellars.WebToken;
using PeterO.Cbor;

namespace Com.AugustCellars.CoAP.OAuth
{
    public class Oauth_Parameter
    {
        public static Dictionary<string, Oauth_Parameter> NameDictionary = new Dictionary<string, Oauth_Parameter>();
        public static Dictionary<int, Oauth_Parameter> IntDictionary = new Dictionary<int, Oauth_Parameter>();

        public static Oauth_Parameter Access_Token = new Oauth_Parameter(1, "access_token", CBORType.ByteString);
        public static Oauth_Parameter Expires_In = new Oauth_Parameter(2, "expires_in", 0);
        public static Oauth_Parameter Aud = new Oauth_Parameter(5, "audience", CBORType.TextString);
        public static Oauth_Parameter ReqCnf = new Oauth_Parameter(4, "req_cnf", CBORType.Map);
        public static Oauth_Parameter Cnf = new Oauth_Parameter(8, "cnf", CBORType.Map, CnfToJson);
        public static Oauth_Parameter Client_id = new Oauth_Parameter(24, "client_id", CBORType.TextString);
        public static Oauth_Parameter Client_secret = new Oauth_Parameter(25, "client_secret", CBORType.ByteString);
        public static Oauth_Parameter Response_type = new Oauth_Parameter(26, "response_type", CBORType.TextString);
        public static Oauth_Parameter Redirect_uri = new Oauth_Parameter(27, "redirect_uri", CBORType.TextString);
        public static Oauth_Parameter Scope = new Oauth_Parameter(9, "scope",  CBORType.TextString);
        public static Oauth_Parameter State = new Oauth_Parameter(28, "state", CBORType.TextString);
        public static Oauth_Parameter Code = new Oauth_Parameter(29, "code", CBORType.ByteString);
        public static Oauth_Parameter Error = new Oauth_Parameter(30, "error", CBORType.TextString);
        public static Oauth_Parameter Error_Description = new Oauth_Parameter(31, "error_description",CBORType.TextString);
        public static Oauth_Parameter Error_URI =new Oauth_Parameter(32, "error_uri", CBORType.TextString);
        public static Oauth_Parameter Grant_Type = new Oauth_Parameter(33, "grant_type", CBORType.Number);
        public static Oauth_Parameter Token_Type = new Oauth_Parameter(34, "token_type", CBORType.TextString);
        public static Oauth_Parameter Username = new Oauth_Parameter(35, "username",CBORType.TextString);
        public static Oauth_Parameter Password = new Oauth_Parameter("password", CBORType.TextString);
        public static Oauth_Parameter Refresh_Token = new Oauth_Parameter(37, "refresh_token", CBORType.ByteString);
        public static Oauth_Parameter Profile = new Oauth_Parameter(38, "profile", CBORType.TextString);
        public static Oauth_Parameter Rs_cnf = new Oauth_Parameter(41, "rs_cnf", CBORType.Map);
        public static Oauth_Parameter CNonce = new Oauth_Parameter(39, "cnonce", CBORType.ByteString);


        private readonly CBORType _type;

        private int? _key;

        public Func<CBORObject, CBORObject> ToJSON;

        public int Key
        {
            get {
                if (! _key.HasValue) throw new Exception("No CBOR Key defined");
                return (int) _key;
            }
        }
        public string KeyJson { get; private set; }

        public Oauth_Parameter(int keyCBOR, string keyJSON, CBORType type, Func<CBORObject, CBORObject> toJson = null)
        {
            _key = keyCBOR;
            KeyJson = keyJSON;
            if (toJson == null) ToJSON = Identity;
            else ToJSON = toJson;
            _type = type;

            NameDictionary.Add(keyJSON, this);
            IntDictionary.Add(keyCBOR, this);
        }

        public Oauth_Parameter(string keyJSON, CBORType type)
        {
            KeyJson = keyJSON;
            _type = type;
            NameDictionary.Add(keyJSON, this);
        }

        public static explicit operator CBORObject(Oauth_Parameter c)
        {
            return CBORObject.FromObject(c.Key);
        }

        private static CBORObject Identity(CBORObject obj)
        {
            return obj;
        }

        private static CBORObject CnfToJson(CBORObject obj)
        {
            Confirmation cnf = new Confirmation(obj);

            return cnf.ToJSON();
        }
    }
}
