using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Com.AugustCellars.CoAP;
using Com.AugustCellars.CoAP.Server.Resources;
using PeterO.Cbor;
using Com.AugustCellars.CoAP.OAuth;

namespace ResourceServer
{
    class EchoResource : Resource
    {
        private byte[] _unsecureResponse;

        public EchoResource() : base("echo")
        {
            AsInfo asInfo = new AsInfo() {
                Nonce = new byte[] {0xe0, 0xa1, 0x56, 0xbb, 0x3f},
                ASServer = "coaps://localhost:5689/token"
            };
            _unsecureResponse = asInfo.EncodeToBytes();
        }

        protected override void DoGet(CoapExchange exchange)
        {
            //  Check to see if we are "secure" in some fashion

            if (exchange.Request.OscoapContext == null && !(exchange.Request.Session is ISecureSession)) {
                exchange.Respond(StatusCode.Unauthorized, _unsecureResponse, 65008);
                return;
            }

            exchange.Respond("Looks like you made it");
        }
    }
}
