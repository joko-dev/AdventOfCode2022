using AdventOfCode2021.SharedKernel;

namespace Day01
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(PuzzleOutputFormatter.getPuzzleCaption("Day 01: Open"));
            Console.WriteLine("Transmission: ");
            PuzzleInput puzzleInput = new PuzzleInput(Console.ReadLine(), true);

        }
    }
}