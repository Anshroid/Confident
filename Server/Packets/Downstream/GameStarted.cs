using System.Collections.Generic;

namespace Server.Packets.Downstream {
    internal sealed class GameStarted : Packet {
        public byte Id => 0x03;

        public GameStarted() { }

        public GameStarted(IEnumerable<byte> data) {
            Deconstruct(data);
        }
    }
}