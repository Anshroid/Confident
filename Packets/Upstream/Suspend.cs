using System.Collections.Generic;

namespace Packets.Upstream {
    public sealed class Suspend : Packet {
        public byte Id => 0xFF;

        public Suspend() { }

        public Suspend(IEnumerable<byte> data) {
            Deconstruct(data);
        }
    }
}