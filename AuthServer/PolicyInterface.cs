using PeterO.Cbor;

namespace AuthServer
{
    public interface PolicyInterface
    {
        //
        //  We need to pass in the following information:
        //  1.  The client id
        //  2.  The Resource Server id
        //  3.  The key used to validate to the AS
        //  4.  The scope asked for
        //  5.  The audience asked for
        //  6.  Client attributes
        //  7.  RS attributes

        bool MakeGrantDecision(PermissionSet requestedPermissions, string clientKeyName, byte[] entityAttributes, CBORObject resourceRules,
                               out PermissionSet responseScope);
    }
}
