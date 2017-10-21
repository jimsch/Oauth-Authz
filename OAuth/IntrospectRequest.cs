using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PeterO.Cbor;

namespace Com.AugustCellars.CoAP.OAuth
{
    public class IntrospectRequest
    {
        private readonly CBORObject _data = CBORObject.NewMap();

        public IntrospectRequest()
        {
            
        }

        public IntrospectRequest(byte[] initData)
        {
            CBORObject obj = CBORObject.DecodeFromBytes(initData);
            if (obj.Type != CBORType.Map) throw new Exception("Incorrect data type");
            _data = obj;
        }

        public byte[] Token
        {
            get => _data["token"].GetByteString();
            set => _data["token"] = CBORObject.FromObject(value);
        }

        public CBORObject TokenTypeHint
        {
            get => _data["token_type_hint"];
            set => _data["token_type_hint"] = value;
        }

        public byte[] EncodeToBytes()
        {
            return _data.EncodeToBytes();
        }
    }
}
