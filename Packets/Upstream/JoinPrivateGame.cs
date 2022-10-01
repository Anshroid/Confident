using System;
using System.Collections.Generic;

namespace Packets.Upstream {
    public sealed class JoinPrivateGame : Packet {
        public byte Id => 0x03;

        public JoinPrivateGame() { throw new NotImplementedException(); }

        public JoinPrivateGame(IEnumerable<byte> data) {
            Deconstruct(data);
        }
    }
}