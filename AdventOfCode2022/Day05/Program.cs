using AdventOfCode2022.SharedKernel;

namespace Day05
{


    internal class Program
    {
        internal struct Rearrangement
        {
            public int From { get; }
            public int To { get; }
            public int Count { get; }

            public Rearrangement(int from, int to, int count)
            {
                From = from;
                To = to;
                Count = count;
            }
        }
       
        static void Main(string[] args)
        {
            Console.WriteLine(PuzzleOutputFormatter.getPuzzleCaption("Day 05: Supply Stacks"));
            Console.WriteLine("Stacks and rearrangement procedure: ");
            PuzzleInput puzzleInput = new(PuzzleOutputFormatter.getPuzzleFilePath(), false);

            List<List<char>> stacks = GetStacks(puzzleInput);
            List<Rearrangement> rearrangements = GetRearrangements(puzzleInput);

            RearrangeStacks(stacks, rearrangements);

            Console.WriteLine("Crates on top: {0}", GetTopCrates(stacks));
        }

        private static string GetTopCrates(List<List<char>> stacks)
        {
            string topCrates = "";
            foreach(List<char> stack in stacks)
            {
                topCrates += stack[stack.Count - 1];
            }

            return topCrates;
        }

        private static void RearrangeStacks(List<List<char>> stacks, List<Rearrangement> rearrangements)
        {
            foreach(Rearrangement rearrangement in rearrangements)
            {
                for (int i = 1; i <= rearrangement.Count; i++)
                {
                    int top = stacks[rearrangement.From - 1].Count - 1;
                    char crate = stacks[rearrangement.From - 1][top];
                    stacks[rearrangement.From - 1].RemoveAt(top);
                    stacks[rearrangement.To - 1].Add(crate);
                    
                }
            }
        }

        private static List<List<char>> GetStacks(PuzzleInput puzzleInput)
        {
            int countStacks = (int)(puzzleInput.Lines[0].Length + 1) / 4;
            List<List<char>> stacks = new();
            for(int i = 0; i < countStacks; i++)
            {
                stacks.Add(new List<char>());
            }

            foreach(string line in puzzleInput.Lines)
            {
                // stack definition is over --> break loop
                if (!line.Contains('['))
                {
                    break;
                }

                for(int i = 0; i<countStacks; i++)
                {
                    string crate = line + ' ';
                    crate = crate.Substring(i * 4, 4);
                    crate = crate.Trim().Replace("[", "").Replace("]", "");
                    if(crate != "")
                    {
                        stacks[i].Insert(0, crate[0]);
                    }
                }

            }

            return stacks;
        }

        private static List<Rearrangement> GetRearrangements(PuzzleInput puzzleInput)
        {
            List<Rearrangement> rearrangements = new();

            foreach (string line in puzzleInput.Lines)
            {
                if (line.StartsWith("move"))
                {
                    string[] elements = line.Split(' ');
                    rearrangements.Add(new Rearrangement(Int32.Parse(elements[3]), Int32.Parse(elements[5]), Int32.Parse(elements[1])));
                }
            }

            return rearrangements;
        }
    }
}