using AdventOfCode2022.SharedKernel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks.Dataflow;

namespace Day13
{
    internal class PacketData : IComparable
    {
        internal List<PacketData> Packets { get; }
        internal int Integer { get; }
        internal PacketData(int integer)
        {
            this.Integer = integer;
            this.Packets = null;
        }
        internal PacketData()
        {
            this.Packets = new List<PacketData>();
            this.Integer = -1;
        }

        public override string ToString()
        {
            StringBuilder stringBuilder = new StringBuilder();

            if (IsIntegerPacket())
            {
                stringBuilder.Append(Integer.ToString());
            }
            else
            {
                stringBuilder.Append("[");
                for(int i = 0; i < Packets.Count; i++)
                {
                    if(i > 0)
                    {
                        stringBuilder.Append(",");
                    }
                    stringBuilder.Append(Packets[i].ToString());
                }
                stringBuilder.Append("]");
            }

            return stringBuilder.ToString();
        }

        public int CompareTo(object? obj)
        {
            if (obj == null)
            {
                return 1;
            }

            PacketData otherPacket = obj as PacketData;
            if (otherPacket != null)
            {
                if(this.IsIntegerPacket() && otherPacket.IsIntegerPacket())
                {
                    if(this.Integer < otherPacket.Integer)
                    {
                        return -1;
                    }
                    else if (this.Integer > otherPacket.Integer)
                    {
                        return 1;
                    }
                    else
                    {
                        return 0;
                    }
                }
                else if(this.IsIntegerPacket() && ! otherPacket.IsIntegerPacket())
                {
                    PacketData thisAsList = IntegerAsList();
                    return thisAsList.CompareTo(otherPacket);
                }
                else if (!this.IsIntegerPacket() && otherPacket.IsIntegerPacket())
                {
                    PacketData otherAsList = otherPacket.IntegerAsList();
                    return this.CompareTo(otherAsList);
                }
                else
                {
                    for(int i = 0; i < this.Packets.Count; i++)
                    {
                        if(i < otherPacket.Packets.Count)
                        {
                            int listCompare = this.Packets[i].CompareTo(otherPacket.Packets[i]);
                            if(listCompare != 0)
                            {
                                return listCompare;
                            }
                        }
                    }
                    
                    if (this.Packets.Count < otherPacket.Packets.Count)
                    {
                        return -1;
                    }
                    else if (this.Packets.Count > otherPacket.Packets.Count)
                    {
                        return 1;
                    }
                    else if (this.Packets.Count == otherPacket.Packets.Count)
                    {
                        return 0;
                    }
                    throw new Exception("Invalid Comparison");
                }
            }

            throw new ArgumentException(nameof(obj));    
        }

        private PacketData IntegerAsList()
        {
            if (IsIntegerPacket())
            {
                PacketData listPacket = new PacketData();
                listPacket.Packets.Add(new PacketData(Integer));
                return listPacket;
            }
            throw new ArgumentException();
        }

        private bool IsIntegerPacket()
        {
            return (this.Integer >= 0);
        }
    }


    internal class Program   
    {
        static void Main(string[] args)
        {
            Console.WriteLine(PuzzleOutputFormatter.getPuzzleCaption("Day 13: Distress Signal"));
            Console.WriteLine("Packet Pairs: ");
            PuzzleInput puzzleInput = new(PuzzleOutputFormatter.getPuzzleFilePath(), true);

            List<PacketData> packetPairs = GetListPackets(puzzleInput.Lines);
            Console.WriteLine("Sum of pair-indizes in right order: {0}", GetSumOfPairsInRightOrder(packetPairs));

            PacketData divider1 = new PacketData();
            divider1.Packets.Add(new PacketData(2));
            packetPairs.Add(divider1);
            PacketData divider2 = new PacketData();
            divider2.Packets.Add(new PacketData(6));
            packetPairs.Add(divider2);

            Console.WriteLine("Decoder key: {0}", GetDecoderKey(packetPairs, divider1, divider2));

        }

        private static List<PacketData> GetListPackets(List<string> lines)
        {
            List<PacketData> packets = new List<PacketData>();
            foreach (string line in lines)
            {
                packets.Add(CreateNewPacket(line));
            }
            return packets;
        }

        private static PacketData CreateNewPacket(string data)
        {
            int integer;
            // Valid Number --> Integer packet
            if(Int32.TryParse(data, out integer))
            {
                return new PacketData(integer);
            }

            // Empty List
            if (data.StartsWith('[') && data.EndsWith(']') && data.Length == 2)
            {
                return new PacketData();
            }

            // Normal List
            if (data.StartsWith('[') && data.EndsWith(']'))
            {
                List<String> subStrings = new List<string>();
                string currentString = "";
                int openBrackets = 0;

                for(int i = 1; i < data.Length - 1; i++)
                {
                    if (data[i] == '[')
                    {
                        openBrackets++;
                    }

                    if (data[i] == ']')
                    {
                        openBrackets--;
                    }

                    if ((openBrackets == 0 && data[i] == ','))
                    {
                        subStrings.Add(currentString);
                        currentString = "";
                    }
                    else
                    {
                        currentString += data[i];
                        if(i == data.Length - 2)
                        {
                            subStrings.Add(currentString);
                        }
                    }
                }

                PacketData packet = new PacketData();
                foreach(string subString in subStrings)
                {
                    packet.Packets.Add(CreateNewPacket(subString));
                }

                return packet;
            }

            throw new ArgumentOutOfRangeException(nameof(data));
        }

        private static int GetSumOfPairsInRightOrder(List<PacketData> packetPairs)
        {
            int result = 0;

            for(int i = 0; i < packetPairs.Count; i += 2)
            {
                if (packetPairs[i].CompareTo(packetPairs[i + 1]) < 1)
                {
                    Console.WriteLine("Pair {0} in right order", (i+2)/2);
                    result += (i + 2) / 2;
                }
                else
                {
                    Console.WriteLine("Pair {0} NOT in right order", (i + 2) / 2);
                }
            }

            return result;
        }

        private static int GetDecoderKey(List<PacketData> packetPairs, PacketData divider1, PacketData divider2)
        {
            int decoderKey;
            packetPairs.Sort();

            foreach(PacketData packet in packetPairs)
            {
                Console.WriteLine(packet);
            }

            decoderKey = (packetPairs.FindIndex(p => p == divider1) + 1) * (packetPairs.FindIndex(p => p == divider2) + 1);

            return decoderKey;
        }
    }
}