using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2022.SharedKernel
{
    public static class PuzzleConverter
    {
        // Source: https://github.com/Bpendragon/AdventOfCodeCSharp/blob/9fd66db83f97b3ebce7739444e6d5f294c9a993a/AdventOfCode/Solutions/Utilities.cs#L184
        public static T[,] TrimArray<T>(this T[,] originalArray, int rowToRemove, int columnToRemove)
        {
            T[,] result = new T[originalArray.GetLength(0) - 1, originalArray.GetLength(1) - 1];

            for (int i = 0, j = 0; i < originalArray.GetLength(0); i++)
            {
                if (i == rowToRemove)
                    continue;

                for (int k = 0, u = 0; k < originalArray.GetLength(1); k++)
                {
                    if (k == columnToRemove)
                        continue;

                    result[j, u] = originalArray[i, k];
                    u++;
                }
                j++;
            }

            return result;
        }
        public static T[,] DeleteArrayRowsBeneath<T>(this T[,] originalArray, int rowToDeleteBeneath)
        {
            T[,] result = new T[originalArray.GetLength(0), originalArray.GetLength(1)];

            for (int y = rowToDeleteBeneath; y < originalArray.GetLength(1); y++)
            {
                for(int x = 0; x < originalArray.GetLength(0); x++)
                {
                    result[x, y - rowToDeleteBeneath] = originalArray[x, y];
                }
            }

            return result;
        }
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

        public static char[,] getInputAsMatrixChar(List<string> lines, char? charToFill)
        {
            int height = lines.Count();
            int width = lines.OrderBy(s => s.Length).Last().Length;
            char[,] matrix = new char[width, height];

            for (int h = 0; h < height; h++)
            {
                for (int w = 0; w < width; w++)
                {
                    if(w >= lines[h].Length && charToFill.HasValue)
                    {
                        matrix[w, h] = charToFill.Value;
                    }
                    else
                    {
                        matrix[w, h] = lines[h][w];
                    }
                }
            }

            return matrix;
        }

        public static void fillMatrix<T>(T[,] matrix, T value)
        {
            for(int x = 0; x < matrix.GetLength(0); x++)
            {
                for (int y = 0; y < matrix.GetLength(1); y++)
                {
                    matrix[x,y] = value;
                }
            }
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

        public static List<(int x, int y)> getAdjacentPoints<T>(T[,] matrix, (int x, int y) point, bool horizontal, bool vertical, bool diagonal)
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
