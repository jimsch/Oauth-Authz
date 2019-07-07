using PeterO.Cbor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Com.AugustCellars.CoAP;

namespace AuthServer
{
    public class Permission
    {
        public string Tags;
        public List<Method> Methods = new List<Method>();
        public bool AllowsAll { get; }

        public Permission(CBORObject obj)
        {
            if (obj.Type == CBORType.TextString) {
                Tags = obj.AsString();
                SetMethod(Tags);
            }
            else if (obj.Type == CBORType.Array) {
                Tags = obj[0].AsString();
                foreach (CBORObject o in obj[1].Values) {
                    SetMethod(o.AsString());
                }
            }
            else {
                throw new FormatException("Expected Array or TextString");
            }
        }

        public Permission(string tag, Method[] methods)
        {
            Methods.AddRange(methods);
            Tags = tag;
        }

        public Permission(string tag, Method methods)
        {
            Methods.Add(methods);
            Tags = tag;
        }

        public Permission(string tag)
        {
            Tags = tag;
            AllowsAll = true;
        }

        public bool Allows(Permission request)
        {
            if (Tags != request.Tags) return false;

            if (AllowsAll) return true;

            foreach (Method m in request.Methods) {
                if (!Methods.Contains(m)) return false;
            }

            return true;
        }

        public CBORObject AsCBOR()
        {
            CBORObject root = CBORObject.NewArray();
            root.Add(Tags);

            CBORObject m1 = CBORObject.NewArray();
            foreach (Method m in Methods) {
                m1.Add(m);
            }
            root.Add(m1);
            return root;
        }

        private void SetMethod(string str)
        {

            switch (str.ToUpper()) {
            case "GET":
                Methods.Add(Method.GET);
                break;

            case "PUT":
                Methods.Add(Method.PUT);
                break;

            case "POST":
                Methods.Add(Method.POST);
                break;

            default:
                throw new NotImplementedException($"Method {str} unrecognized");
            }
        }
    }

public class PermissionSet
    {
        public static PermissionSet AllPermissionSet = new PermissionSet("ALL");
        public List<Permission> Permissions { get; } = new List<Permission>();

        public PermissionSet(CBORObject obj)
        {
            if (obj.Type == CBORType.Array) {
                if (obj[0].Type == CBORType.Array) {
                    foreach (CBORObject x in obj.Values) {
                        Permission p = new Permission(x);
                        Permissions.Add(p);
                    }
                }
                else if (obj[0].Type == CBORType.TextString) {
                    Permission p = new Permission(obj);
                    Permissions.Add(p);
                }
                else {
                    throw new Exception("Invalid Scope");
                }
            }
            else {
                throw new Exception("Invalid Scope");
            }
        }

        /// <summary>
        /// What string formats are wanted 
        /// "string"
        /// </summary>
        /// <param name="permissions"></param>
        public PermissionSet(string permissions)
        {
            Permissions.Add(new Permission(permissions));
        }

        public PermissionSet IntersectWith(PermissionSet setB)
        {
            if (Permissions.Count != 1 || setB.Permissions.Count != 1) {
                throw new Exception("Incorrect number of permissions");
            }

            return null;
        }

        public bool IsEmpty
        {
            get => false;
        }

        public bool Allows(PermissionSet request)
        {
            foreach (Permission p1 in request.Permissions) {
                bool found = false;
                foreach (Permission p in Permissions) {
                    if (p.Allows(p1)) {
                        found = true;
                        break;
                    }
                }

                if (!found) return false;
            }
            return true;
        }

        public CBORObject AsCBOR()
        {
            if (Permissions.Count == 1) {
                return Permissions[0].AsCBOR();
            }
            else {
                CBORObject obj = CBORObject.NewArray();

                foreach (Permission p in Permissions) {
                    obj.Add(p.AsCBOR());
                }

                return obj;
            }
        }
    }
}
