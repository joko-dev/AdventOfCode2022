using AdventOfCode2022.SharedKernel;

namespace Day11
{
    internal class Monkey
    {
        internal List<Int64> Items { get; }
        internal String Operation { get; }
        internal int TestDivisibile { get; }
        internal int TestPositiveTo { get; }
        internal int TestNegativeTo { get; }
        internal Int64 Inspections { get; private set; }
        public Monkey(string items, string operation, string testDivisible, string testPositiveTo, string testNegativeTo)
        {
            Inspections = 0;
            Items = new List<Int64>();
            items = items.Replace("Starting items:", "").Trim();
            Items.AddRange(items.Split(',').Select(Int64.Parse));
            Operation = operation.Replace("Operation: new = ", "").Trim();
            TestDivisibile = Int32.Parse(testDivisible.Replace("Test: divisible by ", "").Trim());
            TestPositiveTo = Int32.Parse(testPositiveTo.Replace("If true: throw to monkey", ""));
            TestNegativeTo = Int32.Parse(testNegativeTo.Replace("If false: throw to monkey", ""));
        }
        public (int newMonkeyTo, Int64 worryLevel) Inspect(int divideWorryLevelBy, int divisorLimit)
        {
            int newMonkeyTo = 0;
            bool testResult;
            Int64 worryLevel = Items[0];

            string[] worryLevelOperationCalculation = Operation.Split(" ");
            worryLevel = calculateNewWorryLevel(worryLevel, worryLevelOperationCalculation[0], worryLevelOperationCalculation[1], worryLevelOperationCalculation[2]);
            if(divideWorryLevelBy > 1)
            {
                worryLevel = (int) Math.Floor((decimal)worryLevel / divideWorryLevelBy);
            }

            worryLevel %= divisorLimit;
            testResult = (worryLevel % TestDivisibile) == 0;

            if (testResult)
            {
                newMonkeyTo = TestPositiveTo;
            }
            else 
            {
                newMonkeyTo = TestNegativeTo;
            }

            Inspections++;
            Items.RemoveAt(0);

            return (newMonkeyTo, worryLevel);
        }

        private Int64 calculateNewWorryLevel(Int64 oldWorryLevel, string left, string op, string right)
        {
            Int64 newWorryLevel = 0;
            Int64 leftSide = getWorryLevelCalculationValue(oldWorryLevel, left);
            Int64 rightSide = getWorryLevelCalculationValue(oldWorryLevel, right);

            if (op == "+")
            {
                newWorryLevel = leftSide + rightSide;
            }
            else if (op == "*")
            {
                newWorryLevel = leftSide * rightSide;
            }

            return newWorryLevel;
        }
        private Int64 getWorryLevelCalculationValue(Int64 oldWorryLevel, string valueString)
        {
            Int64 value = 0;

            if(valueString == "old")
            {
                value = oldWorryLevel;
            }
            else
            {
                value = Int64.Parse(valueString);
            }

            return value;
        }
        
    }

    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(PuzzleOutputFormatter.getPuzzleCaption("Day 11: Monkey in the Middle"));
            Console.WriteLine("Program: ");
            PuzzleInput puzzleInput = new(PuzzleOutputFormatter.getPuzzleFilePath(), false);

            List<Monkey> monkeys = BuildMonkeyList(puzzleInput);
            Console.WriteLine("Monkey business (20): {0}", GetMonkeyBusiness(monkeys, 20, 3));

            monkeys = BuildMonkeyList(puzzleInput);
            Console.WriteLine("Monkey business (10000): {0}", GetMonkeyBusiness(monkeys, 10000, 1));
        }

        private static List<Monkey> BuildMonkeyList(PuzzleInput puzzleInput)
        {
            List<Monkey> monkeys = new List<Monkey>();

            for(int i = 0; i < puzzleInput.Lines.Count; i+=7)
            {
                if (puzzleInput.Lines[i].StartsWith("Monkey"))
                {
                    Monkey monkey = new Monkey(puzzleInput.Lines[i+1], puzzleInput.Lines[i+2], puzzleInput.Lines[i+3],
                                                puzzleInput.Lines[i+4], puzzleInput.Lines[i + 5]);

                    monkeys.Add(monkey);
                }
                else
                {
                    throw new InvalidDataException();
                }
            }

            return monkeys;
            
        }

        private static Int64 GetMonkeyBusiness(List<Monkey> monkeys, int turns, int divideWorryLevelsBy)
        {
            Int64 monkeyBusiness = 0;
            var divisorLimit = monkeys.Aggregate(1, (c, m) => c * m.TestDivisibile);


            for (int i = 1; i <= turns; i++)
            {
                foreach(Monkey monkey in monkeys)
                {
                    while(monkey.Items.Count > 0)
                    {
                        (int newMonkeyTo, Int64 worryLevel) inspectionResult = monkey.Inspect(divideWorryLevelsBy, divisorLimit);
                        monkeys[inspectionResult.newMonkeyTo].Items.Add(inspectionResult.worryLevel);
                    }
                }
            }

            monkeys = monkeys.OrderByDescending(m => m.Inspections).ToList();
            monkeyBusiness = monkeys[0].Inspections * monkeys[1].Inspections;

            return monkeyBusiness;
        }
    }
}