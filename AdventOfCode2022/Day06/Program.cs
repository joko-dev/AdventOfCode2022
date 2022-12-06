using AdventOfCode2022.SharedKernel;

namespace Day06
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(PuzzleOutputFormatter.getPuzzleCaption("Day 06: Tuning Trouble"));
            Console.WriteLine("Datastream: ");
            PuzzleInput puzzleInput = new(PuzzleOutputFormatter.getPuzzleFilePath(), false);

            Console.WriteLine("Charactercount before start-of-packet marker: {0}", GetCharacterCount(puzzleInput.Lines[0], 4));
        }

        private static int GetCharacterCount(string datastream, int markerLength)
        {
            for(int i = markerLength; i < datastream.Length; i++)
            {

                if (!datastream.Substring(i - markerLength, markerLength).GroupBy(c => c).Any(g => g.Count() > 1))               
                {
                    return i;
                }
            }
            throw new ArgumentException(nameof(datastream));
        }
    }
}