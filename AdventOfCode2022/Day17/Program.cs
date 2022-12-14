using AdventOfCode2022.SharedKernel;
using System.Runtime.CompilerServices;

namespace Day17
{
    internal class Program
    {
        const char FALLING = '@';
        const char AIR = '.';
        const char ROCK = '#';
        const char LEFTJET = '<';
        const char RIGHTJET = '>';
        internal struct RockStructure
        {
            internal List<String> Shape { get; }
            internal RockStructure(List<string> shape)
            {
                Shape = shape;
            }
        }
        static void Main(string[] args)
        {
            Console.WriteLine(PuzzleOutputFormatter.getPuzzleCaption("Day 17: Pyroclastic Flow"));
            Console.WriteLine("Flow rate report: ");
            PuzzleInput puzzleInput = new(PuzzleOutputFormatter.getPuzzleFilePath(), true);

            string jetPattern = puzzleInput.Lines[0];
            List<RockStructure> rocks = CreateRockStructures();

            char[,] tower = SimulateRockTower(rocks, jetPattern, 2022, out int maxHeight );
            Console.WriteLine("Max Height: {0}", maxHeight);

            //Part 2:   there's no part were all elements in 1 row are rocks. I assume there must be some kind of loop, but currently
            //          no idea how to determine. Some kind of "cache tower height for each step, check wether there is a pattern"
        }

        private static char[,] SimulateRockTower(List<RockStructure> rocks, string jetPattern, int rockCount, out int maxHeight)
        {
            char[,] tower = new char[7, rocks.Max(r => r.Shape.Count() * rockCount)];
            PuzzleConverter.fillMatrix(tower, '.');
            int currentJetIndex = 0;
            for(int i = 1; i <= rockCount; i++)
            {
                int rockIndex = ( i % rocks.Count == 0 ) ? rocks.Count - 1 : (i % rocks.Count - 1);

                SpawnRock(tower, rocks[rockIndex]);

                /*for (int y = 10; y >= 0; y--)
                {
                    for (int x = 0; x < tower.GetLength(0); x++)
                    {
                        Console.Write(tower[x, y]);
                    }
                    Console.Write("\n");
                }
                Console.Write("=============\n"); */

                bool bottomReached = false;
                while (!bottomReached)
                {
                    char jet = jetPattern[currentJetIndex];

                    if(jet == LEFTJET)
                    {
                        MoveRockJetLeft(tower, jet);
                    }
                    else
                    {
                        MoveRockJetRight(tower, jet);
                    }

                    bottomReached = MoveRockDown(tower);

                    if (currentJetIndex < jetPattern.Length - 1)
                    {
                        currentJetIndex++;
                    }
                    else
                    {
                        currentJetIndex = 0;
                    }
                }
            }
            maxHeight = GetMaxTowerHeight(tower);
            return tower;
        }

        private static bool MoveRockDown(char[,] tower)
        {
            bool bottomReached = false;
            int maxHeight = GetMaxTowerHeight(tower) + 10;

            for (int y = 0; y < maxHeight && !bottomReached; y++)
            {
                for (int x = 0; x < tower.GetLength(0) && !bottomReached; x++)
                {
                    if (tower[x,y] == FALLING)
                    {
                        if( y > 0)
                        {
                            bottomReached = tower[x,y-1] == ROCK;
                        }
                        else 
                        {
                            bottomReached = true;
                        }
                       
                    }
                }
            }

            if (!bottomReached)
            {
                for (int y = 1; y < maxHeight; y++)
                {
                    for (int x = 0; x < tower.GetLength(0); x++)
                    {
                        if (tower[x,y] == FALLING)
                        {
                            tower[x, y - 1] = FALLING;
                            tower[x, y] = AIR;
                        }
                    }
                }
            }
            else
            {
                for (int y = 0; y < maxHeight; y++)
                {
                    for (int x = 0; x < tower.GetLength(0); x++)
                    {
                        if (tower[x, y] == FALLING)
                        {
                            tower[x, y] = ROCK;
                        }
                    }
                }
            }

            return bottomReached;
        }

