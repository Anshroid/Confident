using System.Collections.Generic;

namespace Server.Packets.Upstream {
    internal sealed class Answer : Packet {
        public byte Id => 0x06;

        public short Lower;
        public short Upper;
        public short Range => (short) (Upper - Lower);

        public Answer() { }

        public Answer(IEnumerable<byte> data) {
            Deconstruct(data);
        }

        public Answer(short lower, short upper) {
            Lower = lower;
            Upper = upper;
        }
    }
}