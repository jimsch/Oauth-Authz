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
            get => _data[CBORObject.FromObject(  27)].GetByteString();
            set => _data[CBORObject.FromObject(27)] = CBORObject.FromObject(value);
        }

        public CBORObject TokenTypeHint
        {
            get => _data[CBORObject.FromObject(28)];
            set => _data[CBORObject.FromObject(28)] = value;
        }

        public byte[] EncodeToBytes()
        {
            return _data.EncodeToBytes();
        }
    }
}
