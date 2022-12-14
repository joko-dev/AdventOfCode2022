using AdventOfCode2022.SharedKernel;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;

namespace Day14
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(PuzzleOutputFormatter.getPuzzleCaption("Day 14: Regolith Reservoir"));
            Console.WriteLine("Rock structures: ");
            PuzzleInput puzzleInput = new(PuzzleOutputFormatter.getPuzzleFilePath(), true);

            List<List<Coordinate>> structures = GetRockStructures(puzzleInput.Lines);
            char[,] cave = CreateCave(structures);
            PouringSandLoop(cave, new Coordinate(500, 0));
            Console.WriteLine("Units of sand before abyss: {0}", CountElements(cave, '+'));
        }

        private static int CountElements(char[,] cave, char element)
        {
            int count = 0;
            foreach(char c in cave)
            {
                if (c == element)
                {
                    count++;
                }
            }
            return count;
        }

        private static char[,] CreateCave(List<List<Coordinate>> structures)
        {
            // Hack: expand width, so that right fill is possible
            int width = structures.Max(s => s.Max( x => x.X)) + 100;
            int height = structures.Max(s => s.Max( y => y.Y)) + 2;

            char[,] cave = new char[width, height];
            PuzzleConverter.fillMatrix(cave, '.');
            foreach(List<Coordinate> structure in structures)
            {
                FillCaveWithStructure(cave, structure);
            }

            return cave;
        }

        private static void FillCaveWithStructure(char[,] cave, List<Coordinate> structure)
        {
            for(int i = 0; i < structure.Count - 1; i++)
            {
                Coordinate from = structure[i];
                Coordinate to = structure[i+1];

                // only straight lines are possible
                if(from.X != to.X)
                {
                    for (int x = Math.Min(from.X, to.X); x <= Math.Max(from.X, to.X); x++)
                    {
                        cave[x, from.Y] = '#';
                    }
                }
                else if (from.Y != to.Y) 
                {
                    for (int y = Math.Min(from.Y, to.Y); y <= Math.Max(from.Y, to.Y); y++)
                    {
                        cave[from.X, y] = '#';
                    }
                }
                else
                {
                    throw new Exception();
                }
            }
        }

        private static List<List<Coordinate>> GetRockStructures(List<string> lines)
        {
            List<List<Coordinate>> structures = new List<List<Coordinate>>();

            foreach(string line in lines)
            {
                List<Coordinate> structure = new List<Coordinate>();
                string[] elements = line.Split("->");
                foreach(string element in elements) 
                { 
                    structure.Add(new Coordinate(element));
                }
                structures.Add(structure);
            }

            return structures;
        }

        private static void PouringSandLoop(char[,] cave, Coordinate coordinate)
        {
            while (true)
            {
                if(!PouringSand(cave, coordinate))
                {
                    break;
                }
            }
        }
        private static bool PouringSand(char[,] cave, Coordinate coordinate)
        {

            Coordinate restPoint = GetRestPoint(cave, coordinate);
            // endless void reached
            if (restPoint == null)
            {
                return false;
            }
            // move diagonally left 
            else if (cave[restPoint.X - 1, restPoint.Y + 1] == '.')
            {
                return PouringSand(cave, new Coordinate(restPoint.X - 1, restPoint.Y + 1));
            }
            // move diagonally right
            else if (cave[restPoint.X + 1, restPoint.Y + 1] == '.')
            {
                return PouringSand(cave, new Coordinate(restPoint.X + 1, restPoint.Y + 1));
            }
            // rest
            else
            {
                cave[restPoint.X, restPoint.Y] = '+';
                return true;
            }

        }

        private static Coordinate GetRestPoint(char[,] cave, Coordinate coordinate)
        {
            Coordinate restPoint = null;
            for(int y = coordinate.Y + 1; y < cave.GetLength(1); y++)
            {
                if (cave[coordinate.X, y] == '#' || cave[coordinate.X, y] == '+')
                {
                    restPoint = new Coordinate(coordinate.X, y - 1);
                    break;
                }
            }
            return restPoint;
        }
    }
}
