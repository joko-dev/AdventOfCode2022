using AdventOfCode2022.SharedKernel;
using System.Collections.Generic;
using System.Numerics;

namespace Day15
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(PuzzleOutputFormatter.getPuzzleCaption("Day 15: Beacon Exclusion Zone"));
            Console.WriteLine("Sensors and beacons: ");
            PuzzleInput puzzleInput = new(PuzzleOutputFormatter.getPuzzleFilePath(), true);

            Console.Write("y to check: ");
            int yToCheck = Int32.Parse(Console.ReadLine());
            List<(Coordinate sensor, Coordinate beacon)> sensors = GetSensors(puzzleInput.Lines);

            Console.WriteLine("Positions without beacons: {0}", CountPositionsWithoutBeacons(sensors, yToCheck));

            Console.WriteLine("=======");

            Console.Write("max coordinate value: ");
            int maxCoordinateValue = Int32.Parse(Console.ReadLine());
            Coordinate distressBeacon = GetDistressBeacon(sensors, 0, maxCoordinateValue);
            Int64 tuningFrequency = Math.BigMul(distressBeacon.X, 4000000) + distressBeacon.Y;
            
            Console.WriteLine("Tuning frequency: {0}", tuningFrequency);
            
        }

        private static Coordinate GetDistressBeacon(List<(Coordinate sensor, Coordinate beacon)> sensors, int minCoordinateValue, int maxCoordinateValue)
        {
            // Approach: only one solution. So it must be a point, which is exactly 1 distance outside a sensors area of influence. For all sensors we determine all
            // points around their area of influence and check them against all other points. If one is outside all other points we have found our distress beacon
            
            List<Coordinate> distress = new List<Coordinate>();

            foreach ((Coordinate sensor, Coordinate beacon) sensor in sensors)
            {
                List<Coordinate> possibleDistressBeacons = GetPossibleDistressBeacons(sensor, minCoordinateValue, maxCoordinateValue);
                
                foreach (Coordinate beacon in possibleDistressBeacons )
                {
                    bool overlapping = false;
                    foreach ((Coordinate sensor, Coordinate beacon) otherSensor in sensors)
                    {
                        if(ManhattanDistance(otherSensor.sensor, beacon) <= ManhattanDistance(otherSensor.sensor, otherSensor.beacon))
                        {
                            overlapping = true;
                            break;
                        }
                    }
                    if (!overlapping)
                    {
                        return beacon;
                    }
                }
            }

            throw new Exception();
        }

        private static List<Coordinate> GetPossibleDistressBeacons((Coordinate sensor, Coordinate beacon) sensor, int minCoordinateValue, int maxCoordinateValue)
        {
            List<Coordinate> possibleDistressBeacons = new List<Coordinate>();
            int manhattenDistance = ManhattanDistance(sensor.sensor, sensor.beacon) + 1;

            for(int x = sensor.sensor.X - manhattenDistance; x <= sensor.sensor.X + manhattenDistance; x++)
            {
                if(x >= minCoordinateValue && x <= maxCoordinateValue)
                {
                    int yDistance = manhattenDistance - Math.Abs(x - sensor.sensor.X);
                    int yUp = sensor.sensor.Y + yDistance;
                    int yDown = sensor.sensor.Y - yDistance;
                    if (yUp >= minCoordinateValue && yUp <= maxCoordinateValue)
                    {
                        possibleDistressBeacons.Add(new Coordinate(x, yUp));
                    }
                    if (yDown >= minCoordinateValue && yDown <= maxCoordinateValue)
                    {
                        possibleDistressBeacons.Add(new Coordinate(x, yDown));
                    }

                }
                
            }

            return possibleDistressBeacons;
        }

        private static int CountPositionsWithoutBeacons(List<(Coordinate sensor, Coordinate beacon)> sensors, int y)
        {
            int count = 0;
            (Coordinate sensor, Coordinate beacon) leftMostSensor = sensors.OrderBy(s => s.sensor.X).First();
            (Coordinate sensor, Coordinate beacon) rightMostSensor = sensors.OrderBy(s => s.sensor.X).Last();
            int maxManhatten = sensors.Select(m => ManhattanDistance(m.sensor, m.beacon)).ToList().OrderByDescending(m=>m).First();
            int minX = leftMostSensor.sensor.X - maxManhatten;
            int maxX = rightMostSensor.sensor.X + maxManhatten;

            for (int x = minX; x <= maxX; x++)
            {
                bool found = false;
                Coordinate possibleBeacon = new Coordinate(x, y);

                foreach ((Coordinate sensor, Coordinate beacon) sensor in sensors)
                {
                    if (!sensor.beacon.Equals(possibleBeacon) && ManhattanDistance(sensor.sensor, possibleBeacon) <= ManhattanDistance(sensor.sensor, sensor.beacon))
                    {
                        count++;
                        found = true;
                        break;
                    }
                }

            }

            return count;
        }

        private static List<(Coordinate sensor, Coordinate beacon)> GetSensors(List<string> lines)
        {
            List < (Coordinate sensor, Coordinate beacon) > sensors = new List<(Coordinate sensor, Coordinate beacon)> ();
            foreach (string line in lines)
            {
                string[] coordinates = line.Replace("Sensor at", "").Replace("closest beacon is at", "").Replace("x=","").Replace("y=", "").Split(":");
                sensors.Add((new Coordinate(coordinates[0]), new Coordinate(coordinates[1])));
            }
            return sensors;
        }

        private static int ManhattanDistance(Coordinate point1, Coordinate point2)
        {
            return Math.Abs(point1.X - point2.X) + Math.Abs(point1.Y - point2.Y);
        }
    }
}