using System.Collections.Generic;

namespace Packets.Downstream {
    public sealed class Round : Packet {
        public byte Id => 0x04;

        public short RoundNumber { get; set; }
        public short QuestionLength => (short) Question.Length;
        public string Question { get; set; }

        public Round() { }
        
        public Round(IEnumerable<byte> data) {
            Deconstruct(data);
        }

        public Round(short roundNo, string question) {
            RoundNumber = roundNo;
            Question = question;
        }
    }
}