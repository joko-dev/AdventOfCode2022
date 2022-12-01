using AdventOfCode2022.SharedKernel;

namespace AdventOfCode2022.Day01
{
    internal class Program
    {
        static void Main(string[] args)
        {
            
            Console.WriteLine(PuzzleOutputFormatter.getPuzzleCaption("Day 01: Calorie Counting"));
            Console.WriteLine("Calories: ");
            PuzzleInput puzzleInput = new PuzzleInput(PuzzleOutputFormatter.getPuzzleFilePath(), false);

            List<int> elvesCalories = getElvesCalories(puzzleInput);

            Console.WriteLine("Max calories: {0}", elvesCalories.Max());
            Console.WriteLine("Sum max 3 calories: {0}", elvesCalories.OrderDescending().Take(3).Sum());
        }

        static List<int> getElvesCalories(PuzzleInput puzzleInput)
        {
            List<int> elvesCalories = new List<int>();

            int current = 0;
            elvesCalories.Add(0);
            foreach (string line in puzzleInput.Lines)
            {
                if (line == "")
                {
                    elvesCalories.Add(0);
                    current++;
                }
                else
                {
                    elvesCalories[current] += Int32.Parse(line);
                }

            }

            return elvesCalories;
        }
    }
}