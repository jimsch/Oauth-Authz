using PeterO.Cbor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthServer
{
    class RuleSet
    {
        public List<string> Keys = new List<string>();
        public RuleSet(CBORObject obj)
        {
            if (obj.ContainsKey("keys")) {
                foreach (CBORObject o in obj["keys"].Values) {
                    Keys.Add(o.AsString());
                }
            }
        }

        public bool Allows(string keyName)
        {
            return Keys.Contains(keyName);
        }
    }
}
