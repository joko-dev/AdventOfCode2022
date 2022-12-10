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

            List<Coordinate> postionsVisited = MoveRope(puzzleInput);

            Console.WriteLine("Visited positions`: {0}", postionsVisited.Count);
        }

        private static List<Coordinate> MoveRope(PuzzleInput puzzleInput)
        {
            List<Coordinate> visited = new List<Coordinate>();
            Coordinate head = new Coordinate(0, 0);
            Coordinate tail = new Coordinate(head);

            foreach (string line in puzzleInput.Lines)
            {
                char direction = line.Split(' ')[0][0];
                int steps = Int32.Parse(line.Split(' ')[1]);

                MoveHead(head, tail, direction, steps, visited);
              
            }

            return visited;
        }

        private static void MoveTail(Coordinate head, Coordinate tail)
        {
            int xToAdd = 0;
            int yToAdd = 0;
            int verticalDistance = head.Y - tail.Y;
            int horizontalDistance = head.X - tail.X;

            if (horizontalDistance > 1)
            {
                xToAdd = 1;
                if(verticalDistance != 0)
                {
                    yToAdd = head.Y - tail.Y;
                }
            }
            else if(horizontalDistance < -1 )
            {
                xToAdd = -1;
                if (verticalDistance != 0)
                {
                    yToAdd = head.Y - tail.Y;
                }
            }

            if (verticalDistance > 1)
            {
                yToAdd = 1;
                if (horizontalDistance != 0)
                {
                    xToAdd = head.X - tail.X;
                }
            }
            else if (verticalDistance < -1) 
            {
                yToAdd = -1;
                if (horizontalDistance != 0)
                {
                    xToAdd = head.X - tail.X;
                }
            }

            tail.Move(xToAdd, yToAdd);
        }

        private static void MoveHead(Coordinate head, Coordinate tail, char direction, int steps, List<Coordinate> visited)
        {
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

                MoveTail(head, tail);
                if (!visited.Any(v => v.Equals(tail)))
                {
                    visited.Add(new Coordinate(tail));
                }
            }
        }
    }
}