        private static void MoveRockJetLeft(char[,] tower, char jet)
        {
            bool movementForbidden = false;
            int maxHeight = GetMaxTowerHeight(tower) + 10;
            for (int y = 0; y < maxHeight && !movementForbidden; y++)
            {
                for (int x = 0; x < tower.GetLength(0) && !movementForbidden; x++)
                {
                    if (tower[x, y] == FALLING)
                    {
                        if (x == 0 || tower[x - 1, y] == ROCK)
                        {
                            movementForbidden = true;
                        }
                    }
                }
            }

            if (!movementForbidden)
            {
                for (int x = 1; x < tower.GetLength(0); x++)
                {
                    for (int y = 0; y < maxHeight; y++)
                    {
                        if (tower[x, y] == FALLING)
                        {
                            tower[x-1, y] = FALLING;
                            tower[x, y] = AIR;
                        }
                    }
                }
            }
        }

        private static void MoveRockJetRight(char[,] tower, char jet)
        {
            bool movementForbidden = false;
            int maxHeight = GetMaxTowerHeight(tower) + 10;
            for (int y = 0; y < maxHeight && !movementForbidden; y++)
            {
                for (int x = 0; x < tower.GetLength(0) && !movementForbidden; x++)
                {
                    if (tower[x, y] == FALLING)
                    {
                        if(x == tower.GetLength(0) - 1 || tower[x + 1, y] == ROCK)
                        {
                            movementForbidden= true;
                        }
                    }
                }
            }

            if (!movementForbidden)
            {
                for (int y = 0; y < maxHeight; y++)
                {
                    for (int x = tower.GetLength(0) - 2; x >= 0; x--)
                    {
                        if (tower[x, y] == FALLING)
                        {
                            tower[x + 1, y] = FALLING;
                            tower[x, y] = AIR;
                        }
                    }
                }
                
            }
        }

        private static void SpawnRock(char[,] tower, RockStructure rockStructure)
        {
            int rockSpawn = GetMaxTowerHeight(tower) + 3;

            for(int y = 0; y < rockStructure.Shape.Count; y++)
            {
                for(int x = 0; x < rockStructure.Shape[y].Length; x++)
                {
                    tower[x + 2, y + rockSpawn] = rockStructure.Shape[y][x];
                    if(tower[x + 2, y + rockSpawn] == ROCK)
                    {
                        tower[x + 2, y + rockSpawn] = FALLING;
                    }
                }
            }
        }

        private static int GetMaxTowerHeight(char[,] tower)
        {
            for (int y = 0; y < tower.GetLength(1); y++)
            {
                bool foundRock = false;
                for(int x = 0; x < tower.GetLength(0); x++)
                {
                    if (tower[x,y] == ROCK)
                    {
                        foundRock = true;
                    }
                }
                if (!foundRock)
                {
                    return y;
                }
            }
            return 0;
        }

        private static List<RockStructure> CreateRockStructures()
        {
            List<RockStructure> rocks= new List<RockStructure>();

            List<string> shape = new List<string>();
            shape.Insert(0, "####");
            rocks.Add(new RockStructure(shape));

            shape = new List<string>();
            shape.Insert(0, ".#.");
            shape.Insert(0, "###");
            shape.Insert(0, ".#.");
            rocks.Add(new RockStructure(shape));

            shape = new List<string>();
            shape.Insert(0, "..#");
            shape.Insert(0, "..#");
            shape.Insert(0, "###");
            rocks.Add(new RockStructure(shape));

            shape = new List<string>();
            shape.Insert(0, "#");
            shape.Insert(0, "#");
            shape.Insert(0, "#");
            shape.Insert(0, "#");
            rocks.Add(new RockStructure(shape));

            shape = new List<string>();
            shape.Insert(0, "##");
            shape.Insert(0, "##");
            rocks.Add(new RockStructure(shape));

            return rocks;
        }
    }
}
