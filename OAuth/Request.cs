using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using PeterO.Cbor;

namespace Com.AugustCellars.CoAP.OAuth
{
    public class Request
    {
        public static readonly uint GrantType_ClientToken = 2;

        private readonly CBORObject _objRequest = CBORObject.NewMap();

        public Request(uint grant_type)
        {
            if (grant_type > 3) throw new Exception("Unknown grant type value");
            if (grant_type != GrantType_ClientToken) throw new Exception("Unsupported grant type value");

            _objRequest[Oauth_Parameter.Grant_Type.Key] = CBORObject.FromObject(grant_type);
        }

        public Request(string grant_type)
        {
            uint itemType = 0;

            switch (grant_type) {
                case "client_credentials":
                    itemType = GrantType_ClientToken;
                    break;

                default:
                    throw new Exception("Unknown or supported grant type value");
            }

            _objRequest[(CBORObject) Oauth_Parameter.Grant_Type] = CBORObject.FromObject(itemType);
        }

        public Request(byte[] data)
        {
            CBORObject obj = CBORObject.DecodeFromBytes(data);
            if (obj.Type != CBORType.Map) throw new Exception("Incorrect request data structure");

            _objRequest = obj;
        }

        public int? Grant_Type {
            get {
                if (!_objRequest.ContainsKey((CBORObject) Oauth_Parameter.Grant_Type)) return null;
                CBORObject obj = _objRequest[(CBORObject) Oauth_Parameter.Grant_Type];
                if (obj.Type == CBORType.Number) return obj.AsInt32();
                switch (obj.AsString()) {
                    case "password": return 0;
                    case "authoriziation_code": return 1;
                    case "client_credentials": return 2;
                    case "referesh_token": return 3;
                    default: throw new Exception("Can't map integer value to string");
                }
            }
        }

        public String Audience {
            get {
                if (!_objRequest.ContainsKey((CBORObject) Oauth_Parameter.Aud)) return null;
                return _objRequest[(CBORObject) Oauth_Parameter.Aud].AsString();
            }
            set => _objRequest[(CBORObject) Oauth_Parameter.Aud] = CBORObject.FromObject(value);
        }

        public String Grant_Type_Text {
            get
            {
                if (!_objRequest.ContainsKey((CBORObject) Oauth_Parameter.Grant_Type)) return null;
                CBORObject grant = _objRequest[(CBORObject) Oauth_Parameter.Grant_Type];
                if (grant.Type == CBORType.TextString) return grant.AsString();
                switch (grant.AsInt32()) {
                    case 0: return "password";
                    case 1: return "authorization_code";
                    case 2: return "client_credentials";
                    case 3: return "refresh_token";
                    default: throw new Exception("Unknown grant_type of '" + grant.AsInt32() + "'");
                }
            }
        }

        public String Profile {
            get {
                if (!_objRequest.ContainsKey((CBORObject)Oauth_Parameter.Profile)) return null;
                return _objRequest[(CBORObject)Oauth_Parameter.Profile].AsString();
            }
            set => _objRequest[(CBORObject)Oauth_Parameter.Profile] = CBORObject.FromObject(value);
        }


        public String Scope {
            get {
                if (!_objRequest.ContainsKey((CBORObject) Oauth_Parameter.Scope)) return null;
                return _objRequest[(CBORObject) Oauth_Parameter.Scope].AsString();
            }
            set => _objRequest[(CBORObject) Oauth_Parameter.Scope] = CBORObject.FromObject(value);
        }


        public byte[] EncodeToBytes()
        {
            return _objRequest.EncodeToBytes();
        }

        public CBORObject GetValue(int key)
        {
            if (_objRequest.HasTag(key)) return _objRequest[key];
            return null;
        }

        public void SetValue(int key, CBORObject value)
        {
            _objRequest[key] = value;
        }

    }
}
