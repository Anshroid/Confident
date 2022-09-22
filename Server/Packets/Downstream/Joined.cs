using System;
using System.Collections.Generic;

namespace Server.Packets.Downstream {
    internal sealed class Joined : Packet {
        public byte Id => 0x01;

        public Guid PlayerId { get; set; }

        public Joined() { }

        public Joined(Guid playerId) {
            PlayerId = playerId;
        }

        public Joined(IEnumerable<byte> data) {
            Deconstruct(data);
        }
    }
}