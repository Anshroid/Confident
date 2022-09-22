using System.Collections.Generic;

namespace Server.Packets.Downstream {
    internal sealed class Round : Packet {
        public byte Id => 0x04;

        public short RoundNumber;
        public short QuestionLength => (short) Question.Length;
        public string Question;

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