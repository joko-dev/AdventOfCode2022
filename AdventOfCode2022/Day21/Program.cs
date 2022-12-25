using AdventOfCode2022.SharedKernel;

namespace Day21
{
    internal class Program
    {
        class Monkey
        {
            public string Name { get; }
            public string Operation { get; }

            public Monkey(string line)
            {
                string[] temp = line.Split(":");
                Name = temp[0];
                Operation = temp[1];
            }
            public Int64 Calculate(List<Monkey> monkeys)
            {
                Int64 result = 0;
                if(!Int64.TryParse(Operation, out result))
                {
                    string monkeyLeft;
                    string monkeyRight;
                    if(CheckOperation(Operation, '+', out monkeyLeft, out monkeyRight))
                    {
                        result = monkeys.Find(m => m.Name == monkeyLeft).Calculate(monkeys) + monkeys.Find(m => m.Name == monkeyRight).Calculate(monkeys);
                    }
                    else if (CheckOperation(Operation, '-', out monkeyLeft, out monkeyRight))
                    {
                        result = monkeys.Find(m => m.Name == monkeyLeft).Calculate(monkeys) - monkeys.Find(m => m.Name == monkeyRight).Calculate(monkeys);
                    }
                    else if (CheckOperation(Operation, '*', out monkeyLeft, out monkeyRight))
                    {
                        result = monkeys.Find(m => m.Name == monkeyLeft).Calculate(monkeys) * monkeys.Find(m => m.Name == monkeyRight).Calculate(monkeys);
                    }
                    else if (CheckOperation(Operation, '/', out monkeyLeft, out monkeyRight))
                    {
                        result = monkeys.Find(m => m.Name == monkeyLeft).Calculate(monkeys) / monkeys.Find(m => m.Name == monkeyRight).Calculate(monkeys);
                    }
                }
                return result;
            }
            private bool CheckOperation(string operation, char op, out string monkeyLeft, out string monkeyRight)
            {
                bool result = false;
                monkeyLeft = "";
                monkeyRight = "";
                if (operation.Contains(op))
                {
                    result = true;
                    monkeyLeft = operation.Split(op)[0].Trim();
                    monkeyRight = operation.Split(op)[1].Trim();
                }
                return result;
            }
        }
        static void Main(string[] args)
        {
            Console.WriteLine(PuzzleOutputFormatter.getPuzzleCaption("Day 21: Monkey Math"));
            Console.WriteLine("Monkey shouts: ");
            PuzzleInput puzzleInput = new(PuzzleOutputFormatter.getPuzzleFilePath(), true);

            List<Monkey> monkeys = CreateMonkeys(puzzleInput.Lines);
            Console.WriteLine("Number of root: {0}", monkeys.Find( m=> m.Name == "root").Calculate(monkeys));
        }

        private static List<Monkey> CreateMonkeys(List<string> lines)
        {
            List<Monkey> monkeys = new List<Monkey>();

            foreach(string line in lines)
            {
                monkeys.Add(new Monkey(line));
            }

            return monkeys;
        }
    }
}