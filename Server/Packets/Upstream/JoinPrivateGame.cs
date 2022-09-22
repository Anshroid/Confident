using System;
using System.Collections.Generic;

namespace Server.Packets.Upstream {
    internal sealed class JoinPrivateGame : Packet {
        public byte Id => 0x03;

        public JoinPrivateGame() { throw new NotImplementedException(); }

        public JoinPrivateGame(IEnumerable<byte> data) {
            Deconstruct(data);
        }
    }
}