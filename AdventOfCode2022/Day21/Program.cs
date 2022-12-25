using AdventOfCode2022.SharedKernel;
using System.Collections.Generic;

namespace Day21
{
    internal class Program
    {
        class Monkey
        {
            public string Name { get; }
            public string? MonkeyLeft { get; }
            public string? MonkeyRight { get; }
            public char? Operation { get; set; }
            public Int64? YellValue{ get; }
            public Monkey(string line)
            {
                string[] temp = line.Split(":");
                Name = temp[0];

                if (Int64.TryParse(temp[1], out Int64 value))
                {
                    YellValue = value;
                }
                else
                {
                    (string monkeyLeft, string monkeyRight, char op) elements = ExtractOperationItems(temp[1]);
                    MonkeyLeft = elements.monkeyLeft;
                    MonkeyRight = elements.monkeyRight;
                    Operation = elements.op;
                }
                
            }
            public Monkey(string name, string monkeyLeft, string monkeyRight, char op)
            {
                Name = name;
                MonkeyLeft = monkeyLeft;
                MonkeyRight = monkeyRight;
                Operation = op;
            }
            public Int64 Calculate(List<Monkey> monkeys)
            {
                if (YellValue.HasValue)
                {
                    return YellValue.Value;
                }

                Int64 result;
                
                if(Operation > 0)
                {
                    Monkey monkeyLeft = monkeys.Find(m => m.Name == MonkeyLeft);
                    Monkey monkeyRight = monkeys.Find(m => m.Name == MonkeyRight);

                    if (Operation == '+')
                    {
                        result = monkeyLeft.Calculate(monkeys) + monkeyRight.Calculate(monkeys);
                    }
                    else if (Operation == '-')
                    {
                        result = monkeyLeft.Calculate(monkeys) - monkeyRight.Calculate(monkeys);
                    }
                    else if (Operation == '*')
                    {
                        result = monkeyLeft.Calculate(monkeys) * monkeyRight.Calculate(monkeys);
                    }
                    else if (Operation == '/')
                    {
                        result = monkeyLeft.Calculate(monkeys) / monkeyRight.Calculate(monkeys);
                    }
                    else
                    {
                        throw new Exception();
                    }
                }
                else
                {
                    Monkey monkeyEqual = monkeys.Find(m => m.Name == MonkeyLeft);
                    result = monkeyEqual.Calculate(monkeys);
                }
                

                return result;
            }
            private (string monkeyLeft, string monkeyRight, char op) ExtractOperationItems(string operation)
            {
                operation = operation.Trim();
                string[] elements = operation.Split(" ");

                string monkeyLeft = elements[0];
                char op = elements[1][0];
                string monkeyRight = elements[2];

                return (monkeyLeft, monkeyRight, op);
            }
            public Monkey GenerateReversedMonkeyLeft()
            {
                Monkey monkeyReversed;

                if (this.Operation.Value == '+')
                {
                    monkeyReversed = new Monkey(this.MonkeyLeft, this.Name, this.MonkeyRight, '-');
                }
                else if (this.Operation.Value == '-')
                {
                    monkeyReversed = new Monkey(this.MonkeyLeft, this.Name, this.MonkeyRight, '+');
                }
                else if (this.Operation.Value == '*')
                {
                    monkeyReversed = new Monkey(this.MonkeyLeft, this.Name, this.MonkeyRight, '/');
                }
                else if (this.Operation.Value == '/')
                {
                    monkeyReversed = new Monkey(this.MonkeyLeft, this.Name, this.MonkeyRight, '*');
                }
                else if (this.Operation.Value == '=')
                {
                    monkeyReversed = new Monkey(this.MonkeyLeft, this.MonkeyRight, "", (char) 0);
                }
                else
                {
                    throw new Exception();
                }

                return monkeyReversed;
            }
            public Monkey GenerateReversedMonkeyRight()
            {
                Monkey monkeyReversed;

                if(this.Operation.Value == '+')
                {
                    monkeyReversed = new Monkey(this.MonkeyRight, this.Name, this.MonkeyLeft, '-');
                }
                else if (this.Operation.Value == '-')
                {
                    monkeyReversed = new Monkey(this.MonkeyRight, this.MonkeyLeft, this.Name, '-');
                }
                else if (this.Operation.Value == '*')
                {
                    monkeyReversed = new Monkey(this.MonkeyRight, this.Name, this.MonkeyLeft, '/');
                }
                else if (this.Operation.Value == '/')
                {
                    monkeyReversed = new Monkey(this.MonkeyRight, this.MonkeyLeft, this.Name, '/');
                }
                else if (this.Operation.Value == '=')
                {
                    monkeyReversed = new Monkey(this.MonkeyRight, this.MonkeyLeft, "", (char)0);
                }
                else
                {
                    throw new Exception();
                }

                return monkeyReversed;
            }

        }
        static void Main(string[] args)
        {
            Console.WriteLine(PuzzleOutputFormatter.getPuzzleCaption("Day 21: Monkey Math"));
            Console.WriteLine("Monkey shouts: ");
            PuzzleInput puzzleInput = new(PuzzleOutputFormatter.getPuzzleFilePath(), true);

            List<Monkey> monkeys = CreateMonkeys(puzzleInput.Lines);
            Console.WriteLine("Number of root: {0}", monkeys.Find( m=> m.Name == "root").Calculate(monkeys));

            // Idea: Reverse Monkey list. Switch Operations of all relevant elements. It seems that every Monkey only appears 1 time
            Console.WriteLine("Number to pass root equality test: {0}", GetNumberToPassEquality(monkeys));
        }

        private static Int64 GetNumberToPassEquality(List<Monkey> monkeys)
        {
            monkeys.Find(m => m.Name == "root").Operation = '=';
            monkeys.Remove(monkeys.Find(m => m.Name == "humn"));

            List<Monkey> reversedMonkeys = GenerateReverseOperationList("humn", monkeys);

            //add every monkey which has not been processed
            foreach (Monkey monkey in monkeys)
            {
                if(!reversedMonkeys.Exists(r => r.Name == monkey.Name))
                {
                    reversedMonkeys.Add(monkey);
                }
            }

            return reversedMonkeys.Find(m => m.Name == "humn").Calculate(reversedMonkeys);
        }

        private static List<Monkey> GenerateReverseOperationList(string monkeyName, List<Monkey> monkeys)
        {
            List<Monkey> reversedList = new List<Monkey>();
            (Monkey monkeyReversed, string switchedMonkeyName) monkeySwitch = (null, "");

            Monkey monkeyToReverse = monkeys.Find(m => m.MonkeyLeft == monkeyName);
            if(monkeyToReverse is not null)
            {
                monkeySwitch.monkeyReversed = monkeyToReverse.GenerateReversedMonkeyLeft();
                monkeySwitch.switchedMonkeyName = monkeyToReverse.Name;
            }
            else
            {
                monkeyToReverse = monkeys.Find(m => m.MonkeyRight == monkeyName);
                if (monkeyToReverse is not null)
                {
                    monkeySwitch.monkeyReversed = monkeyToReverse.GenerateReversedMonkeyRight();
                    monkeySwitch.switchedMonkeyName = monkeyToReverse.Name;
                }
            }

            if(monkeySwitch.monkeyReversed is not null)
            {
                reversedList.Add(monkeySwitch.monkeyReversed);
                reversedList.AddRange(GenerateReverseOperationList(monkeySwitch.switchedMonkeyName, monkeys));
            }


            return reversedList;
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