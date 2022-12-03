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

            List<char> priorityItemTypes = GetPrioritiyItemTypes(puzzleInput);
            Console.WriteLine("Sum of priority items: {0}", GetPrioritiesSum(priorityItemTypes));

            priorityItemTypes = GetCommonItemTypesForGroup(puzzleInput);
            Console.WriteLine("Sum of priority items in group: {0}", GetPrioritiesSum(priorityItemTypes));
        }

        private static List<char> GetPrioritiyItemTypes(PuzzleInput puzzleInput)
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

        private static List<char> GetCommonItemTypesForGroup(PuzzleInput puzzleInput)
        {
            List<char> priorityItemTypes = new List<char>();

            for(int i = 0; i < puzzleInput.Lines.Count; i+=3)
            {
                foreach(char type in puzzleInput.Lines[i])
                {
                    if(puzzleInput.Lines[i+1].Contains(type) && puzzleInput.Lines[i + 2].Contains(type))
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