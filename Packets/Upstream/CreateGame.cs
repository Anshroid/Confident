using System.Collections.Generic;

namespace Packets.Upstream {
    public sealed class CreateGame : Packet {
        public byte Id => 0x04;

        public bool Private { get; set; }

        public CreateGame() { }

        public CreateGame(IEnumerable<byte> data) {
            Deconstruct(data);
        }

        public CreateGame(bool pPrivate) {
            Private = pPrivate;
        }
    }
}