using System;
using System.Collections.Generic;
using System.Linq;
using log4net;
using Server.Packets;
using Server.Packets.Downstream;
using Server.Packets.Upstream;
using WebSocketSharp;
using WebSocketSharp.Server;
// ReSharper disable UnusedVariable

namespace Server {
    internal class ConfidentClient : WebSocketBehavior {
        private static readonly Dictionary<Guid, ConfidentClient> ClientList = new Dictionary<Guid, ConfidentClient>();

        private static readonly ILog Logger = LogManager.GetLogger(typeof(ConfidentClient));

        public Guid ClientId;
        private string _name = "";

        private Game _currentGame;
        public short Score;
        public AnswerEntry Answer;

        public Game CurrentGame {
            get => _currentGame;
            set {
                value.Players.Add(ClientId, this);
                _currentGame = value;
            }
        }

        protected override void OnOpen() {
            Logger.InfoFormat("Connection opened with client ID {0}", ID);
            ClientId = new Guid(ID);
        }

        protected override void OnMessage(MessageEventArgs e) {
            Logger.InfoFormat("Recieving packet {0} from {1} [{2}]", BitConverter.ToString(e.RawData), ID, _name);

            dynamic pack = Packet.Get(Direction.Upstream, e.RawData);
            switch (pack) {
                case Suspend packet: break;
                case Join packet:
                    if (ClientList.ContainsKey(ClientId)) {
                        Send(new Response(false));
                        return;
                    }

                    Logger.InfoFormat("Client joined: {0} with id {1}", packet.Name, ID);
                    _name = packet.Name;
                    ClientList.Add(ClientId, this);
                    Send(new Joined(ClientId));
                    break;
                case GetGames packet:
                    Send(new ListGames(Game.AllGames.Where(game => game.Private != true && game.Started != true)));
                    break;
                case JoinGame packet:
                    CurrentGame = Game.AllGames.Where(game => game.Id.Equals(packet.GameId)).ToList()[0];
                    Send(new Response(true));
                    break;
                case JoinPrivateGame packet:
                    break;
                case CreateGame packet:
                    CurrentGame = new Game(packet.Private);
                    Send(new Response(true));
                    break;
                case StartGame packet:
                    CurrentGame.Start();
                    Send(new Response(true));
                    break;
                case Answer packet:
                    CurrentGame.AddAnswer(this, packet.Lower, packet.Upper, packet.Range);
                    break;
            }
        }

        protected override void OnClose(CloseEventArgs e) {
            Logger.InfoFormat("Client {0} [{1}] disconnected: {2}", ID, _name, e.Reason);
            ClientList.Remove(ClientId);
            CurrentGame.Players.Remove(ClientId);
        }

        public void Send(Packet data) {
            Send((byte[])data);
        }
    }
}