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

        static List<(Coordinate coordinate, Facing facing)> way = new List<(Coordinate coordinate, Facing facing)>();

        static void Main(string[] args)
        {
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.SetCursorPosition(0, 0);
            Console.SetBufferSize(250, 250);
            Console.SetWindowSize(200, 50);

            Console.WriteLine(PuzzleOutputFormatter.getPuzzleCaption("Day 22: Monkey Map"));
            Console.WriteLine("Notes: ");
            PuzzleInput puzzleInput = new(PuzzleOutputFormatter.getPuzzleFilePath(), true);

            char[,] map = PuzzleConverter.getInputAsMatrixChar(puzzleInput.Lines.Take(puzzleInput.Lines.Count - 1).ToList(), UNDEFINED);
            string instructions = puzzleInput.Lines.Last();

            (Coordinate coordinate, Facing facing) endpoint = TravelMap(map, DetermineStartingPoint(map), instructions, false);
            Console.WriteLine("final password: {0}", 1000 * (endpoint.coordinate.Y + 1) + 4 * (endpoint.coordinate.X + 1) + endpoint.facing);
            Console.WriteLine("Second part only for the given puzzle input");
            way.Clear();
            endpoint = TravelMap(map, DetermineStartingPoint(map), instructions, true);
            Console.WriteLine("final password cube: {0}", 1000 * (endpoint.coordinate.Y + 1) + 4 * (endpoint.coordinate.X + 1) + endpoint.facing);

            
            //TravelMapDebug(map, (new Coordinate(53,0), Facing.Left), "5");
            //TravelMapDebug(map, (new Coordinate(50,3), Facing.Up), "5");
            //TravelMapDebug(map, (new Coordinate(100,3), Facing.Up), "5");
            //TravelMapDebug(map, (new Coordinate(147, 0), Facing.Right), "5");
            //TravelMapDebug(map, (new Coordinate(100, 47), Facing.Down), "5");
            //TravelMapDebug(map, (new Coordinate(97, 50), Facing.Right), "5");
            //TravelMapDebug(map, (new Coordinate(97, 100), Facing.Right), "5");
            //TravelMapDebug(map, (new Coordinate(50, 147), Facing.Down), "5");
            //TravelMapDebug(map, (new Coordinate(47, 150), Facing.Right), "5");
            //TravelMapDebug(map, (new Coordinate(0, 197), Facing.Down), "5");
            //TravelMapDebug(map, (new Coordinate(3, 150), Facing.Left), "5");
            //TravelMapDebug(map, (new Coordinate(3, 100), Facing.Left), "5");
            //TravelMapDebug(map, (new Coordinate(0, 103), Facing.Up), "5");
            //TravelMapDebug(map, (new Coordinate(53, 50), Facing.Left), "5");
            //TravelMapDebug(map, (new Coordinate(53, 0), Facing.Left), "5");

            for (int y = 0; y < map.GetLength(1); y++)
            {
                for (int x = 0; x < map.GetLength(0); x++)
                {
                    if (way.Exists(w => w.coordinate.X == x && w.coordinate.Y == y))
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        (Coordinate coordinate, Facing facing) element = way.Find(w => w.coordinate.X == x && w.coordinate.Y == y);
                        switch (element.facing)
                        {
                            case Facing.Left: Console.Write('<'); break;
                            case Facing.Right: Console.Write('>'); break;
                            case Facing.Up: Console.Write('^'); break;
                            case Facing.Down: Console.Write('v'); break;
                        }
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Gray;
                        Console.Write(map[x, y]);
                    }
                }
                Console.Write('\n');

            }      
                
        }

        private static void TravelMapDebug(char[,] map, (Coordinate c, Facing d) pos, string instruction)
        {
            if(pos.d == Facing.Right || pos.d == Facing.Left)
            {
                for(int y = 0; y <= 25; y++)
                {
                    TravelMap(map, (new Coordinate(pos.c.X, pos.c.Y + y), pos.d), instruction, true);
                }
            }
            else 
            {
                for (int x = 0; x <= 25; x++)
                {
                    TravelMap(map, (new Coordinate(pos.c.X + x, pos.c.Y), pos.d), instruction, true);
                }
            }
        }

        private static (Coordinate coordinate, Facing facing) TravelMap(char[,] map, (Coordinate coordinate, Facing facing) startingPoint, string instructions, bool cube)
        {
            (Coordinate coordinate, Facing facing) currentPoint = startingPoint;
            
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
                    way.Add(movingForward);
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
                    newPosition = MoveRightOutOfArea(map, newPosition, cube);
                }
            }
            else if (move.facing == Facing.Left)
            {
                newPosition.coordinate.Move(-1,0);
                if (newPosition.coordinate.X < 0 || map[newPosition.coordinate.X, newPosition.coordinate.Y] == UNDEFINED)
                {
                    newPosition = MoveLeftOutOfArea(map, newPosition, cube);
                }
            }
            else if (move.facing == Facing.Up)
            {
                newPosition.coordinate.Move(0,-1);
                if (newPosition.coordinate.Y < 0 || map[newPosition.coordinate.X, newPosition.coordinate.Y] == UNDEFINED)
                {
                    newPosition = MoveUpOutOfArea(map, newPosition, cube);
                }
            }
            else if (move.facing == Facing.Down)
            {
                newPosition.coordinate.Move(0,1);
                if (newPosition.coordinate.Y >= mapHeight || map[newPosition.coordinate.X, newPosition.coordinate.Y] == UNDEFINED)
                {
                    newPosition = MoveDownOutOfArea(map, newPosition, cube);
                }
            }

            return newPosition;
        }

        private static (Coordinate coordinate, Facing facing) MoveUpOutOfArea(char[,] map, (Coordinate coordinate, Facing facing) position, bool cube)
        {
            if (!cube)
            {
                for (int y = map.GetLength(1) - 1; y >= 0; y--)
                {
                    if (map[position.coordinate.X, y] != UNDEFINED)
                    {
                        return (new Coordinate(position.coordinate.X, y), position.facing);
                    }
                }
            }
            else
            {
                int subMapHeight = 50;
                int subMapX = position.coordinate.X % subMapHeight;
                int subMapLevel = position.coordinate.X / subMapHeight;
                if (subMapLevel == 0)
                {
                    Coordinate coordinate = new Coordinate(subMapHeight, 1 * subMapHeight + subMapX);
                    Facing facing = Facing.Right;
                    return (coordinate, facing);
                }
                else if (subMapLevel == 1)
                {
                    Coordinate coordinate = new Coordinate(0, 3 * subMapHeight + subMapX);
                    Facing facing = Facing.Right;
                    return (coordinate, facing);
                }
                else if (subMapLevel == 2)
                {
                    Coordinate coordinate = new Coordinate(subMapX, 4 * subMapHeight - 1);
                    Facing facing = Facing.Up;
                    return (coordinate, facing);
                }
            }
            
            throw new Exception();
        }
        private static (Coordinate coordinate, Facing facing) MoveDownOutOfArea(char[,] map, (Coordinate coordinate, Facing facing) position, bool cube)
        {
            if (!cube)
            {
                for (int y = 0; y < map.GetLength(1); y++)
                {
                    if (map[position.coordinate.X, y] != UNDEFINED)
                    {
                        return (new Coordinate(position.coordinate.X, y), position.facing);
                    }
                }
            }
            else
            {
                int subMapHeight = 50;
                int subMapX = position.coordinate.X % subMapHeight;
                int subMapLevel = position.coordinate.X / subMapHeight;
                if (subMapLevel == 0)
                {
                    Coordinate coordinate = new Coordinate(2 * subMapHeight + subMapX, 0);
                    Facing facing = Facing.Down;
                    return (coordinate, facing);
                }
                else if (subMapLevel == 1)
                {
                    Coordinate coordinate = new Coordinate(1 * subMapHeight - 1, 3 * subMapHeight + subMapX);
                    Facing facing = Facing.Left;
                    return (coordinate, facing);
                }
                else if (subMapLevel == 2)
                {
                    Coordinate coordinate = new Coordinate(2 * subMapHeight - 1, 1 * subMapHeight + subMapX);
                    Facing facing = Facing.Left;
                    return (coordinate, facing);
                }
            }
   
            throw new Exception();
        }

        private static (Coordinate coordinate, Facing facing) MoveRightOutOfArea(char[,] map, (Coordinate coordinate, Facing facing) position, bool cube)
        {
            if (!cube)
            {
                for (int x = 0; x < map.GetLength(0); x++)
                {
                    if (map[x, position.coordinate.Y] != UNDEFINED)
                    {
                        return (new Coordinate(x, position.coordinate.Y), position.facing);
                    }
                }
            }
            else
            {
                int subMapHeight = 50;
                int subMapY = position.coordinate.Y % subMapHeight;
                int subMapLevel = position.coordinate.Y / subMapHeight;
                if (subMapLevel == 0)
                {
                    Coordinate coordinate = new Coordinate(2 * subMapHeight - 1, 3 * subMapHeight - subMapY  - 1);
                    Facing facing = Facing.Left;
                    return (coordinate, facing);
                }
                else if (subMapLevel == 1)
                {
                    Coordinate coordinate = new Coordinate(2 * subMapHeight + subMapY, subMapHeight - 1);
                    Facing facing = Facing.Up;
                    return (coordinate, facing);
                }
                else if (subMapLevel == 2)
                {
                    Coordinate coordinate = new Coordinate(3 * subMapHeight - 1, 1 * subMapHeight - subMapY  - 1);
                    Facing facing = Facing.Left;
                    return (coordinate, facing);
                }
                else if (subMapLevel == 3)
                {
                    Coordinate coordinate = new Coordinate(1 * subMapHeight + subMapY, 3 * subMapHeight - 1);
                    Facing facing = Facing.Up;
                    return (coordinate, facing);
                }
            }
            throw new Exception();
        }
        private static (Coordinate coordinate, Facing facing) MoveLeftOutOfArea(char[,] map, (Coordinate coordinate, Facing facing) position, bool cube)
        {
            if (!cube)
            {
                for (int x = map.GetLength(0) - 1; x >= 0; x--)
                {
                    if (map[x, position.coordinate.Y] != UNDEFINED)
                    {
                        return (new Coordinate(x, position.coordinate.Y), position.facing);
                    }
                }
            }
            else
            {
                int subMapHeight = 50;
                int subMapY = position.coordinate.Y % subMapHeight;
                int subMapLevel = position.coordinate.Y / subMapHeight;
                if (subMapLevel == 0)
                {
                    Coordinate coordinate = new Coordinate(0, 3 * subMapHeight - subMapY - 1);
                    Facing facing = Facing.Right;
                    return (coordinate, facing);
                }
                else if (subMapLevel == 1)
                {
                    Coordinate coordinate = new Coordinate(subMapY, 2 * subMapHeight);
                    Facing facing = Facing.Down;
                    return (coordinate, facing);
                }
                else if (subMapLevel == 2)
                {
                    Coordinate coordinate = new Coordinate(subMapHeight, 1 * subMapHeight - subMapY - 1);
                    Facing facing = Facing.Right;
                    return (coordinate, facing);
                }
                else if (subMapLevel == 3)
                {
                    Coordinate coordinate = new Coordinate(1 * subMapHeight + subMapY, 0);
                    Facing facing = Facing.Down;
                    return (coordinate, facing);
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