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
            get => _data[CBORObject.FromObject(29)].AsBoolean();
            set => _data[CBORObject.FromObject(29)] = CBORObject.FromObject(value);
        }

        public string Audience
        {
            get => _data[CBORObject.FromObject(3)].AsString();
            set => _data[CBORObject.FromObject(3)] = CBORObject.FromObject(value);
        }

        public byte[] ClientToken
        {
            get => _data["client_token"].GetByteString();
            set => _data["client_token"] = CBORObject.FromObject(value);
        }

        public Confirmation Cnf
        {
            get => new Confirmation(_data[CBORObject.FromObject(25)]);
            set => _data[CBORObject.FromObject(25)] = value.AsCBOR;
        }

        public int Profile {
            get => _data[CBORObject.FromObject(26)].AsInt32();
            set => _data[CBORObject.FromObject(26)] = CBORObject.FromObject(value);
        }

        public CBORObject Scope
        {
            get => _data[CBORObject.FromObject(12)];
            set => _data[CBORObject.FromObject(12)] = value;
        }

        public byte[] EncodeToBytes()
        {
            return _data.EncodeToBytes();
        }
    }
}
