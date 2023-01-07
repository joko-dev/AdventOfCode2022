using AdventOfCode2022.SharedKernel;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using static Day23.Program;

namespace Day23
{
    internal class Program
    {
        internal enum Direction
        {
            North, South, East, West
        }

        internal class Move
        {
            internal Coordinate MoveDirection { get; }
            internal List<Coordinate> CheckDirection { get; }

            internal Move(Direction direction)
            {
                CheckDirection = new List<Coordinate>();
                switch (direction)
                {
                    case Direction.North:
                        {
                            MoveDirection = new Coordinate(0, -1);
                            CheckDirection.Add(new Coordinate(-1, -1));
                            CheckDirection.Add(new Coordinate(0, -1));
                            CheckDirection.Add(new Coordinate(1, -1));
                            break;
                        }
                    case Direction.South:
                        {
                            MoveDirection = new Coordinate(0, 1);
                            CheckDirection.Add(new Coordinate(-1, 1));
                            CheckDirection.Add(new Coordinate(0, 1));
                            CheckDirection.Add(new Coordinate(1, 1));
                            break;
                        }
                    case Direction.West:
                        {
                            MoveDirection = new Coordinate(-1, 0);
                            CheckDirection.Add(new Coordinate(-1, -1));
                            CheckDirection.Add(new Coordinate(-1, 0));
                            CheckDirection.Add(new Coordinate(-1, 1));
                            break;
                        }
                    case Direction.East:
                        {
                            MoveDirection = new Coordinate(1, 0);
                            CheckDirection.Add(new Coordinate(1, -1));
                            CheckDirection.Add(new Coordinate(1, 0));
                            CheckDirection.Add(new Coordinate(1, 1));
                            break;
                        }
                    default: throw new ArgumentOutOfRangeException();
                }
            }
        }

        internal class Elve
        {
           internal Coordinate Position { get; }
           internal Coordinate? Direction { get; set; }
           internal Elve(Coordinate position)
            {
                Position = position;
                Direction = null;
            }

            internal Coordinate SimulateMove() 
            {
                Coordinate simulation = new Coordinate(Position);
                if(Direction is not null)
                {
                    simulation.Move(Direction);
                }

                return simulation;
            }

            internal void Move()
            {
                if(Direction is not null)
                {
                    Position.Move(Direction);
                }
                Direction = null;
            }
        }

        static void Main(string[] args)
        {
            Console.WriteLine(PuzzleOutputFormatter.getPuzzleCaption("Day 23: Unstable Diffusion"));
            Console.WriteLine("Grove: ");
            PuzzleInput puzzleInput = new(PuzzleOutputFormatter.getPuzzleFilePath(), true);

            int lastRound;
            List<Elve> elves = GetElves(PuzzleConverter.getInputAsMatrixChar(puzzleInput.Lines, null));
            elves = MoveElves(elves, out lastRound, 10);

            Console.WriteLine("Empty ground files in smallest rectangle: {0}", CountEmptyGroundFiles(elves));

            elves = GetElves(PuzzleConverter.getInputAsMatrixChar(puzzleInput.Lines, null));
            elves = MoveElves(elves, out lastRound);
            Console.WriteLine("First Round where no elf moves: {0}", lastRound);
        }

        private static List<Elve> MoveElves(List<Elve> elves, out int lastRound, int rounds = 0)
        {
            List<Move> moves = GenerateMoves();
            int round = 1;
            bool isElfMoving = true;

            while((rounds == 0 || round <= rounds ) && isElfMoving)
            {
                isElfMoving = false;
                //PrintElves(elves);

                // Initialize Direction
                foreach (Elve elf in elves)
                {
                    elf.Direction = GetDirectionForElve(elf, elves, moves);
                }

                foreach(Elve elf in elves)
                {
                    if (elf.Direction is not null && !elves.Exists(e => e != elf && e.SimulateMove().Equals(elf.SimulateMove())))
                    {
                        elf.Move();
                        isElfMoving = true;
                    }
                }

                round++;

                moves.Add(moves[0]);
                moves.RemoveAt(0);
            }

            lastRound = round - 1;

            return elves;
        }

        private static void PrintElves(List<Elve> elves)
        {
            int minY = elves.Min(e => e.Position.Y);
            int minX = elves.Min(e => e.Position.X);
            int maxY = elves.Max(e => e.Position.Y);
            int maxX = elves.Max(e => e.Position.X);

            for (int y = minY; y <= maxY; y++)
            {
                for (int x = minX; x <= maxX; x++)
                {
                    if (!elves.Exists(e => e.Position.X == x && e.Position.Y == y))
                    {
                        Console.Write('.');
                    }
                    else
                    {
                        Console.Write('#');
                    }
                }
                Console.Write('\n');
            }
            Console.WriteLine("===========");
        }

        private static Coordinate? GetDirectionForElve(Elve elve, List<Elve> elves, List<Move> moves)
        {
            List<Coordinate> adjacents = elve.Position.GetAdjacentCoordinates();

            if (elves.Exists(e => adjacents.Exists(a => a.Equals(e.Position))))
            {
                foreach (Move move in moves)
                {
                    bool existsElve = false;

                    foreach (Coordinate check in move.CheckDirection)
                    {
                        Coordinate positionToCheck = new Coordinate(elve.Position);
                        positionToCheck.Move(check);
                        if (elves.Exists(e => e.Position.Equals(positionToCheck)))
                        {
                            existsElve = true;
                            break;
                        }
                    }

                    if (!existsElve)
                    {
                        return move.MoveDirection;
                    }
                }
            }

            return null;
        }

        private static List<Move> GenerateMoves()
        {
            List<Move> moves = new List<Move>();

            moves.Add(new Move(Direction.North));
            moves.Add(new Move(Direction.South));
            moves.Add(new Move(Direction.West));
            moves.Add(new Move(Direction.East));

            return moves;
        }

        private static int CountEmptyGroundFiles(List<Elve> elves)
        {
            int minY = elves.Min(e => e.Position.Y);
            int minX = elves.Min(e => e.Position.X);
            int maxY = elves.Max(e => e.Position.Y);
            int maxX = elves.Max(e => e.Position.X);
            int emptyGroundFiles = 0;

            for (int x = minX; x <= maxX; x++)
            {
                for (int y = minY; y <= maxY; y++)
                {
                    if (!elves.Exists(e => e.Position.X == x && e.Position.Y == y))
                    {
                        emptyGroundFiles++;
                    }
                }
            }

            return emptyGroundFiles;
        }

        private static List<Elve> GetElves(char[,] grove)
        {
            List<Elve> elves = new List<Elve>();

            for(int y = 0; y < grove.GetLength(1); y++)
            {
                for (int x = 0; x < grove.GetLength(0); x++)
                {
                    if (grove[x,y] == '#')
                    {
                        elves.Add(new Elve(new Coordinate(x,y)));
                    }
                }
            }

            return elves;
        }
    }
}