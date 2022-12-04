using AdventOfCode2022.SharedKernel;
using System.Reflection.Metadata.Ecma335;

namespace Day04
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(PuzzleOutputFormatter.getPuzzleCaption("Day 04: Camp Cleanup"));
            Console.WriteLine("Assignment pairs: ");
            PuzzleInput puzzleInput = new(PuzzleOutputFormatter.getPuzzleFilePath(), false);

            Console.WriteLine("Count assignments containing others: {0}", GetAssignmentsContainingOthers(puzzleInput, false));
            Console.WriteLine("Count assignments partially containing others: {0}", GetAssignmentsContainingOthers(puzzleInput, true));
        }

        private static int GetAssignmentsContainingOthers(PuzzleInput puzzleInput, bool partiallyOverlapping)
        {
            int count = 0;

            foreach(string line in puzzleInput.Lines)
            {
                string[] assignments = line.Split(',');
                if (IsAssignmentContainingOther(GetAssignment(assignments[0]), GetAssignment(assignments[1]), partiallyOverlapping ))
                {
                    count++;
                }
            }

            return count;
        }

        private static (int firstSection, int lastSection) GetAssignment(string assignmentString)
        {
            (int firstSection, int lastSection) assignment;

            string[] temp = assignmentString.Split('-');
            assignment.firstSection = Int32.Parse(temp[0]);
            assignment.lastSection = Int32.Parse(temp[1]);

            return assignment;
        }

        private static bool IsAssignmentContainingOther((int firstSection, int lastSection) assignment1, (int firstSection, int lastSection) assignment2, bool partiallyOverlapping)
        {
            bool containing = false;

            if (partiallyOverlapping)
            {
                containing = (assignment1.firstSection <= assignment2.lastSection && assignment1.lastSection >= assignment2.firstSection);
                containing = containing || (assignment2.firstSection <= assignment1.lastSection && assignment2.lastSection >= assignment1.firstSection);
            }
            else
            {
                containing = (assignment1.firstSection <= assignment2.firstSection && assignment1.lastSection >= assignment2.lastSection);
                containing = containing || (assignment2.firstSection <= assignment1.firstSection && assignment2.lastSection >= assignment1.lastSection);
            }
            

            return containing;
        }
    }
}