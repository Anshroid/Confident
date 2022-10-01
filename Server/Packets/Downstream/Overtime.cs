using System;
using System.Collections.Generic;
using System.Linq;

namespace Server.Packets.Downstream {
    internal sealed class Overtime : Packet {
        public byte Id => 0x08;

        public short NumWinners => (short) Winners.Count();
        public IEnumerable<Guid> Winners;

        public Overtime() { }

        public Overtime(IEnumerable<byte> data) {
            Deconstruct(data);
        }

        public Overtime(IEnumerable<Guid> tiedWinners) {
            Winners = tiedWinners;
        }

        protected override byte[] Construct() {
            var data = new List<byte> { Id };

            data.AddRange(BitConverter.GetBytes(NumWinners));
            Winners.ToList().ForEach(player => { data.AddRange(player.ToByteArray()); });

            return data.ToArray();
        }

        protected override Packet Deconstruct(IEnumerable<byte> packet) {
            return null;
        }
    }
}