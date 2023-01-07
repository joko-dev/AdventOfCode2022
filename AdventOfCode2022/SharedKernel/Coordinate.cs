using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2022.SharedKernel
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

        public Coordinate(Coordinate coordinate) 
        { 
            this.X = coordinate.X;
            this.Y = coordinate.Y;
        }

        public void Move(int xToMove, int yToMove)
        {
            X += xToMove;
            Y += yToMove;
        }

        public void Move(Coordinate toMove)
        {
            Move(toMove.X, toMove.Y);
        }

        public bool Equals(Coordinate coordinate)
        {
            return (this.X == coordinate.X && this.Y == coordinate.Y);
        }

        public List<Coordinate> GetAdjacentCoordinates()
        {
            List<Coordinate> adjacent = new List<Coordinate>();
            adjacent.Add(new Coordinate(this.X - 1, Y - 1));
            adjacent.Add(new Coordinate(this.X, Y - 1));
            adjacent.Add(new Coordinate(this.X + 1, Y - 1));
            adjacent.Add(new Coordinate(this.X - 1, Y));
            adjacent.Add(new Coordinate(this.X + 1, Y));
            adjacent.Add(new Coordinate(this.X - 1, Y + 1));
            adjacent.Add(new Coordinate(this.X, Y + 1));
            adjacent.Add(new Coordinate(this.X + 1, Y + 1));

            return adjacent;
        }

        public int X { get; private set; }
        public int Y { get; private set; }

        public static explicit operator (int x, int y)(Coordinate obj)
        {
            return (obj.X, obj.Y);
        }
    }
}

