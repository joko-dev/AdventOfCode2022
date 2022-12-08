using AdventOfCode2022.SharedKernel;

namespace Day08
{
    public struct Coordinate
    {
        public int X { get;  }
        public int Y { get;  }
        public Coordinate(int x, int y)
        {
            this.X = x;
            this.Y = y;
        }
        public bool Equals(Coordinate other)
        {
            return (this.X == other.X && this.Y == other.Y);
        }
    }

    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(PuzzleOutputFormatter.getPuzzleCaption("Day 08: Treetop Tree House"));
            Console.WriteLine("Tree heights output: ");
            PuzzleInput puzzleInput = new(PuzzleOutputFormatter.getPuzzleFilePath(), false);

            int[,] treeMap = PuzzleConverter.getInputAsMatrixInt(puzzleInput.Lines);

            Console.WriteLine("Count visible trees: {0}", GetCountVisibleTrees(treeMap));
        }

        private static int GetCountVisibleTrees(int[,] treeMap)
        {
            List<Coordinate> visibleTrees = new List<Coordinate>();

            Console.WriteLine("========");
            GetVisibleTreesLine(visibleTrees, treeMap);
            Console.WriteLine("========");
            GetVisibleTreesColumn(visibleTrees, treeMap);
            Console.WriteLine("========");
            GetVisibleTreesBorder(visibleTrees, treeMap);

            return visibleTrees.Count;
        }

        private static void GetVisibleTreesBorder(List<Coordinate> visibleTrees, int[,] treeMap)
        {
            for (int y = 0; y < treeMap.GetLength(1); y++)
            {
                Coordinate tree = new Coordinate(0, y);
                CheckAndAddTree(visibleTrees, tree);

                tree = new Coordinate(treeMap.GetLength(0) - 1, y);
                CheckAndAddTree(visibleTrees, tree);
            }

            for (int x = 0; x < treeMap.GetLength(0); x++)
            {
                Coordinate tree = new Coordinate(x, 0);
                CheckAndAddTree(visibleTrees, tree);

                tree = new Coordinate(x, treeMap.GetLength(1) - 1);
                CheckAndAddTree(visibleTrees, tree);
            }
        }

        private static void GetVisibleTreesColumn(List<Coordinate> visibleTrees, int[,] treeMap)
        {
            for (int x = 0; x < treeMap.GetLength(0); x++)
            {
                int lastY = 0;
                int highestTreeColumn = GetHighestTreeColumn(treeMap, x);
                for (int y = 0; y < treeMap.GetLength(1); y++)
                {
                    if (treeMap[x, y] == highestTreeColumn)
                    {
                        Coordinate tree = new Coordinate(x, y);
                        CheckAndAddTree(visibleTrees, tree);
                        lastY = y;
                        break;
                    }
                }

                for (int y = treeMap.GetLength(1) - 1; y > lastY; y--)
                {
                    if (treeMap[x, y] == highestTreeColumn)
                    {
                        Coordinate tree = new Coordinate(x, y);
                        CheckAndAddTree(visibleTrees, tree);

                        break;
                    }
                }
            }
        }

        private static void GetVisibleTreesLine(List<Coordinate> visibleTrees, int[,] treeMap)
        {
            for (int y = 0; y < treeMap.GetLength(1); y++)
            {
                int lastX = 0;

                int highestTreeLine = GetHighestTreeLine(treeMap, y);
                for (int x = 0; x < treeMap.GetLength(0); x++)
                {
                    if (treeMap[x, y] == highestTreeLine)
                    {
                        Coordinate tree = new Coordinate(x, y);
                        CheckAndAddTree(visibleTrees, tree);
                        lastX = x;

                        break;
                    }
                }

                for (int x = treeMap.GetLength(0) - 1; x > lastX; x--)
                {
                    if (treeMap[x, y] == highestTreeLine)
                    {
                        Coordinate tree = new Coordinate(x, y);
                        CheckAndAddTree(visibleTrees, tree);

                        break;
                    }
                }
            }
        }

        private static void CheckAndAddTree(List<Coordinate> visibleTrees, Coordinate tree)
        {
            if (!visibleTrees.Any(t => t.Equals(tree)))
            {
                visibleTrees.Add(tree);
                Console.WriteLine("Tree: {0}, {1}", tree.X, tree.Y);
            }
        }

        private static int GetHighestTreeLine(int[,] treeMap, int y)
        {
            int maxHeight = 0;

            for (int x = 0; x < treeMap.GetLength(0); x++)
            {
                if (treeMap[x, y] > maxHeight)
                {
                    maxHeight = treeMap[x, y];
                }
            }

            return maxHeight;
        }

        private static int GetHighestTreeColumn(int[,] treeMap, int x)
        {
            int maxHeight = 0;

            for (int y = 0; y < treeMap.GetLength(1); y++)
            {
                if (treeMap[x, y] > maxHeight)
                {
                    maxHeight = treeMap[x, y];
                }
            }

            return maxHeight;
        }
    }
}