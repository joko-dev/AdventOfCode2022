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

    }
}
