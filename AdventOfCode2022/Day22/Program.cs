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

            (Coordinate coordinate, Facing facing) endpoint = TravelMap(map, instructions, false);
            Console.WriteLine("final password: {0}", 1000 * (endpoint.coordinate.Y + 1) + 4 * (endpoint.coordinate.X + 1) + endpoint.facing);

            endpoint = TravelMap(map, instructions, true);
            Console.WriteLine("final password cube: {0}", 1000 * (endpoint.coordinate.Y + 1) + 4 * (endpoint.coordinate.X + 1) + endpoint.facing);
        }

        private static (Coordinate coordinate, Facing facing) TravelMap(char[,] map, string instructions, bool cube)
        {
            (Coordinate coordinate, Facing facing) currentPoint = DetermineStartingPoint(map);
            
            while(instructions.Length > 0)
            {
                string currentInstruction = StripInstruction(ref instructions);
                if (Int32.TryParse(currentInstruction, out int steps))
                {
                    currentPoint = Move(map, currentPoint, steps, cube);
                }
                else
                {
                    currentPoint = (currentPoint.coordinate, TurnAround(currentPoint.facing, currentInstruction));
                }
            }

            return currentPoint;
        }

        private static (Coordinate coordinate, Facing facing) Move(char[,] map, (Coordinate coordinate, Facing facing) currentPosition, int steps, bool cube)
        {
            (Coordinate coordinate, Facing facing) movingForward = (new Coordinate(currentPosition.coordinate), currentPosition.facing);
            for (int i = 1; i <= steps; i++)
            {
                (Coordinate coordinate, Facing facing) nextCoordinate = DetermineNextCoordinateFacing(map, movingForward, cube);
                if (map[nextCoordinate.coordinate.X, nextCoordinate.coordinate.Y] == WALL)
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

        private static (Coordinate coordinate, Facing facing) DetermineNextCoordinateFacing(char[,] map, (Coordinate coordinate, Facing facing) move, bool cube)
        {
            (Coordinate coordinate, Facing facing) newPosition = (new Coordinate(move.coordinate), move.facing);
            int mapWidth = map.GetLength(0);
            int mapHeight = map.GetLength(1);

            if (move.facing == Facing.Right)
            {
                newPosition.coordinate.Move(1,0);
                if (newPosition.coordinate.X >= mapWidth || map[newPosition.coordinate.X, newPosition.coordinate.Y] == UNDEFINED)
                {
                    newPosition = GetLeftX(map, newPosition);
                }
            }
            else if (move.facing == Facing.Left)
            {
                newPosition.coordinate.Move(-1,0);
                if (newPosition.coordinate.X < 0 || map[newPosition.coordinate.X, newPosition.coordinate.Y] == UNDEFINED)
                {
                    newPosition = GetRightX(map, newPosition);
                }
            }
            else if (move.facing == Facing.Up)
            {
                newPosition.coordinate.Move(0,-1);
                if (newPosition.coordinate.Y < 0 || map[newPosition.coordinate.X, newPosition.coordinate.Y] == UNDEFINED)
                {
                    newPosition = GetLowerY(map, newPosition);
                }
            }
            else if (move.facing == Facing.Down)
            {
                newPosition.coordinate.Move(0,1);
                if (newPosition.coordinate.Y >= mapHeight || map[newPosition.coordinate.X, newPosition.coordinate.Y] == UNDEFINED)
                {
                    newPosition = GetUpperY(map, newPosition);
                }
            }

            return newPosition;
        }

        private static (Coordinate coordinate, Facing facing) GetLowerY(char[,] map, (Coordinate coordinate, Facing facing) position)
        {
            for (int y = map.GetLength(1) - 1; y >= 0; y--)
            {
                if (map[position.coordinate.X, y] != UNDEFINED)
                {
                    return (new Coordinate(position.coordinate.X, y), position.facing);
                }
            }
            throw new Exception();
        }
        private static (Coordinate coordinate, Facing facing) GetUpperY(char[,] map, (Coordinate coordinate, Facing facing) position)
        {
            for (int y = 0; y < map.GetLength(1); y++)
            {
                if (map[position.coordinate.X, y] != UNDEFINED)
                {
                    return (new Coordinate(position.coordinate.X, y), position.facing);
                }
            }
            throw new Exception();
        }

        private static (Coordinate coordinate, Facing facing) GetLeftX(char[,] map, (Coordinate coordinate, Facing facing) position)
        {
            for (int x = 0; x < map.GetLength(0); x++)
            {
                if (map[x, position.coordinate.Y] != UNDEFINED)
                {
                    return (new Coordinate(x, position.coordinate.Y), position.facing);
                }
            }
            throw new Exception();
        }
        private static (Coordinate coordinate, Facing facing) GetRightX(char[,] map, (Coordinate coordinate, Facing facing) position)
        {
            for (int x = map.GetLength(0) - 1; x >= 0; x--)
            {
                if (map[x, position.coordinate.Y] != UNDEFINED)
                {
                    return (new Coordinate(x, position.coordinate.Y), position.facing);
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