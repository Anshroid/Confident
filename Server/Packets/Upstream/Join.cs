using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Server.Packets.Upstream
{
    internal sealed class Join : Packet
    {
        public static byte Id => 0x00;
        
        public short NameLength { get => (short) Name.Length; set => _ = value; }
        public string Name { get; set; } = "";
        
        public Join() {}
        
        public Join(IEnumerable<byte> data)
        {
            Deconstruct(data);
        }
    }
}