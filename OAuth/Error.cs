using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using PeterO.Cbor;

namespace Com.AugustCellars.CoAP.OAuth
{
    public class Error
    {
        CBORObject _obj = CBORObject.NewMap();

        public Error(int error_code)
        {
            _obj[15] = CBORObject.FromObject(error_code);
        }

        public Error(byte[] data)
        {
            _obj = CBORObject.DecodeFromBytes(data);
            if (_obj.Type != CBORType.Map) throw new Exception("Invalid error structure");
        }

        public String Description {
            get {
                if (!_obj.HasTag(16)) return null;
                return _obj[16].AsString();
            }
            set { _obj[16] = CBORObject.FromObject(value); }
        }

        public String URI {
            get {
                if (!_obj.HasTag(17)) return null;
                return _obj[17].AsString();
            }
            set { _obj[17] = CBORObject.FromObject(value); }
        }

        public byte[] EncodeToBytes()
        {
            return _obj.EncodeToBytes();
        }
    }
}
