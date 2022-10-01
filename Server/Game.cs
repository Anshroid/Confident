using System;
using System.Collections.Generic;
using System.Linq;
using log4net;
using Packets.Downstream;

namespace Server {
    internal class Game {
        public static readonly List<Game> AllGames = new List<Game>();

        private static readonly ILog Logger = LogManager.GetLogger(typeof(ConfidentClient));

        public readonly Dictionary<Guid, ConfidentClient> Players = new Dictionary<Guid, ConfidentClient>();
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
            Players.Values.ToList().ForEach(player => {
                player.Score = 0;
                player.Send(new GameStarted());
            });

            Round();
        }

        private void Round() {
            _roundNo++;
            Console.Write("Game {0} Round {1}: Enter question: ", Id, _roundNo);
            _question = Console.ReadLine();
            Logger.InfoFormat("Game {0} round {1} started, question is {2}", Id, _roundNo, _question);
            Players.Values.ToList().ForEach(player => player.Send(new Round(_roundNo, _question)));
        }

        public void AddAnswer(ConfidentClient answerer, short lower, short upper, short range) {
            answerer.Answer = new AnswerEntry(lower, upper, range);
            if (Players.Values.All(player => player.Answer.ThisRound)) return;

            Players.Values.ToList().ForEach(player => {
                player.Send(new AllAnswered());
                player.Answer.ThisRound = false;
            });

            getAnswer:
            Console.Write("Game {0} Round {1}: Enter answer (question was {2}): ", Id, _roundNo, _question);
            var answer = Console.ReadLine();
            Console.Write("Game {0} Round {1}: Enter note (question was {2}): ", Id, _roundNo, _question);
            var note = Console.ReadLine();
            if (string.IsNullOrEmpty(answer)) goto getAnswer;
            
            Mark(short.Parse(answer), note);
        }

        private void Mark(short answer, string note) {
            var correct = Players.Values.Where(player =>
                answer >= player.Answer.Lower && answer <= player.Answer.Upper).ToList();

            correct.Sort((answer1, answer2) => answer1.Answer.Range.CompareTo(answer2.Answer.Range));

            var incorrect = new List<ConfidentClient>();
            var tie = false;
            if (correct.Count() == Players.Count) {
                tie = true;
                incorrect.Add(correct.Last());
            }
            else {
                incorrect.AddRange(Players.Values.Except(correct));
            }

            var best = correct.First();

            var scores = new Dictionary<Guid, short>();

            // Stack of overwrites (fun!)
            // Since the best is also correct, but an incorrect could also be correct
            correct.ForEach(player => scores[player.ClientId] = 1);
            scores[best.ClientId] = 3;
            incorrect.ForEach(player => scores[player.ClientId] = 0);

            foreach (var score in scores) {
                Players[score.Key].Score += score.Value;
            }

            Players.Values.ToList().ForEach(player => player.Send(new EndRound(scores, note, best.Answer, tie)));

            CheckWins();
        }

        private void CheckWins() {
            var winners = Players.Values.Where(player => player.Score >= 12).ToList();
            winners.Sort((player1, player2) => player1.Score.CompareTo(player2.Score));
            switch (winners.Count) {
                case 0:
                    Round();
                    return;
                case 1:
                    Players.Values.ToList().ForEach(player => player.Send(new GameWon(winners.First().ClientId)));
                    return;
                default:
                    if (winners[0].Score == winners[1].Score) {
                        Players.Values.ToList().ForEach(player =>
                            player.Send(new Overtime(winners.Select(winner => winner.ClientId))));
                        Round();
                    }
                    else {
                        Players.Values.ToList().ForEach(player => player.Send(new GameWon(winners.First().ClientId)));
                    }

                    return;
            }
        }

        public IEnumerable<byte> GetInfo(InfoLevel level) {
            var data = new List<byte>();
            data.AddRange(Id.ToByteArray());
            return data;
        }
    }

    public enum InfoLevel {
        Basic,
        Detailed
    }
}