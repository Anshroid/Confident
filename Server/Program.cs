using System;
using System.Collections.Generic;
using System.Net;
using Server.Packets;
using WebSocketSharp.Server;

namespace Server
{
    internal class Program
    {
        public static readonly Dictionary<string, ConfidentClient> ClientList = new Dictionary<string, ConfidentClient>();
        public static void Main(string[] args)
        {
            var server = new WebSocketServer(IPAddress.Parse("127.0.0.1"), 8023);
            
            Packet.RegisterAll();

            server.AddWebSocketService<ConfidentClient>("/");
            server.KeepClean = false;
            server.Start();
            Console.ReadKey(true);
        }
    }
}