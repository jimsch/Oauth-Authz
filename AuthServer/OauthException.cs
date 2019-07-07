using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Com.AugustCellars.CoAP;
using Oauth = Com.AugustCellars.CoAP.OAuth;
using PeterO.Cbor;

namespace AuthServer
{
    class OauthException : Exception
    {
        public StatusCode Error { get; }
        public byte[] Body { get; }
        public OauthException(StatusCode status, OauthResource.ErrorCodes errorCode, string diagnostics = null)
        {
            CBORObject error = CBORObject.NewMap();

            error.Set(Oauth.Oauth_Parameter.Error.Key, CBORObject.FromObject(errorCode));
            if (diagnostics != null) {
                error.Set(Oauth.Oauth_Parameter.Error_Description.Key, CBORObject.FromObject(diagnostics));
            }

            Error = status;
            Body = error.EncodeToBytes();

        }
    }
}
