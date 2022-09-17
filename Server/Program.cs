using System;
using System.Net;
using System.Net.Sockets;

namespace Server
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            var server = new TcpListener(IPAddress.Parse("127.0.0.1"), 8023);
            
            server.Start();
            Console.WriteLine("Server has started on 127.0.0.1:8023{0}Waiting for a connection...", Environment.NewLine);

            var client = server.AcceptTcpClient();
            
            Console.WriteLine("A client connected.");
        }
    }
}