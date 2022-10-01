using System.Collections.Generic;

namespace Packets.Upstream {
    public sealed class Answer : Packet {
        public byte Id => 0x06;

        public short Lower { get; set; }
        public short Upper { get; set; }
        public short Range {
            get { return (short)(Upper - Lower); }
            set => _ = value;
        }

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