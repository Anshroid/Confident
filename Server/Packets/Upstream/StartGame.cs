using System.Collections.Generic;

namespace Server.Packets.Upstream {
    internal sealed class StartGame : Packet {
        public byte Id => 0x05;
        
        public StartGame() { }

        public StartGame(IEnumerable<byte> data) {
            Deconstruct(data);
        }
    }
}