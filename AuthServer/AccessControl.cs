using PeterO.Cbor;

namespace AuthServer
{
    public class AccessControl
    {
        private Scope scope;
        private RuleSet rules;

        public AccessControl(CBORObject cbor)
        {
            if (cbor.ContainsKey("Scope")) {
                scope = new Scope(cbor["Scope"]);
            }

            if (cbor.ContainsKey("Rules")) {
                rules = new RuleSet(cbor["Rules"]);
            }
        }

        public Scope Evaluate(string subjectKeyName)
        {
            if (rules.Allows(subjectKeyName)) {
                return scope;
            }
            return null;
        }
    }
}
