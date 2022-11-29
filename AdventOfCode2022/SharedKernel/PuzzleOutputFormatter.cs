using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2021.SharedKernel
{
    public static class PuzzleOutputFormatter
    {
        public static string getPuzzleCaption(string caption)
        {
            StringBuilder sb = new StringBuilder() ;
            sb.AppendLine(caption);
            sb.AppendLine(new string('=', caption.Length));

            return sb.ToString();
        }

    }
}
