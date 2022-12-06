using AdventOfCode2022.SharedKernel;

namespace Day07
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(PuzzleOutputFormatter.getPuzzleCaption("Day 07: "));
            Console.WriteLine(": ");
            PuzzleInput puzzleInput = new(PuzzleOutputFormatter.getPuzzleFilePath(), false); 
        }
    }
}