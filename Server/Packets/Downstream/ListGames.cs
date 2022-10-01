using System;
using System.Collections.Generic;
using System.Linq;

namespace Server.Packets.Downstream {
    internal sealed class ListGames : Packet {
        public static byte Id => 0x02;

        public short NumGames => (short)SendGames.Count();
        public IEnumerable<Game> SendGames { get; } = new List<Game>();

        public ListGames() { }

        public ListGames(IEnumerable<byte> data) {
            Deconstruct(data);
        }

        public ListGames(IEnumerable<Game> games) {
            SendGames = games;
        }

        protected override Packet Deconstruct(IEnumerable<byte> data) {
            return null;
        }

        protected override byte[] Construct() {
            var data = new List<byte> { Id };

            data.AddRange(BitConverter.GetBytes(NumGames));
            SendGames.ToList().ForEach(game => { data.AddRange(game.GetInfo(InfoLevel.Basic)); });

            return data.ToArray();
        }
    }
}