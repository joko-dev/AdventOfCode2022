using AdventOfCode2022.SharedKernel;
using System.Net;

namespace Day02
{
    internal class Program
    {
        enum Shape
        {
            Rock, Paper, Scissor
        }

        static void Main(string[] args)
        {
            Console.WriteLine(PuzzleOutputFormatter.getPuzzleCaption("Day 02: Rock Paper Scissors"));
            Console.WriteLine("Strategy guide: ");
            PuzzleInput puzzleInput = new(PuzzleOutputFormatter.getPuzzleFilePath(), false);

            Console.WriteLine("Score for guide: {0}", GetScore(puzzleInput));
            Console.WriteLine("Score for ultra top secret guide: {0}", GetScoreTopSecret(puzzleInput));
        }

        private static int GetScore(PuzzleInput puzzleInput)
        {
            int score = 0;

            foreach(string line in puzzleInput.Lines) 
            {
                Shape opponent = GetShape(line[0]);
                Shape self = GetShape(line[2]);

                score += GetResponseScore(self);
                score += GetOutcomeScore(self, opponent);
            }

            return score;
        }

        private static int GetScoreTopSecret(PuzzleInput puzzleInput)
        {
            int score = 0;

            foreach (string line in puzzleInput.Lines)
            {
                Shape opponent = GetShape(line[0]);
                Shape self = GetShapeForResult(opponent, line[2]);

                score += GetResponseScore(self);
                score += GetOutcomeScore(self, opponent);
            }

            return score;
        }

        private static Shape GetShapeForResult(Shape opponent, char result)
        {
            Shape self;

            if(result == 'Y')
            {
                self = opponent;
            }
            else if (result == 'Z' )
            {
                self = opponent switch
                {
                    Shape.Rock => Shape.Paper,
                    Shape.Paper => Shape.Scissor,
                    Shape.Scissor => Shape.Rock,
                    _ => throw new ArgumentOutOfRangeException(nameof(opponent))
                };
            }
            else if (result == 'X')
            {
                self = opponent switch
                {
                    Shape.Rock => Shape.Scissor,
                    Shape.Paper => Shape.Rock,
                    Shape.Scissor => Shape.Paper,
                    _ => throw new ArgumentOutOfRangeException(nameof(opponent))
                };
            }
            else
            {
                throw new ArgumentOutOfRangeException(nameof(result));
            }

            return self;
        }

        private static int GetOutcomeScore(Shape self, Shape opponent)
        {
            int score;

            if(self == opponent)
            {
                score = 3;
            }
            else if((self == Shape.Rock && opponent == Shape.Scissor) 
                    || (self == Shape.Paper && opponent == Shape.Rock)
                    || (self == Shape.Scissor && opponent == Shape.Paper))
            {
                score = 6;
            }
            else
            {
                score = 0;
            }

            return score;
        }

        private static int GetResponseScore(Shape self)
        {
            int score = self switch
            {
                Shape.Rock => 1,
                Shape.Paper => 2,
                Shape.Scissor => 3,
                _ => throw new ArgumentOutOfRangeException(nameof(self)),
            };
            return score;
        }

        private static Shape GetShape(char shape)
        {
            Shape response = shape switch
            {
                'A' or 'X' => Shape.Rock,
                'B' or 'Y' => Shape.Paper,
                'C' or 'Z' => Shape.Scissor,
                _ => throw new ArgumentOutOfRangeException(nameof(shape)),
            };
            return response;
        }
    }
}