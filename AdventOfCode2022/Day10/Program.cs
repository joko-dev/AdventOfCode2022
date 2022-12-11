using AdventOfCode2022.SharedKernel;
using System.Net.Mail;

namespace Day10
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(PuzzleOutputFormatter.getPuzzleCaption("Day 10: Cathode-Ray Tube"));
            Console.WriteLine("Program: ");
            PuzzleInput puzzleInput = new(PuzzleOutputFormatter.getPuzzleFilePath(), false);

            List<int> cycles = new List<int>(new int[]{ 20, 60, 100, 140, 180, 220 });
            

            Console.WriteLine("Sum of signal strengths (20th, 60th, 100th, 140th, 180th, 220th): {0}", GetSumSignalStrenghts(puzzleInput, cycles));
        }

        private static int GetSumSignalStrenghts(PuzzleInput puzzleInput, List<int> cycles)
        {
            int sum = 0;
            int currentCycle = 0;
            int X = 1;
            List<int> cyclesForCalculation = new List<int>(cycles);

            foreach(string line in puzzleInput.Lines)
            {
                int startCycle = currentCycle;
                int startX = X;

                if (line == "noop")
                {
                    currentCycle += 1; 
                }
                else if (line.StartsWith("addx"))
                {
                    X += Int32.Parse(line.Split(' ')[1]);
                    currentCycle += 2;
                }
                else
                {
                    throw new ArgumentOutOfRangeException(nameof(line));
                }

                if (cyclesForCalculation.Count > 0)
                {
                    if(startCycle < cyclesForCalculation[0] && currentCycle >= cyclesForCalculation[0])
                    {
                        sum += cyclesForCalculation[0] * startX;
                        cyclesForCalculation.RemoveAt(0);
                    }

                }

            }
            return sum;
        }
    }
}