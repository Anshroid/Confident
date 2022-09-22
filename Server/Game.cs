using System;
using System.Collections.Generic;
using System.Linq;
using log4net;
using Server.Packets.Downstream;

namespace Server {
    internal class Game {
        public static readonly List<Game> AllGames = default;

        private static readonly ILog Logger = LogManager.GetLogger(typeof(ConfidentClient));

        public readonly Dictionary<Guid, ConfidentClient> Players = default;
        private readonly Dictionary<Guid, short> _score = default;
        private readonly Dictionary<Guid, AnswerEntry> _answers = default;
        public Guid Id = Guid.NewGuid();
        public readonly bool Private;
        public bool Started;
        private short _roundNo;
        private string _question;

        public Game(bool pPrivate) {
            Logger.InfoFormat("New Game created with id {0}", Id);
            Private = pPrivate;
            AllGames.Add(this);
        }

        public void AddPlayer(ConfidentClient player) {
            player.CurrentGame = this;
            Players.Add(new Guid(player.ID), player);
        }

        public void Start() {
            Started = true;
            Logger.InfoFormat("Game {0} started", Id);
            _score.Keys.ToList().ForEach(player => _score[player] = 0);
            Players.Values.ToList().ForEach(player => player.Send(new GameStarted()));
            Round();
        }

        private void Round() {
            _roundNo++;
            Console.Write("Game {0} Round {1}: Enter question: ", Id, _roundNo);
            _question = Console.ReadLine();
            Logger.InfoFormat("Game {0} round {1} started, question is {2}", Id, _roundNo, _question);
            Players.Values.ToList().ForEach(player => player.Send(new Round(_roundNo, _question)));
        }

        public void AddAnswer(Guid answerer, short lower, short upper, short range) {
            _answers[answerer] = new AnswerEntry(lower, upper, range);
            if (_answers.Count != Players.Count) return;

            Players.Values.ToList().ForEach(player => player.Send(new AllAnswered()));

            getAnswer:
            Console.Write("Game {0} Round {1}: Enter answer (question was {2}): ", Id, _roundNo, _question);
            var answer = Console.ReadLine();
            Console.Write("Game {0} Round {1}: Enter note (question was {2}): ", Id, _roundNo, _question);
            var note = Console.ReadLine();
            if (string.IsNullOrEmpty(answer)) goto getAnswer;
            Mark(short.Parse(answer), note);
        }

        private void Mark(short answer, string note) {
            var correct = _answers.Where(player =>
                answer >= player.Value.Lower && answer <= player.Value.Upper).ToList();
            
            correct.Sort((answer1, answer2) => answer1.Value.Range - answer2.Value.Range);

            List<KeyValuePair<Guid, AnswerEntry>> incorrect = default;
            if (correct.Count() == Players.Count) {
                incorrect.Add(correct.Aggregate((currentMax, next) =>
                    next.Value.Range > currentMax.Value.Range ? next : currentMax));
            }
            else {
                incorrect = _answers.Except(correct).ToList();
            }
            
            var best = correct.Aggregate((currentMin, next) => 
                next.Value.Range < currentMin.Value.Range ? next : currentMin);

            var scores = new Dictionary<Guid, short> {
                [best.Key] = 3
            };
            
            correct.Except(new[] {best}).ToList().ForEach(player => scores[player.Key] = 1);
            // incorrect.ToList().ForEach(players.);
            
            // Players.Values.ToList().ForEach(player => player.Send(new EndRound)));
        }

        public IEnumerable<byte> GetInfo(InfoLevel level) {
            return new byte[] { };
        }
    }

    public enum InfoLevel {
        Basic,
        Detailed
    }

    public struct AnswerEntry {
        public readonly short Lower;
        public readonly short Upper;
        public readonly short Range;

        public AnswerEntry(short lower, short upper, short range) {
            Lower = lower;
            Upper = upper;
            Range = range;
        }
    }
}