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
                int startX = X;
                int cyclesToAdd = 0;

                if (line == "noop")
                {
                    cyclesToAdd = 1; 
                }
                else if (line.StartsWith("addx"))
                {
                    X += Int32.Parse(line.Split(' ')[1]);
                    cyclesToAdd = 2;
                }
                else
                {
                    throw new ArgumentOutOfRangeException(nameof(line));
                }

                for(int c = 1; c <= cyclesToAdd; c++)
                {

                    currentCycle++;
                    int currentPixel = (currentCycle - 1) % 40;
                    

                    if (startX - 1 == currentPixel || startX == currentPixel || startX + 1 == currentPixel)
                    {
                        Console.Write("#");
                    }
                    else
                    {
                        Console.Write(".");
                    }
                        

                    if (currentCycle > 0 && currentCycle % 40 == 0)
                    {
                        Console.Write("\n");
                    }

                    if (cyclesForCalculation.Count > 0 && currentCycle == cyclesForCalculation[0])
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