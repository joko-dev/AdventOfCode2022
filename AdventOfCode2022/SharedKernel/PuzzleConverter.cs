﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2021.SharedKernel
{
    public static class PuzzleConverter
    {
        public static int[,] getInputAsMatrixInt(List<string> lines)
        {
            int height = lines.Count();
            int width = lines.OrderBy(s => s.Length).Last().Length;
            int[,] matrix = new int[width, height];

            for (int h = 0; h < height; h++)
            {
                for (int w = 0; w < width; w++)
                {
                    matrix[w, h] = (int)char.GetNumericValue(lines[h][w]);
                }
            }

            return matrix;
        }

        public static int[,] getInputCoordinateAsMatrix(List<string> lines, int coordinateValue, string separator)
        {
            List<(int x, int y)> coordinates = new List<(int x, int y)>();

            foreach(string line in lines)
            {
                string[] temp = line.Split(separator);
                coordinates.Add((int.Parse(temp[0]), int.Parse(temp[1])));
            }

            int width = coordinates.Select(c => c.x).Max() + 1;
            int height = coordinates.Select(c => c.y).Max() + 1;

            int[,] matrix = new int[width, height];

            foreach((int x, int y) coordinate in coordinates)
            {
                matrix[coordinate.x, coordinate.y] = coordinateValue;
            }

            return matrix;
        }

        public static List<(int x, int y)> getAdjacentPoints(int[,] matrix, (int x, int y) point, bool horizontal, bool vertical, bool diagonal)
        {
            List<(int x, int y)> adjacent = new List<(int x, int y)>();

            if (horizontal && (point.x > 0)) { adjacent.Add((point.x - 1, point.y)); }
            if (horizontal && (point.x < matrix.GetLength(0) - 1)) { adjacent.Add((point.x + 1, point.y)); }
            if (vertical && (point.y > 0)) { adjacent.Add((point.x, point.y - 1)); }
            if (vertical && (point.y < matrix.GetLength(1) - 1)) { adjacent.Add((point.x, point.y + 1)); }

            if (diagonal && (point.x > 0) && (point.y > 0)) { adjacent.Add((point.x - 1, point.y - 1)); }
            if (diagonal && (point.x < matrix.GetLength(0) - 1) && (point.y > 0)) { adjacent.Add((point.x + 1, point.y - 1)); }
            if (diagonal && (point.x > 0) && (point.y < matrix.GetLength(1) - 1)) { adjacent.Add((point.x - 1, point.y + 1)); }
            if (diagonal && (point.x < matrix.GetLength(0) - 1) && (point.y < matrix.GetLength(1) - 1)) { adjacent.Add((point.x + 1, point.y + 1)); }

            return adjacent;
        }
    }
}
