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
            internal List<List<Coordinate3D>> GetPlanes()
            {
                List<List<Coordinate3D>> planes = new List<List<Coordinate3D>>();

                List<Coordinate3D> plane = new List<Coordinate3D>();
                plane.Add(new Coordinate3D(Coordinate.X, Coordinate.Y, Coordinate.Z));
                plane.Add(new Coordinate3D(Coordinate.X + 1, Coordinate.Y, Coordinate.Z));
                plane.Add(new Coordinate3D(Coordinate.X + 1, Coordinate.Y, Coordinate.Z + 1));
                plane.Add(new Coordinate3D(Coordinate.X, Coordinate.Y, Coordinate.Z + 1));
                planes.Add(plane);

                plane = new List<Coordinate3D>();
                plane.Add(new Coordinate3D(Coordinate.X, Coordinate.Y + 1, Coordinate.Z));
                plane.Add(new Coordinate3D(Coordinate.X + 1, Coordinate.Y + 1, Coordinate.Z));
                plane.Add(new Coordinate3D(Coordinate.X + 1, Coordinate.Y + 1, Coordinate.Z + 1));
                plane.Add(new Coordinate3D(Coordinate.X, Coordinate.Y + 1, Coordinate.Z + 1));
                planes.Add(plane);

                plane = new List<Coordinate3D>();
                plane.Add(new Coordinate3D(Coordinate.X, Coordinate.Y, Coordinate.Z));
                plane.Add(new Coordinate3D(Coordinate.X, Coordinate.Y, Coordinate.Z + 1));
                plane.Add(new Coordinate3D(Coordinate.X, Coordinate.Y + 1, Coordinate.Z + 1));
                plane.Add(new Coordinate3D(Coordinate.X, Coordinate.Y + 1, Coordinate.Z));
                planes.Add(plane);

                plane = new List<Coordinate3D>();
                plane.Add(new Coordinate3D(Coordinate.X + 1, Coordinate.Y, Coordinate.Z));
                plane.Add(new Coordinate3D(Coordinate.X + 1, Coordinate.Y, Coordinate.Z + 1));
                plane.Add(new Coordinate3D(Coordinate.X + 1, Coordinate.Y + 1, Coordinate.Z + 1));
                plane.Add(new Coordinate3D(Coordinate.X + 1, Coordinate.Y + 1, Coordinate.Z));
                planes.Add(plane);

                plane = new List<Coordinate3D>();
                plane.Add(new Coordinate3D(Coordinate.X, Coordinate.Y, Coordinate.Z));
                plane.Add(new Coordinate3D(Coordinate.X + 1, Coordinate.Y, Coordinate.Z));
                plane.Add(new Coordinate3D(Coordinate.X + 1, Coordinate.Y + 1, Coordinate.Z));
                plane.Add(new Coordinate3D(Coordinate.X, Coordinate.Y + 1, Coordinate.Z));
                planes.Add(plane);

                plane = new List<Coordinate3D>();
                plane.Add(new Coordinate3D(Coordinate.X, Coordinate.Y, Coordinate.Z + 1));
                plane.Add(new Coordinate3D(Coordinate.X + 1, Coordinate.Y, Coordinate.Z + 1));
                plane.Add(new Coordinate3D(Coordinate.X + 1, Coordinate.Y + 1, Coordinate.Z + 1));
                plane.Add(new Coordinate3D(Coordinate.X, Coordinate.Y + 1, Coordinate.Z + 1));
                planes.Add(plane);

                return planes;
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
            //Idea: get all planes of a cube. Compare them to all other planes of all other cubes. If all points of one plane equals all points of another, 
            //      the cubes are touching. 
            int surfaceArea = 0;

            foreach (Cube cube1 in cubes)
            {
                surfaceArea += GetSideNotConnected(cube1, cubes);
            }

            return surfaceArea;
        }

        private static int GetSideNotConnected(Cube cube1, List<Cube> cubes)
        {
            int sidesNotConnected = 0;
            foreach (List<Coordinate3D> plane1 in cube1.GetPlanes())
            {
                bool touching = false;
                foreach (Cube cube2 in cubes.Where(c => c != cube1))
                {
                    foreach (List<Coordinate3D> plane2 in cube2.GetPlanes())
                    {
                        if(ArePlanesEqual(plane1, plane2))
                        {
                            touching = true;
                            break;
                        }
                    }
                    if (touching)
                    {
                        break;
                    }
                }
                if (!touching)
                {
                    sidesNotConnected++;
                }
            }
            return sidesNotConnected;
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