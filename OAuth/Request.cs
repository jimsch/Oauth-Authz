using System;
using System.CodeDom;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Com.AugustCellars.WebToken;
using PeterO.Cbor;

namespace Com.AugustCellars.CoAP.OAuth
{

    public enum ProfileIds
    {
        Error_Profile = 0,
        Coap_Dtls = 1,
        Coap_Oscore = 2
    }




    public class Request
    {
        public static readonly uint GrantType_ClientToken = 2;

        private readonly bool _asCBOR;

        private Dictionary<Oauth_Parameter, CBORObject> _values = new Dictionary<Oauth_Parameter, CBORObject>();

        public Request(uint grant_type, bool asCBOR = true)
        {
            if (grant_type > 3) throw new Exception("Unknown grant type value");
            if (grant_type != GrantType_ClientToken) throw new Exception("Unsupported grant type value");

            Grant_Type = (int) grant_type;
            _asCBOR = asCBOR;
        }

        public Request(string grant_type, bool asCBOR = true)
        {

            uint itemType = 0;

            switch (grant_type) {
                case "client_credentials":
                    itemType = GrantType_ClientToken;
                    break;

                default:
                    throw new Exception("Unknown or supported grant type value");
            }

            Grant_Type =(int) itemType;
        }

        public Request(byte[] data) : this(CBORObject.DecodeFromBytes(data), true)
        {
        }

        private Request(CBORObject req, bool asCBOR)
        {
            if (req.Type != CBORType.Map) throw new Exception("Incorrect request data structure");

            _values = new Dictionary<Oauth_Parameter, CBORObject>();
            foreach (CBORObject key in req.Keys) {
                if (key.Type == CBORType.Number) {
                    _values.Add(Oauth_Parameter.IntDictionary[key.AsInt32()], req[key]);
                }
                else if (key.Type == CBORType.TextString) {
                    Oauth_Parameter p;
                    if (!Oauth_Parameter.NameDictionary.TryGetValue(key.AsString(), out p)) {
                        p = new Oauth_Parameter(key.AsString(), req[key].Type);
                    }
                    _values.Add(p, req[key]);


                }
                else {
                    throw new Exception("Invalid key in the request");
                }
            }
            _asCBOR = asCBOR;
        }

        public static Request FromCBOR(byte[] data)
        {
            CBORObject obj = CBORObject.DecodeFromBytes(data);
            if (obj.Type != CBORType.Map) throw new Exception("Incorrect request data structure");

            return new Request(obj, true);
        }

        public static Request FromJSON(string data)
        { 
            CBORObject obj = CBORObject.FromJSONString(data);
            if (obj.Type != CBORType.Map) throw new Exception("Incorrect request data struture");

            Request req = new Request(obj, false);
            return req;
        }


        private static readonly string[] GrantTypeNames = {
            "password", "authorization_code", "client_credentials", "refresh_token"
        };

        public int Grant_Type {
            get {
                if (!_values.ContainsKey(Oauth_Parameter.Grant_Type)) {
                    throw new Exception("No Grant_Type in request");
                }
                CBORObject obj = _values[Oauth_Parameter.Grant_Type];
                if (obj.Type == CBORType.Number) return obj.AsInt32();
                for (int i = 0; i < GrantTypeNames.Length; i++) {
                    if (obj.AsString() == GrantTypeNames[i]) {
                        return i;
                    }
                }
                throw new Exception("Can't map integer value to string");
            }
            set => SetValue(Oauth_Parameter.Grant_Type, CBORObject.FromObject(value));
        }

        public string Audience {
            get {
                if (!_values.ContainsKey(Oauth_Parameter.Aud)) return null;
                return _values[Oauth_Parameter.Aud].AsString();
            }
            set => SetValue(Oauth_Parameter.Aud, CBORObject.FromObject(value));
        }

        public String Grant_Type_Text {
            get
            {
                if (!_values.ContainsKey(Oauth_Parameter.Grant_Type)) return null;
                CBORObject grant = _values[Oauth_Parameter.Grant_Type];
                if (grant.Type == CBORType.TextString) return grant.AsString();
                if (grant.AsInt32() >= GrantTypeNames.Length) {
                    throw new Exception("Unknown grant_type of '" + grant.AsInt32() + "'");
                }

                return GrantTypeNames[grant.AsInt32()];
            }
        }

        public CBORObject Scope
        {
            get {
                if (!_values.ContainsKey(Oauth_Parameter.Scope)) return null;
                return _values[Oauth_Parameter.Scope];
            }
            set {
                if (value.Type != CBORType.ByteString && value.Type != CBORType.TextString) {
                    throw new ArgumentException("Incorrect type for value");
                }
                SetValue(Oauth_Parameter.Scope, value);
            }
        }

        public Confirmation Cnf
        {
            get {

                if (!_values.ContainsKey(Oauth_Parameter.Cnf)) return null;
                return new Confirmation(_values[Oauth_Parameter.Cnf]);
            }
            set => SetValue(Oauth_Parameter.Cnf, value.AsCBOR);
        }


        public byte[] EncodeToBytes()
        {
            CBORObject obj = CBORObject.NewMap();
            foreach (KeyValuePair<Oauth_Parameter, CBORObject> kv in _values) {
                obj.Add(kv.Key.Key, kv.Value);
            }
            return obj.EncodeToBytes();
        }

        public string EncodeToString()
        {
            CBORObject obj = CBORObject.NewMap();
            foreach (KeyValuePair<Oauth_Parameter, CBORObject> kv in _values) {
                CBORObject newValue = kv.Value;
                if (kv.Key == Oauth_Parameter.Grant_Type) {
                    if (newValue.Type == CBORType.Number) {
                        newValue = CBORObject.FromObject(Grant_Type_Text);
                    }
                }

                if (newValue.Type == CBORType.ByteString) {
                    char[] rgch = new char[newValue.GetByteString().Length*2];
                    Convert.ToBase64CharArray(newValue.GetByteString(), 0, newValue.GetByteString().Length, rgch, 0,
                                              Base64FormattingOptions.None);
                    newValue = CBORObject.FromObject(rgch.ToString());
                }
                obj.Add(kv.Key.KeyJson, newValue);
            }
            return obj.ToJSONString();
        }

        public bool ContainsKey(Oauth_Parameter key)
        {
            return _values.ContainsKey(key);
        }

        public CBORObject GetValue(Oauth_Parameter key)
        {
            if (_values.ContainsKey(key)) {
                return _values[key];
            }

            throw new Exception("Key not in request");
        }


        public void SetValue(Oauth_Parameter key, CBORObject value)
        {
            if (_values.ContainsKey(key)) {
                _values[key] = value;
            }
            else {
                _values.Add(key, value);
            }
        }

    }
}
