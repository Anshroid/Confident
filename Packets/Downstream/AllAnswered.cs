using System.Collections.Generic;

namespace Packets.Downstream {
    public sealed class AllAnswered : Packet {
        public byte Id => 0x05;

        public AllAnswered() { }

        public AllAnswered(IEnumerable<byte> data) {
            Deconstruct(data);
        }
    }
}