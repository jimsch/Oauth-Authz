using PeterO.Cbor;

namespace AuthServer
{
    public class EvaluatePolicy : PolicyInterface
    {
        public bool MakeGrantDecision(PermissionSet requestedPermissions, string clientKeyName, byte[] entityAttributes, CBORObject resourceRules,
                               out PermissionSet responseScope)
        {
            responseScope = null;

            // 1.  Extract from resourceRules 
            //           scope & rules
            // 2.  

            CBORObject scope = resourceRules["Scope"];
            CBORObject keys = resourceRules["Rules"]["keys"];

            bool found = false;
            foreach (CBORObject keyName in keys.Values) {
                if (keyName.AsString() == clientKeyName) {
                    found = true;
                    break;
                }
            }

            if (!found) {
                return false;
            }




            PermissionSet resourcePermissions = new PermissionSet(scope);

            responseScope = resourcePermissions.IntersectWith(requestedPermissions);
            if (responseScope.IsEmpty) {
                return false;
            }

            return true;
        }
    }
}
