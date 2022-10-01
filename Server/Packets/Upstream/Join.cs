using System.Collections.Generic;

namespace Server.Packets.Upstream {
    internal sealed class Join : Packet {
        public static byte Id => 0x00;

        public short NameLength { get; set; }
        public string Name { get; set; } = "";

        public Join() { }

        public Join(IEnumerable<byte> data) {
            Deconstruct(data);
        }
    }
}