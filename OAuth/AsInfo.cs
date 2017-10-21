using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PeterO.Cbor;

namespace Com.AugustCellars.CoAP.OAuth
{
    public class AsInfo
    {
        private readonly CBORObject _map;

        public AsInfo()
        {
            _map = CBORObject.NewMap();
        }

        public AsInfo(byte[] encodedBytes)
        {
            _map = CBORObject.DecodeFromBytes(encodedBytes);
        }

     

        public string ASServer
        {
            get => _map[CBORObject.FromObject(0)].AsString();
            set => _map.Add(0, value);
        }

        public byte[] Nonce
        {
            get => _map[CBORObject.FromObject(5)].GetByteString();
            set => _map.Add(5, value);
        }

        public byte[] EncodeToBytes()
        {
            return _map.EncodeToBytes();
        }
    }
}
