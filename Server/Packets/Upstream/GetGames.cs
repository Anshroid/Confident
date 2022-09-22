using System.Collections.Generic;

namespace Server.Packets.Upstream {
    internal sealed class GetGames : Packet {
        public static byte Id => 0x01;

        public GetGames() { }

        public GetGames(IEnumerable<byte> data) {
            Deconstruct(data);
        }
    }
}