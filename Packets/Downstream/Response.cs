using System.Collections.Generic;

namespace Packets.Downstream {
    public sealed class Response : Packet {
        public static byte Id => 0x00;

        public bool Success { get; set; }

        public Response() { }

        public Response(bool success) {
            this.Success = success;
        }

        public Response(IEnumerable<byte> data) {
            Deconstruct(data);
        }
    }
}