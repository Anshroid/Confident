using System;
using System.Collections.Generic;

namespace Server.Packets.Upstream {
    internal sealed class JoinGame : Packet {
        public byte Id => 0x02;

        public Guid GameId;

        public JoinGame() { }

        public JoinGame(IEnumerable<byte> data) {
            Deconstruct(data);
        }

        public JoinGame(Guid gameId) {
            GameId = gameId;
        }
    }
}