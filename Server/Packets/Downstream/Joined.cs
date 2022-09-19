using System.Collections.Generic;

namespace Server.Packets.Downstream
{
    internal sealed class Joined : Packet
    {
        public byte Id => 0x01;
        
        public int PlayerIdLength => PlayerId.Length;
        public string PlayerId { get; set; }
        
        public Joined() {}
        
        public Joined(string playerId)
        {
            PlayerId = playerId;
        }

        public Joined(IEnumerable<byte> data)
        {
            Deconstruct(data);
        }
    }
}