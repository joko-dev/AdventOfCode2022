using AdventOfCode2022.SharedKernel;
using System.Net;

namespace Day02
{
    internal class Program
    {
        enum Response
        {
            Rock, Paper, Scissor
        }

        static void Main(string[] args)
        {
            Console.WriteLine(PuzzleOutputFormatter.getPuzzleCaption("Day 02: Rock Paper Scissors"));
            Console.WriteLine("Strategy guide: ");
            PuzzleInput puzzleInput = new(PuzzleOutputFormatter.getPuzzleFilePath(), false);

            Console.WriteLine("Score for guide: {0}", GetScore(puzzleInput));
        }

        private static int GetScore(PuzzleInput puzzleInput)
        {
            int score = 0;

            foreach(string line in puzzleInput.Lines) 
            {
                Response opponent = GetResponse(line[0]);
                Response self = GetResponse(line[2]);

                score += GetResponseScore(self);
                score += GetOutcomeScore(self, opponent);
            }

            return score;
        }

        private static int GetOutcomeScore(Response self, Response opponent)
        {
            int score;

            if(self == opponent)
            {
                score = 3;
            }
            else if((self == Response.Rock && opponent == Response.Scissor) 
                    || (self == Response.Paper && opponent == Response.Rock)
                    || (self == Response.Scissor && opponent == Response.Paper))
            {
                score = 6;
            }
            else
            {
                score = 0;
            }

            return score;
        }

        private static int GetResponseScore(Response self)
        {
            int score = self switch
            {
                Response.Rock => 1,
                Response.Paper => 2,
                Response.Scissor => 3,
                _ => throw new ArgumentOutOfRangeException(nameof(self)),
            };
            return score;
        }

        private static Response GetResponse(char shape)
        {
            Response response;
            switch (shape)
            {
                case 'A':
                case 'X': response = Response.Rock; break;
                case 'B':
                case 'Y': response = Response.Paper; break;
                case 'C':
                case 'Z': response = Response.Scissor; break;
                default: throw new ArgumentOutOfRangeException(nameof(shape));
            }

            return response;
        }
    }
}