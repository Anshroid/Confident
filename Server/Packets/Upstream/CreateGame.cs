using System.Collections.Generic;

namespace Server.Packets.Upstream {
    internal sealed class CreateGame : Packet {
        public byte Id => 0x04;

        public bool Private;

        public CreateGame() { }

        public CreateGame(IEnumerable<byte> data) {
            Deconstruct(data);
        }

        public CreateGame(bool pPrivate) {
            Private = pPrivate;
        }
    }
}