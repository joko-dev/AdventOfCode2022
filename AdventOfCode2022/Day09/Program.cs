using AdventOfCode2022.SharedKernel;
using System.ComponentModel;

namespace Day09
{
    
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(PuzzleOutputFormatter.getPuzzleCaption("Day 09: Rope Bridge"));
            Console.WriteLine("series of motions: ");
            PuzzleInput puzzleInput = new(PuzzleOutputFormatter.getPuzzleFilePath(), false);

            
            List<Coordinate> positionsVisited = MoveRope(puzzleInput, 2);
            Console.WriteLine("Visited positions (2 knots): {0}", positionsVisited.Count);

            positionsVisited = MoveRope(puzzleInput, 10);
            Console.WriteLine("Visited positions (10 knots): {0}", positionsVisited.Count);
            PrintVisited(positionsVisited);
        }

        private static void PrintVisited(List<Coordinate> positionsVisited)
        {
            int minX = positionsVisited.Min(x => x.X);
            int maxX = positionsVisited.Max(x => x.X);
            int minY = positionsVisited.Min(y => y.Y);
            int maxY = positionsVisited.Max(y => y.Y);

            char[,] output = new char[maxX + Int32.Abs(minX) + 1, maxY + Int32.Abs(minY) + 1];
            for(int x = 0; x < output.GetLength(0); x++)
            {
                for (int y = 0; y < output.GetLength(1); y++)
                {
                    output[x, y] = '.';
                }
            }

            foreach(Coordinate position in positionsVisited)
            {
                output[position.X + Int32.Abs(minX), position.Y + Int32.Abs(minY)] = '#';
            }

            for (int y = output.GetLength(1) - 1; y >= 0; y--)
            {
                for (int x = 0; x < output.GetLength(0); x++)
                {
                    Console.Write(output[x, y]);
                }

                Console.Write("\n");

            }
           
        }

        private static List<Coordinate> MoveRope(PuzzleInput puzzleInput, int countKnots)
        {
            List<Coordinate> visited = new List<Coordinate>();
            List<Coordinate> knots = new List<Coordinate>();
            for (int i = 1; i <= countKnots; i++)
            {
                knots.Add(new Coordinate(0, 0));
            }

            foreach (string line in puzzleInput.Lines)
            {
                char direction = line.Split(' ')[0][0];
                int steps = Int32.Parse(line.Split(' ')[1]);

                MoveHead(knots, direction, steps, visited);
              
            }

            return visited;
        }

        private static void MoveTail(Coordinate head, Coordinate tail)
        {
            int dx = head.X - tail.X;
            int dy = head.Y - tail.Y;

            if (Math.Abs(dx) > 1 || Math.Abs(dy) > 1)
            {
                tail.Move(Math.Sign(dx), Math.Sign(dy));
            }

            
        }

        private static void MoveHead(List<Coordinate> knots, char direction, int steps, List<Coordinate> visited)
        {
            Coordinate head = knots[0];

            for (int s = 1; s <= steps; s++)
            {
                if (direction == 'U')
                {
                    head.Move(0, 1);
                }
                else if (direction == 'D')
                {
                    head.Move(0, -1);
                }
                else if (direction == 'L')
                {
                    head.Move(-1, 0);
                }
                else if (direction == 'R')
                {
                    head.Move(1, 0);
                }

                for(int i = 1; i < knots.Count; i++)
                {
                    MoveTail(knots[i-1], knots[i]);
                   
                }

                if (!visited.Any(v => v.Equals(knots.Last())))
                {
                    visited.Add(new Coordinate(knots.Last()));
                }

            }
        }
    }
}