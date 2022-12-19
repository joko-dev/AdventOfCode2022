using AdventOfCode2022.SharedKernel;

namespace Day18
{
    internal class Program
    {
        internal class Coordinate3D
        {
            internal int X { get; }
            internal int Y { get; }
            internal int Z { get; }
            internal Coordinate3D(int x, int y, int z)
            {
                X = x;
                Y = y;
                Z = z;
            }
            internal bool Equals(Coordinate3D coordinate)
            {
                return (this.X == coordinate.X && this.Y == coordinate.Y && this.Z == coordinate.Z);
            }
        }
        internal class Cube
        {
            internal Coordinate3D Coordinate { get; }

            internal Cube(int x, int y, int z)
            {
                Coordinate = new Coordinate3D(x, y, z);
            }
            internal bool IsAdjacent(Cube other)
            {
                int diff = Math.Abs(this.Coordinate.X - other.Coordinate.X) + Math.Abs(this.Coordinate.Y - other.Coordinate.Y) + Math.Abs(this.Coordinate.Z - other.Coordinate.Z);
                return diff == 1;
            }
        }
        static void Main(string[] args)
        {
            Console.WriteLine(PuzzleOutputFormatter.getPuzzleCaption("Day 18: Boiling Boulders"));
            Console.WriteLine("Cubes: ");
            PuzzleInput puzzleInput = new(PuzzleOutputFormatter.getPuzzleFilePath(), true);

            //Visualization: https://www.matheretter.de/geoservant/en
            List<Cube> cubes = CreateCubes(puzzleInput.Lines);

            Console.WriteLine("Surface area: {0}", GetSurfaceArea(cubes));
        }

        private static int GetSurfaceArea(List<Cube> cubes)
        {
            int surfaceArea = 0;

            foreach (Cube cube1 in cubes)
            {
                surfaceArea += GetSideNotConnected(cube1, cubes);
            }

            return surfaceArea;
        }

        private static int GetSideNotConnected(Cube cube1, List<Cube> cubes)
        {
            int sidesConnected = 0;

            foreach (Cube cube2 in cubes.Where(c => c != cube1))
            {
                if (cube1.IsAdjacent(cube2))
                {
                    sidesConnected++;
                }
            }
            return (6- sidesConnected);
        }

        private static bool ArePlanesEqual(List<Coordinate3D> plane1, List<Coordinate3D> plane2)
        {
            foreach(Coordinate3D point1 in plane1)
            {
                bool found = plane2.Exists(c => c.Equals(point1));
                if (!found)
                {
                    return false;
                }
            }
            return true;
        }

        private static List<Cube> CreateCubes(List<string> lines)
        {
            List<Cube> cubes = new List<Cube>();

            foreach(string line in lines)
            {
                string[] coordinates = line.Split(',');
                cubes.Add(new Cube(Int32.Parse(coordinates[0]), Int32.Parse(coordinates[1]), Int32.Parse(coordinates[2])));
            }

            return cubes;
        }
    }
}