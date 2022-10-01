using System;
using System.Collections.Generic;

namespace Packets.Upstream {
    public sealed class JoinGame : Packet {
        public byte Id => 0x02;

        public Guid GameId { get; set; }

        public JoinGame() { }

        public JoinGame(IEnumerable<byte> data) {
            Deconstruct(data);
        }

        public JoinGame(Guid gameId) {
            GameId = gameId;
        }
    }
}