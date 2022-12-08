using AdventOfCode2022.SharedKernel;
using System.Security;

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
            Console.WriteLine("Highest scenic score: {0}", GetHighestScenicScore(treeMap));
        }

        private static int GetHighestScenicScore(int[,] treeMap)
        {
            int highestScore = 0;
            
            for(int y = 0; y < treeMap.GetLength(1); y++)
            {
                for (int x = 0; x < treeMap.GetLength(0); x++)
                {
                    int score = GetScenicScore(treeMap, x, y);
                    if(score > highestScore)
                    {
                        highestScore = score;
                    }
                }
            }

            return highestScore;
        }

        private static int GetScenicScore(int[,] treeMap, int x, int y)
        {
            int score = 0;
            
            score = GetTreesUp(treeMap, x, y) * GetTreesDown(treeMap, x, y) * GetTreesRight(treeMap, x, y) * GetTreesLeft(treeMap, x, y);

            return score;
        }

        private static int GetTreesUp(int[,] treeMap, int x, int y)
        {
            int currentHeight = treeMap[x,y];
            int count = 0;

            for (int newY = y - 1; newY >= 0; newY--)
            {
                count++;
                if (treeMap[x, newY] >= currentHeight)
                {
                    break;
                }
            }

            return count;
        }
        private static int GetTreesDown(int[,] treeMap, int x, int y)
        {
            int currentHeight = treeMap[x, y];
            int count = 0;

            for (int newY = y + 1; newY < treeMap.GetLength(1); newY++)
            {
                count++;
                if (treeMap[x, newY] >= currentHeight)
                {
                    break;
                }
            }

            return count;
        }

        private static int GetTreesLeft(int[,] treeMap, int x, int y)
        {
            int currentHeight = treeMap[x, y];
            int count = 0;

            for (int newX = x - 1; newX >= 0; newX--)
            {
                count++;
                if (treeMap[newX, y] >= currentHeight)
                {
                    break;
                }
            }

            return count;
        }

        private static int GetTreesRight(int[,] treeMap, int x, int y)
        {
            int currentHeight = treeMap[x, y];
            int count = 0;

            for (int newX = x + 1; newX < treeMap.GetLength(0); newX++)
            {
                count++;
                if (treeMap[newX, y] >= currentHeight)
                {
                    break;
                }
            }

            return count;
        }

        private static int GetCountVisibleTrees(int[,] treeMap)
        {
            List<Coordinate> visibleTrees = new List<Coordinate>();

            GetVisibleTreesLine(visibleTrees, treeMap);
            GetVisibleTreesColumn(visibleTrees, treeMap);

            return visibleTrees.Count;
        }

        private static void GetVisibleTreesColumn(List<Coordinate> visibleTrees, int[,] treeMap)
        {
            for (int x = 0; x < treeMap.GetLength(0); x++)
            {
                int lastY = 0;
                int lastHeight = -1;
                for (int y = 0; y < treeMap.GetLength(1); y++)
                {
                    if (treeMap[x, y] > lastHeight)
                    {
                        Coordinate tree = new Coordinate(x, y);
                        
                        CheckAndAddTree(visibleTrees, tree);
                        lastY = y;
                        lastHeight = treeMap[x, y];
                    }
                }

                lastHeight = -1;
                for (int y = treeMap.GetLength(1) - 1; y > lastY; y--)
                {
                    if (treeMap[x, y] > lastHeight)
                    {
                        Coordinate tree = new Coordinate(x, y);
                        CheckAndAddTree(visibleTrees, tree);
                        lastHeight = treeMap[x, y];
                    }
                }
            }
        }

        private static void GetVisibleTreesLine(List<Coordinate> visibleTrees, int[,] treeMap)
        {
            for (int y = 0; y < treeMap.GetLength(1); y++)
            {
                int lastX = 0;
                int lastHeight = -1;

                for (int x = 0; x < treeMap.GetLength(0); x++)
                {
                    if (treeMap[x, y] > lastHeight)
                    {
                        Coordinate tree = new Coordinate(x, y);
                        CheckAndAddTree(visibleTrees, tree);
                        lastX = x;
                        lastHeight = treeMap[x, y];
                    }
                }

                lastHeight = -1;
                for (int x = treeMap.GetLength(0) - 1; x > lastX; x--)
                {
                    if (treeMap[x, y] > lastHeight)
                    {
                        Coordinate tree = new Coordinate(x, y);
                        CheckAndAddTree(visibleTrees, tree);
                        lastHeight = treeMap[x, y];
                    }
                }
            }
        }

        private static void CheckAndAddTree(List<Coordinate> visibleTrees, Coordinate tree)
        {
            if (!visibleTrees.Any(t => t.Equals(tree)))
            {
                visibleTrees.Add(tree);
            }
        }

    }
}