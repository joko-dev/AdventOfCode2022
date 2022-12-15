using AdventOfCode2022.SharedKernel;
using System.Collections.Generic;

namespace Day15
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(PuzzleOutputFormatter.getPuzzleCaption("Day 15: Beacon Exclusion Zone"));
            Console.WriteLine("Sensors and beacons: ");
            PuzzleInput puzzleInput = new(PuzzleOutputFormatter.getPuzzleFilePath(), true);

            List<(Coordinate sensor, Coordinate beacon)> sensors = GetSensors(puzzleInput.Lines);

            Console.WriteLine("Positions without beacons (y=10): {0}", CountPositionsWithoutBeacons(sensors, 10));
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

                Coordinate possibleBeacon = new Coordinate(x, y);
                (Coordinate sensor, Coordinate beacon) nearestSensor = GetNearestSensor(sensors, possibleBeacon);

                if(ManhattanDistance(possibleBeacon, nearestSensor.sensor) <= ManhattanDistance(nearestSensor.sensor, nearestSensor.beacon))
                {
                    count++;
                    Console.Write('#');
                }
                else { Console.Write('.');  }
            }

            return count;
        }

        private static (Coordinate sensor, Coordinate beacon) GetNearestSensor(List<(Coordinate sensor, Coordinate beacon)> sensors, Coordinate possibleBeacon)
        {
            (Coordinate sensor, Coordinate beacon) nearestSensor = sensors.First();
            foreach ((Coordinate sensor, Coordinate beacon) sensor in sensors.Skip(1))
            {
                if(ManhattanDistance(sensor.sensor, possibleBeacon) < ManhattanDistance(nearestSensor.sensor, possibleBeacon))
                {
                    nearestSensor = sensor;
                }
            }
            return nearestSensor;
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