using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2021.SharedKernel
{
    public class Coordinate
    {

        public Coordinate(int X, int Y)
        {
            this.X = X;
            this.Y = Y;
        }

        public Coordinate((int x, int y) point)
        {
            this.X = point.x;
            this.Y = point.y;
        }

        public Coordinate(string coordinate)
        {
            string[] temp = coordinate.Split(',');
            X = int.Parse(temp[0]);
            Y = int.Parse(temp[1]);
        }

        public int X { get; }
        public int Y { get; }

        public static explicit operator (int x, int y)(Coordinate obj)
        {
            return (obj.X, obj.Y);
        }
    }
}
