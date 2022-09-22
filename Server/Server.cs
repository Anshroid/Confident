using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using log4net;
using log4net.Config;
using Server.Packets;
using WebSocketSharp.Server;

namespace Server {
    internal class Server {
        public static readonly Dictionary<string, ConfidentClient> ClientList =
            new Dictionary<string, ConfidentClient>();

        private static readonly ILog Log = LogManager.GetLogger(typeof(Server));

        public static void Main(string[] args) {
            var server = new WebSocketServer(IPAddress.Parse("127.0.0.1"), 8023);

            Packet.RegisterAll();

            XmlConfigurator.Configure(new System.IO.FileInfo("../../log4net.config.xml"));

            server.AddWebSocketService<ConfidentClient>("/");
            server.KeepClean = false;
            Log.Info("Starting server...");

            try {
                server.Start();
            }
            catch (SocketException) {
                Log.Error("Another instance of the server was already running. Exiting...");
                return;
            }

            Console.ReadKey(true);
            Log.Info("Stopping server...");
        }
    }
}