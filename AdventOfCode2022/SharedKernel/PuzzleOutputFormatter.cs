using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2022.SharedKernel
{
    public static class PuzzleOutputFormatter
    {
        public static string getPuzzleCaption(string caption)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(caption);
            sb.AppendLine(new string('=', caption.Length));

            return sb.ToString();
        }

        public static string getPuzzleFilePath()
        {
            string filePath = "";
            string input = "";
            int inputValue = 0;

            Console.WriteLine("(1) Input.txt (2) Sample.txt (other) <<custom file path>>");
            input =  Console.ReadLine();

            if (Int32.TryParse(input, out inputValue ))
            {
                if(inputValue == 1)
                {
                    filePath = "Input.txt";
                }
                else if (inputValue == 2)
                {
                    filePath = "Sample.txt";
                }
            }
            else
            {
                filePath = input;
            }

            return filePath;
        }

        public static List<string> outputMap<T>(T[,] map)
        {
            List<string> output = new List<string>();
            for (int y=0; y < map.GetLength(1); y++)
            {
                StringBuilder stringBuilder = new StringBuilder();
                for (int x = 0; x < map.GetLength(0); x++)
                {
                    stringBuilder.Append(Convert.ToString(map[x, y]));
                }
                output.Add(stringBuilder.ToString());
            }
            return output;
        }
    }
}
