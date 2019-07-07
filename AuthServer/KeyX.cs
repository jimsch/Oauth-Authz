using Com.AugustCellars.CoAP.OAuth;
using Com.AugustCellars.COSE;
using PeterO.Cbor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Com.AugustCellars.CoAP.OSCOAP;

namespace AuthServer
{
    class KeyX
    {
        public string Name { get; }
        public OneKey Key { get;  }
        public List<ProfileIds> Profiles { get; }
        public SecurityContext Context { get; set; }
        public KeyX(CBORObject obj)
        {
            Name = obj["name"].AsString();
            Key = new OneKey(CBORDiagnostics.Parse(obj["Key"].AsString()));
            Profiles = new List<ProfileIds>();
            foreach (CBORObject x in obj["Profiles"].Values) {
                switch (x.AsString().ToLower()) {
                    case "coap_dtls":
                    case "coap-dtls":
                        Profiles.Add(ProfileIds.Coap_Dtls);
                        break;
                    case "coap_oscoap":
                    case "coap-oscoap":
                        Profiles.Add(ProfileIds.Coap_Oscore);
                        break;
                    default:
                        throw new Exception($"Unrecognized profile {x}");
                }
            }
        }
    }
}
