using AdventOfCode2022.SharedKernel;
using System.Security;

namespace Day03
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(PuzzleOutputFormatter.getPuzzleCaption("Day 03: Rucksack Reorganization"));
            Console.WriteLine("Rucksack content: ");
            PuzzleInput puzzleInput = new(PuzzleOutputFormatter.getPuzzleFilePath(), false);

            List<char> priorityItemTypes = GetPrioritiyItemTyens(puzzleInput);
            
            Console.WriteLine("Score for guide: {0}", GetPrioritiesSum(priorityItemTypes));
        }

        private static List<char> GetPrioritiyItemTyens(PuzzleInput puzzleInput)
        { 
            List<char> priorityItemTypes = new List<char>();

            foreach (string line in puzzleInput.Lines)
            {
                int length = line.Length / 2;
                string firstPart = line.Substring(0, length);
                string secondPart = line.Substring(length);

                foreach (char type in firstPart)
                {
                    if (secondPart.Contains(type))
                    {
                        priorityItemTypes.Add(type);
                        break;
                    }
                }
            }

            return priorityItemTypes;
        }

        private static int GetPrioritiesSum(List<char> priorityItemTypes)
        {
            int sum = 0;

            foreach(char type in priorityItemTypes)
            {
                if (Char.IsAsciiLetterLower(type))
                {
                    sum += type - 96;
                }
                else if (Char.IsAsciiLetterUpper(type))
                {
                    sum += type - 38;
                }
                else
                {
                    throw new ArgumentOutOfRangeException(nameof(type));
                }
            }

            return sum;
        }
    }
}