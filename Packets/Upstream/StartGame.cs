using System.Collections.Generic;

namespace Packets.Upstream {
    public sealed class StartGame : Packet {
        public byte Id => 0x05;
        
        public StartGame() { }

        public StartGame(IEnumerable<byte> data) {
            Deconstruct(data);
        }
    }
}