using System;
using System.Collections.Generic;

namespace Server.Packets.Downstream {
    internal sealed class EndRound : Packet {
        public byte Id => 0x06;
        
        public short NumScores => (short) Scores.Count;
        public Dictionary<Guid, short> Scores;
        public short NoteLength => (short) Note.Length;
        public string Note;
        public AnswerEntry WinningAnswer;
        public bool Tie;

        public EndRound() { }

        public EndRound(IEnumerable<byte> data) {
            Deconstruct(data);
        }

        public EndRound(Dictionary<Guid, short> scores, string note, AnswerEntry winner, bool isTie) {
            Scores = scores;
            Note = note;
            WinningAnswer = winner;
            Tie = isTie;
        }

        protected override Packet Deconstruct(IEnumerable<byte> packet) {
            return null;
        }

        protected override byte[] Construct() {
            var data = new List<byte>();

            data.AddRange(BitConverter.GetBytes(NumScores));

            foreach (var score in Scores) {
                data.AddRange(score.Key.ToByteArray());
                data.AddRange(BitConverter.GetBytes(score.Value));
            }
            
            data.AddRange(BitConverter.GetBytes(NoteLength));
            data.AddRange(BitConverter.GetBytes(NoteLength));

            return data.ToArray();
        }
    }
}