using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using PeterO.Cbor;

namespace Com.AugustCellars.CoAP.OAuth
{
    public class Oauth_Parameter
    {
        public static Oauth_Parameter Aud = new Oauth_Parameter(3, CBORType.TextString);
        public static Oauth_Parameter Client_id = new Oauth_Parameter(8, CBORType.TextString);
        public static Oauth_Parameter Client_secret = new Oauth_Parameter(9, CBORType.ByteString);
        public static Oauth_Parameter Response_type = new Oauth_Parameter(10, CBORType.TextString);
        public static Oauth_Parameter Redirect_uri = new Oauth_Parameter(11, CBORType.TextString);
        public static Oauth_Parameter Scope = new Oauth_Parameter(12, CBORType.TextString);
        public static Oauth_Parameter State = new Oauth_Parameter(13, CBORType.TextString);
        public static Oauth_Parameter Code = new Oauth_Parameter(14, CBORType.ByteString);
        public static Oauth_Parameter Error = new Oauth_Parameter(15, CBORType.TextString);
        public static Oauth_Parameter Error_Description = new Oauth_Parameter(16, CBORType.TextString);
        public static Oauth_Parameter Error_URI =new Oauth_Parameter(17, CBORType.TextString);
        public static Oauth_Parameter Grant_Type = new Oauth_Parameter(18, CBORType.Number);
        public static Oauth_Parameter Access_Token = new Oauth_Parameter(19, CBORType.ByteString);
        public static Oauth_Parameter Token_Type = new Oauth_Parameter(20, 0);
        public static Oauth_Parameter Expires_In = new Oauth_Parameter(21, 0);
        public static Oauth_Parameter Username = new Oauth_Parameter(22, CBORType.TextString);
        public static Oauth_Parameter Password = new Oauth_Parameter(23, CBORType.TextString);
        public static Oauth_Parameter Refresh_Token = new Oauth_Parameter(24, CBORType.ByteString);
        public static Oauth_Parameter Cnf = new Oauth_Parameter(25, CBORType.Map);
        public static Oauth_Parameter Profile = new Oauth_Parameter(26, CBORType.TextString);

        private readonly CBORType _type;

        public int Key { get; private set; }

        private Oauth_Parameter(int key, CBORType type)
        {
            Key = key;
            _type = type;
        }

        public static explicit operator CBORObject(Oauth_Parameter c)
        {
            return CBORObject.FromObject(c.Key);
        }
    }
}
