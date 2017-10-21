using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Com.AugustCellars.WebToken;
using PeterO.Cbor;

namespace Com.AugustCellars.CoAP.OAuth
{
    /// <summary>
    /// Introspection response message from the AS->RS
    /// Derived from ietf-draft-oauth-authz-06
    /// </summary>
    public class IntrospecResponse
    {
        private readonly CBORObject _data = CBORObject.NewMap();

        public IntrospecResponse()
        {
            
        }

        public IntrospecResponse(byte[] data)
        {
            CBORObject cbor = CBORObject.DecodeFromBytes(data);
            if (cbor.Type != CBORType.Map) throw new Exception("Invalid data");
            _data = cbor;
        }

        public bool HasKey(CBORObject key)
        {
            return _data.ContainsKey(key);
        }

        public CBORObject GetValue(CBORObject key)
        {
            return _data[key];
        }

        public void SetValue(CBORObject key, CBORObject value)
        {
            _data[key] = value;
        }

        public bool Active
        {
            get => _data["active"].AsBoolean();
            set => _data["active"] = CBORObject.FromObject(value);
        }

        public byte[] ClientToken
        {
            get => _data["client_token"].GetByteString();
            set => _data["client_token"] = CBORObject.FromObject(value);
        }

        public Confirmation Cnf
        {
            get => new Confirmation(_data["cnf"]);
            set => _data["cnf"] = value.AsCBOR;
        }

        public String Profile {
            get => _data["profile"].AsString();
            set => _data["profile"] = CBORObject.FromObject(value);
        }

        public String Scope
        {
            get => _data["scope"].AsString();
            set => _data["scope"] = CBORObject.FromObject(value);
        }

        public byte[] EncodeToBytes()
        {
            return _data.EncodeToBytes();
        }
    }
}
