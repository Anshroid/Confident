using System;
using System.Collections.Generic;

namespace Server.Packets.Downstream {
    internal sealed class GameWon : Packet {
        public byte Id => 0x07;
        
        public Guid Winner { get; set; }

        public GameWon() { }

        public GameWon(IEnumerable<byte> data) {
            Deconstruct(data);
        }

        public GameWon(Guid winner) {
            Winner = winner;
        }
    }
}