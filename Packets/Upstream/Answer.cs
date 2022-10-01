using System;
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
    
    public struct AnswerEntry {
        public readonly short Lower;
        public readonly short Upper;
        public readonly short Range;
        public bool ThisRound;

        public AnswerEntry(short lower, short upper, short range) {
            Lower = lower;
            Upper = upper;
            Range = range;
            ThisRound = true;
        }

        public IEnumerable<byte> ToByteArray() {
            var data = new List<byte>();
            data.AddRange(BitConverter.GetBytes(Lower));
            data.AddRange(BitConverter.GetBytes(Upper));
            data.AddRange(BitConverter.GetBytes(Range));
            return data;
        }
    }
}