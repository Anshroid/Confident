using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Packets {
    public abstract class Packet {
        private static readonly Dictionary<Direction, Dictionary<byte, Type>> AllPackets =
            new Dictionary<Direction, Dictionary<byte, Type>> {
                { Direction.Upstream, new Dictionary<byte, Type>() },
                { Direction.Downstream, new Dictionary<byte, Type>() }
            };

        protected virtual Packet Deconstruct(IEnumerable<byte> packet) {
            var index = 0;

            GetType().GetProperties()
                .Where(info => info.Name != "Id")
                .ToList()
                .ForEach((field) => {
                    switch (field.GetValue(this)) {
                        case bool _:
                            field.SetValue(this, packet.ElementAt(index) == 1);
                            index += 1;
                            break;
                        case short _:
                            field.SetValue(this, BitConverter.ToInt16(packet.Skip(index).Take(2).ToArray(), 0));
                            index += 2;
                            break;
                        case string _:
                            var length = (short)GetType().GetProperties()
                                .Where(info => info.Name != "Id")
                                .TakeWhile(info => info != field)
                                .Last().GetValue(this);
                            field.SetValue(this, Encoding.Default.GetString(packet.Skip(index).Take(length).ToArray()));
                            break;
                        case Guid _:
                            field.SetValue(this, new Guid(packet.Skip(index).Take(16).ToArray()));
                            index += 16;
                            break;
                    }
                });
            return this;
        }

        protected virtual byte[] Construct() {
            var packet = new List<byte> { (byte) GetType().GetProperty("Id")!.GetValue(null) };

            GetType().GetProperties()
                .Where(info => info.Name != "Id")
                .ToList()
                .ForEach((field) => {
                    switch (field.GetValue(this)) {
                        case bool _:
                            packet.Add((bool)field.GetValue(this) ? (byte)1 : (byte)0);
                            break;
                        case short _:
                            packet.AddRange(BitConverter.GetBytes((short)field.GetValue(this)));
                            break;
                        case string _:
                            packet.AddRange(Encoding.Default.GetBytes((string)field.GetValue(this)));
                            break;
                        case Guid _:
                            packet.AddRange(((Guid)field.GetValue(this)).ToByteArray());
                            break;
                    }
                });

            return packet.ToArray();
        }

        public static Packet Get(Direction direction, IEnumerable<byte> packet) {
            packet = packet.ToList();
            return ((Packet)Activator.CreateInstance(AllPackets[direction][packet.First()]))
                .Deconstruct(packet.Skip(1));
        }

        public static void RegisterAll() {
            var packets = typeof(Packet).Assembly;
            packets.GetTypes()
                .Where(t => string.Equals(t.Namespace, "Packets.Upstream"))
                .Where(t => t.BaseType == typeof(Packet))
                .ToList()
                .ForEach(packet => {
                    AllPackets[Direction.Upstream].Add((byte)packet.GetProperty("Id")!.GetValue(null), packet);
                });

            packets.GetTypes()
                .Where(t => string.Equals(t.Namespace, "Packets.Downstream"))
                .Where(t => t.BaseType == typeof(Packet))
                .ToList()
                .ForEach(packet => {
                    AllPackets[Direction.Downstream].Add((byte)packet.GetProperty("Id")!.GetValue(null), packet);
                });
        }

        public static implicit operator byte[](Packet p) => p.Construct();
    }

    public enum Direction {
        Upstream,
        Downstream
    }
}