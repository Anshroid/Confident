using System.Collections.Generic;
using Server.Packets;
using Server.Packets.Downstream;
using Server.Packets.Upstream;
using WebSocketSharp;
using WebSocketSharp.Server;

namespace Server
{
    internal class ConfidentClient : WebSocketBehavior
    {
        public static readonly Dictionary<string, ConfidentClient> ClientList = new Dictionary<string, ConfidentClient>();

        protected override void OnMessage(MessageEventArgs e)
        {
            dynamic pack = Packet.Get(Direction.Upstream, e.RawData);
            switch (pack)
            {
                case Join packet:
                    if (ClientList.ContainsKey(ID)) { Send(new Response(false)); return; }
                    ClientList.Add(packet.Name, this);
                    Send(ID);
                    break;
            }
        }

        protected override void OnClose(CloseEventArgs e)
        {
            ClientList.Remove(ID);
        }
    }
}