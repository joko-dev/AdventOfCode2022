using AdventOfCode2022.SharedKernel;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Day16
{
    internal class Program
    {
        internal class Valve{
            internal int FlowRate { get; }
            internal String Name { get; }
            internal bool Opened { get; set; }
            internal List<String> Neighbours { get; }
            internal Valve (String name, int flowRate)
            {
                FlowRate = flowRate;
                Name = name;
                Opened = false;
                Neighbours= new List<String> ();
            }
        }

        static void Main(string[] args)
        {
            Console.WriteLine(PuzzleOutputFormatter.getPuzzleCaption("Day 16: Proboscidea Volcanium"));
            Console.WriteLine("Flow rate report: ");
            PuzzleInput puzzleInput = new(PuzzleOutputFormatter.getPuzzleFilePath(), true);


            // Floyd Warshall Algorithm - idea from reddit
            // Solution was mostly taken from: Source: https://github.com/Bpendragon/AdventOfCodeCSharp/blob/9fd66/AdventOfCode/Solutions/Year2022/Day16-Solution.cs
            // had no really idea how to approach the task, after implemententing the given solution it was quite obvious
            List<Valve> valves = CreateValves(puzzleInput.Lines);
            int[,] distances = FloydWarshall(valves);
            int[,] importantDists; 

            List<Valve>  importantValves = valves.Where(v => v.Name == "AA" || v.FlowRate != 0).ToList();
            List<int> indices = new();

            for (int i = 0; i < valves.Count; i++) if (valves[i].FlowRate == 0 && valves[i].Name != "AA") indices.Add(i);
            indices.Reverse();

            importantDists = distances;
            foreach (var i in indices)
            {
                importantDists = importantDists.TrimArray(i, i);
            }

            Dictionary<int, int> cache = new();
            VisitValve(importantValves, importantDists, 0, 30, 0, 0, cache);
            Console.WriteLine("Most preasure to release: {0}", cache.Values.Max()) ;

            cache = new();
            VisitValve(importantValves, importantDists, 0, 26, 0, 0, cache);
            int curMax = 0;
            foreach (var kvp1 in cache)
            {
                foreach (var kvp2 in cache)
                {
                    if ((kvp1.Key & kvp2.Key) != 0) continue; //Only care if valves for disjoint set, so human and elephant take completly different routes
                    curMax = int.Max(curMax, kvp1.Value + kvp2.Value);
                }
            }
            Console.WriteLine("Most preasure to release (help of elephant): {0}", curMax);
        }

        private static void VisitValve(List<Valve> valves, int[,] distances, int node, int time, int state, int flow, Dictionary<int, int> cache)
        {
            cache[state] = int.Max(cache.GetValueOrDefault(state, 0), flow); //Are we at a better point with the current valves turned on than last time we were at this point? if so, update value
            for (int i = 0; i < valves.Count; i++) //For all valves
            {
                var newTime = time - distances[node, i] - 1; //time remaining is time minus walking time, minus 1 minute to open valve
                if (((1 << i) & state) != 0 || newTime <= 0) continue; //Don't go to the same valve twice, don't go to a valve if it means we run out of time.
                VisitValve(valves, distances, i, newTime, state | (1 << i), flow + (newTime * valves[i].FlowRate), cache);
                //Go to new valve, update state so it's turned on, add it's flow, repeat everything above.
            }
        }

        private static int[,] FloydWarshall(List<Valve> valves)
        {
            int[,] distances = new int[valves.Count, valves.Count];
            for(int x = 0; x < valves.Count; x++)
            {
                for (int y = 0; y < valves.Count; y++)
                {
                    if (x == y)
                    {
                        distances[x, y] = 0;
                    }
                    else if (valves[x].Neighbours.Contains(valves[y].Name))
                    {
                        distances[x, y] = 1;
                    }
                    else
                    {
                        distances[x, y] = 1000;
                    }
                    
                }
            }

            for (int k = 0; k < valves.Count; k++)
            {
                for (int x = 0; x < valves.Count; x++)
                {
                    for (int y = 0; y < valves.Count; y++)
                    {
                        if (distances[x, k] + distances[k, y] < distances[x, y])
                        {
                            distances[x, y] = distances[x, k] + distances[k, y];
                        }
                    }
                }
            }

            return distances;
        }

        private static List<Valve> CreateValves(List<string> lines)
        {
            List<Valve> valves = new List<Valve>();
            foreach (string line in lines)
            { 
                string[] valveTunnel = line.Split(';');
                string[] valve = valveTunnel[0].Replace("Valve ", "").Replace("has flow rate=", "").Split(" ");
                string[] tunnel = valveTunnel[1].Replace("tunnels", "tunnel").Replace("leads", "lead").Replace("valves", "valve").
                                                    Replace("tunnel lead to valve ", "").Split(",").Select(s => s.Trim()).ToArray();

                Valve newValve = new Valve(valve[0], Int32.Parse(valve[1]));
                newValve.Neighbours.AddRange(tunnel);
                valves.Add(newValve);
            }
            valves.OrderBy(v => v.Name);
            return valves;
        }
    }
}