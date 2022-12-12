using AdventOfCode2022.SharedKernel;
using System.Security.Cryptography.X509Certificates;

namespace Day12
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(PuzzleOutputFormatter.getPuzzleCaption("Day 12: Hill Climbing Algorithm"));
            Console.WriteLine("Heightmap: ");
            PuzzleInput puzzleInput = new(PuzzleOutputFormatter.getPuzzleFilePath(), false);

            char[,] map = PuzzleConverter.getInputAsMatrixChar(puzzleInput.Lines);

            List<Coordinate> path = GetShortestPath(map);
            Console.WriteLine("Fewest steps required: {0}", path.Count - 1);
        }

        private static List<Coordinate> GetShortestPath(char[,] map)
        {
            List<Coordinate> coordinates = getAllCoordinates(map);
            Coordinate start = getCoordinate(map, coordinates, 'S');
            Coordinate end = getCoordinate(map, coordinates, 'E');

            Dictionary<Coordinate, Coordinate> predecessors = Dijkstra(map, coordinates, start, end);
            List<Coordinate> path = getShortestPath(end, predecessors);

            return path;
        }

        private static List<Coordinate> getAllCoordinates(char[,] map)
        {
            List<Coordinate> coordinates = new List<Coordinate>();

            for(int x = 0; x < map.GetLength(0); x++)
            {
                for(int y = 0; y < map.GetLength(1); y++)
                {
                    coordinates.Add(new Coordinate(x, y));
                }
            }

            return coordinates;
        }

        private static Coordinate getCoordinate(char[,] map, List<Coordinate> coordinates, char element)
        {
            for (int x = 0; x < map.GetLength(0); x++)
            {
                for (int y = 0; y < map.GetLength(1); y++)
                {
                    if (map[x, y] == element)
                    {
                        return coordinates.Find(c => c.X == x && c.Y == y);
                    }
                }
            }
            throw new ArithmeticException(nameof(map));
        }

        static Dictionary<Coordinate, Coordinate> Dijkstra(char[,] map, List<Coordinate> ways, Coordinate start, Coordinate end)
        {
            Dictionary<Coordinate, int> distance = new Dictionary<Coordinate, int>();
            Dictionary<Coordinate, Coordinate> predecessor = new Dictionary<Coordinate, Coordinate>();
            List<Coordinate> coordinatesWithoutPath = new List<Coordinate>();

            initialize(ways, start, distance, predecessor, coordinatesWithoutPath);
            while (coordinatesWithoutPath.Count() > 0)
            {
                Coordinate lowestDistance = findCoordinateWithLowestDistance(coordinatesWithoutPath, ref distance);
                coordinatesWithoutPath.Remove(lowestDistance);
                if (end is not null && lowestDistance == end)
                {
                    break;
                }
                List<Coordinate> adjacent = getAdjacentCoordinates(map, lowestDistance, coordinatesWithoutPath);
                foreach (Coordinate adj in adjacent)
                {
                    if (coordinatesWithoutPath.Exists(c => c == adj))
                    {
                        distanceUpdate(map, lowestDistance, adj, distance, predecessor);
                    }
                }
            }
            return predecessor;
        }

        private static List<Coordinate> getShortestPath(Coordinate endPoint, Dictionary<Coordinate, Coordinate> predecessors)
        {
            List<Coordinate> path = new List<Coordinate>();
            path.Add(endPoint);
            Coordinate u = endPoint;
            while (predecessors[u] is not null)
            {
                u = predecessors[u];
                path.Insert(0, u);
            }
            return path;
        }

        private static void distanceUpdate(char[,] map, Coordinate from, Coordinate to, Dictionary<Coordinate, int> distance, Dictionary<Coordinate, Coordinate> predecessor)
        {
            //distance from one point to another is always 1
            int alternative = distance[from] + 1;

            if (alternative < distance[to])
            {
                distance[to] = alternative;
                predecessor[to] = from;
            }
        }

        private static List<Coordinate> getAdjacentCoordinates(char[,] map, Coordinate coordinate, List<Coordinate> coordinates)
        {
            List<Coordinate> adjacent = new List<Coordinate>();
            List<(int x, int y)> points = PuzzleConverter.getAdjacentPoints(map, (coordinate.X, coordinate.Y), true, true, false);

            foreach ((int x, int y) point in points)
            {
                if (GetElevation(map[point.x, point.y]) - 1 <= GetElevation(map[coordinate.X, coordinate.Y]))
                {
                    Coordinate adj = coordinates.Find(c => c.X == point.x && c.Y == point.y);
                    if (adj is not null)
                    {
                        adjacent.Add(adj);
                    }
                }
            }

            return adjacent;
        }

        private static char GetElevation(char elevationIn)
        {
            char elevation = elevationIn;
            if(elevation == 'S')
            {
                elevation = 'a';
            }
            else if (elevation == 'E')
            {
                elevation = 'z';
            }

            return elevation;
        }

        private static Coordinate findCoordinateWithLowestDistance(List<Coordinate> coordinatesWithoutPath, ref Dictionary<Coordinate, int> distance)
        {
            Coordinate coordinateLowestDistance = null;
            distance = distance.OrderBy(d => d.Value).ToDictionary(d => d.Key, d => d.Value);
            foreach (KeyValuePair<Coordinate, int> coordinate in distance)
            {
                if (coordinatesWithoutPath.Exists(c => c == coordinate.Key))
                {
                    coordinateLowestDistance = coordinate.Key;
                    break;
                }

            }

            return coordinateLowestDistance;
        }

        static void initialize(List<Coordinate> ways, Coordinate start, Dictionary<Coordinate, int> distance, Dictionary<Coordinate, Coordinate> predecessor, List<Coordinate> coordinatesWithoutPath)
        {
            distance.Clear();
            predecessor.Clear();
            coordinatesWithoutPath.Clear();

            foreach (Coordinate coordinate in ways)
            {
                distance.Add(coordinate, int.MaxValue);
                predecessor.Add(coordinate, null);
                coordinatesWithoutPath.Add(coordinate);
            }

            distance[start] = 0;
        }
    }
}