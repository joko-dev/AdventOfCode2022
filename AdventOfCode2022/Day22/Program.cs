using AdventOfCode2022.SharedKernel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.AccessControl;

namespace Day22
{
    internal class Program
    {
        const char WALL = '#';
        const char OPEN = '.';
        const char UNDEFINED = ' ';
        const char CLOCKWISE = 'R';
        const char COUNTERCLOCKWISE = 'L';
        enum Facing
        {
            Right = 0, Down = 1, Left = 2, Up = 3
        }

        static void Main(string[] args)
        {
            Console.WriteLine(PuzzleOutputFormatter.getPuzzleCaption("Day 22: Monkey Map"));
            Console.WriteLine("Notes: ");
            PuzzleInput puzzleInput = new(PuzzleOutputFormatter.getPuzzleFilePath(), true);

            char[,] map = PuzzleConverter.getInputAsMatrixChar(puzzleInput.Lines.Take(puzzleInput.Lines.Count - 1).ToList(), UNDEFINED);
            string instructions = puzzleInput.Lines.Last();

            (Coordinate coordinate, Facing facing) endpoint = TravelMap(map, instructions);
            Console.WriteLine("final password: {0}", 1000 * (endpoint.coordinate.Y + 1) + 4 * (endpoint.coordinate.X + 1) + endpoint.facing);
        }

        private static (Coordinate coordinate, Facing facing) TravelMap(char[,] map, string instructions)
        {
            (Coordinate coordinate, Facing facing) currentPoint = DetermineStartingPoint(map);
            
            while(instructions.Length > 0)
            {
                string currentInstruction = StripInstruction(ref instructions);
                if (Int32.TryParse(currentInstruction, out int steps))
                {
                    currentPoint = (Move(map, currentPoint, steps), currentPoint.facing);
                }
                else
                {
                    currentPoint = (currentPoint.coordinate, TurnAround(currentPoint.facing, currentInstruction));
                }
            }

            return currentPoint;
        }

        private static Coordinate Move(char[,] map, (Coordinate coordinate, Facing facing) currentPosition, int steps)
        {
            Coordinate movingForward = new Coordinate(currentPosition.coordinate);
            for(int i = 1; i <= steps; i++)
            {
                Coordinate nextCoordinate = DetermineNextCoordinateFacing(map, movingForward, currentPosition.facing);
                if (map[nextCoordinate.X, nextCoordinate.Y] == WALL)
                {
                    break;
                }
                else
                {
                    movingForward = nextCoordinate;
                }
            }
            return movingForward;
        }

        private static Coordinate DetermineNextCoordinateFacing(char[,] map, Coordinate coordinate, Facing facing)
        {
            int x = -1; int y = -1;
            int mapWidth = map.GetLength(0);
            int mapHeight = map.GetLength(1);

            if (facing == Facing.Right)
            {
                x = coordinate.X + 1;
                y = coordinate.Y;
                if ( x >= mapWidth || map[x, y] == UNDEFINED)
                {
                    x = GetLeftX(map, y);
                }
            }
            else if (facing == Facing.Left)
            {
                x = coordinate.X - 1;
                y = coordinate.Y;
                if (x < 0 || map[x, y] == UNDEFINED)
                {
                    x = GetRightX(map, y);
                }
            }
            else if (facing == Facing.Up)
            {
                x = coordinate.X;
                y = coordinate.Y - 1;
                if (y < 0 || map[x, y] == UNDEFINED)
                {
                    y = GetLowerY(map, x);
                }
            }
            else if (facing == Facing.Down)
            {
                x = coordinate.X;
                y = coordinate.Y + 1;
                if (y >= mapHeight || map[x, y] == UNDEFINED)
                {
                    y = GetUpperY(map, x);
                }
            }

            return new Coordinate(x, y);
        }

        private static int GetLowerY(char[,] map, int x)
        {
            for (int y = map.GetLength(1) - 1; y >= 0; y--)
            {
                if (map[x, y] != UNDEFINED)
                {
                    return y;
                }
            }
            throw new Exception();
        }
        private static int GetUpperY(char[,] map, int x)
        {
            for (int y = 0; y < map.GetLength(1); y++)
            {
                if (map[x, y] != UNDEFINED)
                {
                    return y;
                }
            }
            throw new Exception();
        }

        private static int GetLeftX(char[,] map, int y)
        {
            for (int x = 0; x < map.GetLength(0); x++)
            {
                if (map[x,y] != UNDEFINED)
                {
                    return x;
                }
            }
            throw new Exception();
        }
        private static int GetRightX(char[,] map, int y)
        {
            for (int x = map.GetLength(0) - 1; x >= 0; x--)
            {
                if (map[x, y] != UNDEFINED)
                {
                    return x;
                }
            }
            throw new Exception();
        }

        private static string StripInstruction(ref string instructions)
        {
            string nextInstruction = "";
            string strippedInstructions = instructions;

            while (strippedInstructions.Length > 0)
            {
                nextInstruction += strippedInstructions[0];
                strippedInstructions = strippedInstructions.Substring(1);
                if (nextInstruction[0] == CLOCKWISE || nextInstruction[0] == COUNTERCLOCKWISE)
                {
                    break;
                }
                else if(strippedInstructions.Length > 0 && (strippedInstructions[0] == CLOCKWISE || strippedInstructions[0] == COUNTERCLOCKWISE))
                {
                    break;
                }
            }

            instructions = strippedInstructions;

            return nextInstruction;
        }

        private static Facing TurnAround(Facing facing, string currentInstruction)
        {
            Facing newFacing = facing;
            if (currentInstruction[0] == CLOCKWISE)
            {
                switch (facing)
                {
                    case Facing.Up: newFacing = Facing.Right; break;
                    case Facing.Right: newFacing = Facing.Down; break;
                    case Facing.Down: newFacing = Facing.Left; break;
                    case Facing.Left: newFacing = Facing.Up; break;
                    default: throw new Exception();
                }
            }
            else if (currentInstruction[0] ==  COUNTERCLOCKWISE)
            {
                switch (facing)
                {
                    case Facing.Up: newFacing = Facing.Left; break;
                    case Facing.Left: newFacing = Facing.Down; break;
                    case Facing.Down: newFacing = Facing.Right; break;
                    case Facing.Right: newFacing = Facing.Up; break;
                    default: throw new Exception();
                }
            }
            return newFacing;
        }

        private static (Coordinate coordinate, Facing facing) DetermineStartingPoint(char[,] map)
        {
            for(int x = 0; x < map.GetLength(0); x++)
            {
                if (map[x,0] == OPEN)
                {
                    return (new Coordinate(x,0), Facing.Right);
                }
            }
            throw new Exception();
        }
    }
}