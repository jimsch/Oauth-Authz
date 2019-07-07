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

        /// <summary>
        /// Make a shallow copy so that a new map exists,
        /// M00BUG does not make a copy of the values, but re-uses the same value object
        /// </summary>
        /// <param name="copy"></param>
        public AsInfo(AsInfo copy)
        {
            _map = CBORObject.NewMap();
            foreach (CBORObject key in copy._map.Keys) {
                _map.Add(key, copy._map[key]);
            }
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
