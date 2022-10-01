using System;
using System.Collections.Generic;

namespace Packets.Downstream {
    public sealed class GameWon : Packet {
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