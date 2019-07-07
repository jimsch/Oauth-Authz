using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PeterO.Cbor;

namespace AuthServer
{
    class Audience
    {
        public static readonly List<Audience> GlobalAudienceList = new List<Audience>();
        private readonly List<string> _audienceNames = new List<string>();
        private readonly List<Scope> _scopeList = new List<Scope>();

        public Audience(CBORObject map)
        {
            foreach (CBORObject o in map["AudienceName"].Values) _audienceNames.Add(o.AsString());
            foreach (CBORObject o in map["Scopes"].Values) {
                Scope s = new Scope(o);
                _scopeList.Add(s);
            }
        }

        public bool HasAudience(string audIn)
        {
            return _audienceNames.Contains(audIn);
        }

        public bool AllowNoScope { get; } = false;

        public int ScopeCount
        {
            get => _scopeList.Count;
        }

        public Scope LookupPermissions(string keyName, PermissionSet requestedPermissions)
        {
            PermissionSet x;
            foreach (Scope s in _scopeList) {
                x = s.LookupPermissions(keyName, requestedPermissions);
                if (x != null) return s;
            }

            return null;
        }
    }
}
