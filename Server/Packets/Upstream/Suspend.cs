using System.Collections.Generic;

namespace Server.Packets.Upstream {
    internal sealed class Suspend : Packet {
        public byte Id => 0xFF;

        public Suspend() { }

        public Suspend(IEnumerable<byte> data) {
            Deconstruct(data);
        }
    }
}