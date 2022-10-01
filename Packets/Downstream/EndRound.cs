using System;
using System.Collections.Generic;
using System.Text;

namespace Packets.Downstream {
    public sealed class EndRound : Packet {
        public byte Id => 0x06;
        
        public short NumScores => (short) Scores.Count;
        public Dictionary<Guid, short> Scores { get; set; }
        public short NoteLength => (short) Note.Length;
        public string Note { get; set; }
        public AnswerEntry WinningAnswer { get; set; }
        public bool Tie { get; set; }

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
            
            data.Add(Id);

            data.AddRange(BitConverter.GetBytes(NumScores));

            foreach (var score in Scores) {
                data.AddRange(score.Key.ToByteArray());
                data.AddRange(BitConverter.GetBytes(score.Value));
            }
            
            data.AddRange(BitConverter.GetBytes(NoteLength));
            data.AddRange(Encoding.Default.GetBytes(Note));
            
            data.AddRange(WinningAnswer.ToByteArray());

            return data.ToArray();
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