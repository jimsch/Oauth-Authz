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
        private Dictionary<Oauth_Parameter, CBORObject> _values = new Dictionary<Oauth_Parameter, CBORObject>();

        public Response(Request request)
        {
            _request = request;
        }

        private Response(CBORObject resp)
        {
            if (resp.Type != CBORType.Map) throw new Exception("Invalid Encoding");

            _values = new Dictionary<Oauth_Parameter, CBORObject>();
            foreach (CBORObject key in resp.Keys) {
                if (key.Type == CBORType.Number) {
                    _values.Add(Oauth_Parameter.IntDictionary[key.AsInt32()], resp[key]);
                }
                else if (key.Type == CBORType.TextString) {
                    Oauth_Parameter p;
                    if (!Oauth_Parameter.NameDictionary.TryGetValue(key.AsString(), out p)) {
                        p = new Oauth_Parameter(key.AsString(), resp[key].Type);
                    }
                    _values.Add(p, resp[key]);
                }
                else {
                    throw new Exception("Invalid key in the request");
                }
            }
        }

        public static Response FromCBOR(byte[] cbor)
        {
            CBORObject obj = CBORObject.DecodeFromBytes(cbor);
            return new Response(obj);
        }

        public static Response FromJSON(string json)
        {
            CBORObject obj = CBORObject.FromJSONString(json);
            return new Response(obj);
        }

        public  CBORObject this[Oauth_Parameter key]
        {
            get => _values[key];
            set => _values[key] = value;
        }

        public bool ContainsKey(Oauth_Parameter key)
        {
            return _values.ContainsKey(key);
        }

        public byte[] EncodeToBytes()
        {
            CBORObject obj = CBORObject.NewMap();
            foreach (KeyValuePair<Oauth_Parameter, CBORObject> kv in _values) {
                obj.Add(kv.Key.Key, kv.Value);
            }
            return obj.EncodeToBytes();
        }

        public string EncodeToJson()
        {
            CBORObject obj = CBORObject.NewMap();
            foreach (KeyValuePair<Oauth_Parameter, CBORObject> kv in _values) {
                CBORObject newValue = kv.Value;

                if (kv.Key.ToJSON != null) {
                    newValue = kv.Key.ToJSON(newValue);
                }

                if (newValue.Type == CBORType.ByteString) {
                    char[] rgch = new char[newValue.GetByteString().Length * 2];
                    Convert.ToBase64CharArray(newValue.GetByteString(), 0, newValue.GetByteString().Length, rgch, 0,
                                              Base64FormattingOptions.None);
                    newValue = CBORObject.FromObject(rgch.ToString());
                }



                obj.Add(kv.Key.KeyJson, newValue);
            }
            return obj.ToJSONString();
        }

        public ProfileIds Profile {
            get => (ProfileIds) _values[Oauth_Parameter.Profile].AsInt32();
            set => _values[Oauth_Parameter.Profile] = CBORObject.FromObject(value);
        }

        public byte[] Token
        {
            get => _values[Oauth_Parameter.Access_Token].GetByteString();
            set => _values[Oauth_Parameter.Access_Token] = CBORObject.FromObject(value);
        }

        public Confirmation Confirmation
        {
            get {
                if (_values.ContainsKey(Oauth_Parameter.Cnf)) {
                    return new Confirmation(_values[Oauth_Parameter.Cnf]);
                }
                else return null;
            }
            set => _values[Oauth_Parameter.Cnf] = value.AsCBOR;
        }

        public Confirmation RsConfirmation
        {
            get {
                if (_values.ContainsKey(Oauth_Parameter.Rs_cnf)) {
                    return new Confirmation(_values[Oauth_Parameter.Rs_cnf]);
                }
                else return null;
            }
            set => _values[Oauth_Parameter.Rs_cnf] = value.AsCBOR;
        }
    }
}
