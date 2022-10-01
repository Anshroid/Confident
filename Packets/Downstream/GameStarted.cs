using System.Collections.Generic;

namespace Packets.Downstream {
    public sealed class GameStarted : Packet {
        public byte Id => 0x03;

        public GameStarted() { }

        public GameStarted(IEnumerable<byte> data) {
            Deconstruct(data);
        }
    }
}