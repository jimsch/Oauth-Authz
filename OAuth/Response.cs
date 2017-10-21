using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PeterO.Cbor;

using Com.AugustCellars.WebToken;

namespace Com.AugustCellars.CoAP.OAuth
{
    public class Response
    {
        private Request _request;
        private CBORObject _map = CBORObject.NewMap();

        public Response(Request request)
        {
            _request = request;
        }

        public Response(byte[] encoded)
        {
            _map = CBORObject.DecodeFromBytes(encoded);
            if (_map.Type != CBORType.Map) throw new Exception("Invalid Encoding");


        }

        public  CBORObject this[CBORObject key]
        {
            get => _map[key];
            set => _map[key] = value;
        }


        public byte[] BuildReply()
        {
            CBORObject obj = CBORObject.NewMap() ;

            // Set Profile
            // Set cnf
            // Set access_token
            // Set expires_in



            return obj.EncodeToBytes();
        }

        public byte[] EncodeToBytes()
        {
            return _map.EncodeToBytes();
        }

        public string Profile {
            get => _map[(CBORObject)Oauth_Parameter.Profile].AsString();
            set => _map[(CBORObject)Oauth_Parameter.Profile] = CBORObject.FromObject(value);
        }


        public byte[] Token
        {
            get => _map[(CBORObject) Oauth_Parameter.Access_Token].GetByteString();
            set => _map[(CBORObject) Oauth_Parameter.Access_Token] = CBORObject.FromObject(value);
        }

        public Confirmation Confirmation
        {
            get => new Confirmation(_map[(CBORObject) Oauth_Parameter.Cnf]);
            set => _map[(CBORObject)Oauth_Parameter.Cnf] = value.AsCBOR;
        }
    }
}
