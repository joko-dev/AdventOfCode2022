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

            internal bool Equals(Cube other)
            {
                return this.Coordinate.Equals(other.Coordinate);
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

            // Part 2 - Flood fill. Get cubes outside of form, then check all frop cubes which are connected to the outside cubes
            List<Cube> outside = GetOutsideCubes(cubes);
            Console.WriteLine("Surface area: {0}", GetExteriorSurfaceArea(cubes, outside));
        }

        private static List<Cube> GetOutsideCubes(List<Cube> cubes)
        {
            List<Cube> outside = new List<Cube>();
            Cube start = new Cube(0, 0, 0);
            int maxX = cubes.Max(c => c.Coordinate.X) + 1;
            int maxY = cubes.Max(c => c.Coordinate.Y) + 1;
            int maxZ = cubes.Max(c => c.Coordinate.Z) + 1;

            int minX = cubes.Min(c => c.Coordinate.X) - 1;
            int minY = cubes.Min(c => c.Coordinate.Y) - 1;
            int minZ = cubes.Min(c => c.Coordinate.Z) - 1;

            floodFillOutside(outside, cubes, start, minX, minY, minZ, maxX, maxY, maxZ);

            return outside;
        }

        private static void floodFillOutside(List<Cube> outside, List<Cube> inside, Cube start, int minX, int minY, int minZ, int maxX, int maxY, int maxZ)
        {
            // Recursive algorithm will lead to a stack overflow
            Stack<Cube> stack = new Stack<Cube>();
            stack.Push(start);
            while (stack.Count > 0)
            {
                Cube toFill = stack.Pop();
                if (IsValidCube(toFill, outside, inside, minX, minY, minZ, maxX, maxY, maxZ))
                {
                    outside.Add(toFill);
                    stack.Push(new Cube(toFill.Coordinate.X + 1, toFill.Coordinate.Y, toFill.Coordinate.Z));
                    stack.Push(new Cube(toFill.Coordinate.X - 1, toFill.Coordinate.Y, toFill.Coordinate.Z));
                    stack.Push(new Cube(toFill.Coordinate.X, toFill.Coordinate.Y + 1, toFill.Coordinate.Z));
                    stack.Push(new Cube(toFill.Coordinate.X, toFill.Coordinate.Y - 1, toFill.Coordinate.Z));
                    stack.Push(new Cube(toFill.Coordinate.X, toFill.Coordinate.Y, toFill.Coordinate.Z + 1));
                    stack.Push(new Cube(toFill.Coordinate.X, toFill.Coordinate.Y, toFill.Coordinate.Z - 1));
                }
            }
        }

        private static bool IsValidCube(Cube toFill, List<Cube> outside, List<Cube> inside, int minX, int minY, int minZ, int maxX, int maxY, int maxZ)
        {
            bool valid = toFill.Coordinate.X >= minX && toFill.Coordinate.X <= maxX
                            && toFill.Coordinate.Y >= minY && toFill.Coordinate.Y <= maxY
                            && toFill.Coordinate.Z >= minZ && toFill.Coordinate.Z <= maxZ
                            && !outside.Exists(c => c.Equals(toFill))
                            && !inside.Exists(c => c.Equals(toFill));
            return valid;
        }

        private static int GetExteriorSurfaceArea(List<Cube> cubes, List<Cube> outside)
        {
            int surfaceArea = 0;

            foreach (Cube cube in cubes)
            {
                surfaceArea += GetSidesConnected(cube, outside);
            }

            return surfaceArea;
        }

        private static int GetSurfaceArea(List<Cube> cubes)
        {
            int surfaceArea = 0;

            foreach (Cube cube in cubes)
            {
                surfaceArea += GetSideNotConnected(cube, cubes);
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
            return (6 - sidesConnected);
        }

        private static int GetSidesConnected(Cube cube1, List<Cube> cubes)
        {
            int sidesConnected = 0;

            foreach (Cube cube2 in cubes)
            {
                if (cube1.IsAdjacent(cube2))
                {
                    sidesConnected++;
                }
            }
            return sidesConnected;
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