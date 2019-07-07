using PeterO.Cbor;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthServer
{
    public class Scope
    {
        public PermissionSet Permissions { get; }
    
        public Scope(CBORObject cbor)
        {
            if (cbor.Type == CBORType.TextString) {
                Permissions = new PermissionSet(cbor.AsString());
            }
            else if (cbor.Type == CBORType.Array) {
                Permissions = new PermissionSet(cbor);
            }
            else {
                throw new Exception("Wrong format for Scope");
            }
        }

        public PermissionSet LookupPermissions(string keyName, PermissionSet requestedPermissions)
        {
            if (!Permissions.Allows(requestedPermissions)) return null;
            return Permissions;
        }

        public CBORObject AsCbor()
        {
            return Permissions.AsCBOR();
        }
    }
}
