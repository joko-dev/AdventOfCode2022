using AdventOfCode2022.SharedKernel;
using System.Numerics;
using System.Runtime.ExceptionServices;
using System.Security.Cryptography;

namespace Day11
{
    internal class Monkey
    {
        internal List<BigInteger> Items { get; }
        internal String Operation { get; }
        internal int TestDivisibile { get; }
        internal int TestPositiveTo { get; }
        internal int TestNegativeTo { get; }
        internal BigInteger Inspections { get; private set; }
        public Monkey(string items, string operation, string testDivisible, string testPositiveTo, string testNegativeTo)
        {
            Inspections = 0;
            Items = new List<BigInteger>();
            items = items.Replace("Starting items:", "").Trim();
            Items.AddRange(items.Split(',').Select(BigInteger.Parse));
            Operation = operation.Replace("Operation: new = ", "").Trim();
            TestDivisibile = Int32.Parse(testDivisible.Replace("Test: divisible by ", "").Trim());
            TestPositiveTo = Int32.Parse(testPositiveTo.Replace("If true: throw to monkey", ""));
            TestNegativeTo = Int32.Parse(testNegativeTo.Replace("If false: throw to monkey", ""));
        }
        public (int newMonkeyTo, BigInteger worryLevel) Inspect(int divideWorryLevelBy, int divisorLimit)
        {
            int newMonkeyTo = 0;
            bool testResult;
            BigInteger worryLevel = Items[0];

            string[] worryLevelOperationCalculation = Operation.Split(" ");
            worryLevel = calculateNewWorryLevel(worryLevel, worryLevelOperationCalculation[0], worryLevelOperationCalculation[1], worryLevelOperationCalculation[2]);
            if(divideWorryLevelBy > 1)
            {
                worryLevel = (BigInteger) Math.Floor((decimal)worryLevel / divideWorryLevelBy);
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

        private BigInteger calculateNewWorryLevel(BigInteger oldWorryLevel, string left, string op, string right)
        {
            BigInteger newWorryLevel = 0;
            BigInteger leftSide = getWorryLevelCalculationValue(oldWorryLevel, left);
            BigInteger rightSide = getWorryLevelCalculationValue(oldWorryLevel, right);

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
        private BigInteger getWorryLevelCalculationValue(BigInteger oldWorryLevel, string valueString)
        {
            BigInteger value = 0;

            if(valueString == "old")
            {
                value = oldWorryLevel;
            }
            else
            {
                value = BigInteger.Parse(valueString);
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

        private static BigInteger GetMonkeyBusiness(List<Monkey> monkeys, int turns, int divideWorryLevelsBy)
        {
            BigInteger monkeyBusiness = 0;
            var divisorLimit = monkeys.Aggregate(1, (c, m) => c * m.TestDivisibile);


            for (int i = 1; i <= turns; i++)
            {
                foreach(Monkey monkey in monkeys)
                {
                    while(monkey.Items.Count > 0)
                    {
                        (int newMonkeyTo, BigInteger worryLevel) inspectionResult = monkey.Inspect(divideWorryLevelsBy, divisorLimit);
                        monkeys[inspectionResult.newMonkeyTo].Items.Add(inspectionResult.worryLevel);
                    }
                }

                if(i == 1 | i == 20 | i == 1000 | i == 2000)
                {
                    Console.WriteLine("========");
                    foreach (Monkey monkey in monkeys)
                    {
                        Console.WriteLine(monkey.Inspections.ToString());
                    }
                }
            }

            monkeys = monkeys.OrderByDescending(m => m.Inspections).ToList();
            monkeyBusiness = monkeys[0].Inspections * monkeys[1].Inspections;

            return monkeyBusiness;
        }
    }
}