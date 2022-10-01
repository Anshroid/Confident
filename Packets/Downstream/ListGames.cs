using System;
using System.Collections.Generic;
using System.Linq;

namespace Packets.Downstream {
    public sealed class ListGames : Packet {
        public static byte Id => 0x02;

        public short NumGames => (short)SendGames.Count();
        public IEnumerable<IEnumerable<byte>> SendGames { get; } = new List<List<byte>>();

        public ListGames() { }

        public ListGames(IEnumerable<byte> data) {
            Deconstruct(data);
        }

        public ListGames(IEnumerable<IEnumerable<byte>> games) {
            SendGames = games;
        }

        protected override Packet Deconstruct(IEnumerable<byte> data) {
            return null;
        }

        protected override byte[] Construct() {
            var data = new List<byte> { Id };

            data.AddRange(BitConverter.GetBytes(NumGames));
            SendGames.ToList().ForEach(game => { data.AddRange(game); });

            return data.ToArray();
        }
    }
}