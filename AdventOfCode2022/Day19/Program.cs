using AdventOfCode2022.SharedKernel;
using System.Text;

namespace Day19
{
    internal class Program
    {
        enum Build
        {
            None, OreRobot, ClayRobot, ObsidianRobot, GeodeRobot
        }
        class Blueprint
        {
            public int ID { get;  }
            public int OreRobotCostOre {get;}
            public int ClayRobotCostOre { get; }
            public int ObsidianRobotCostOre { get; }
            public int ObsidianRobotCostClay { get; }
            public int GeodeRobotCostOre { get; }
            public int GeodeRobotCostObsidian { get; }
            public Blueprint(string blueprint)
            {
                ID = Int32.Parse(blueprint.Split(":")[0].Replace("Blueprint",""));
                string[] robots = blueprint.Split(":")[1].Split(".");

                OreRobotCostOre = Int32.Parse(robots[0].Replace("Each ore robot costs ","").Replace("ore", ""));
                ClayRobotCostOre = Int32.Parse(robots[1].Replace("Each clay robot costs ", "").Replace("ore", ""));
                ObsidianRobotCostOre = Int32.Parse(robots[2].Split("and")[0].Replace("Each obsidian robot costs ", "").Replace("ore", ""));
                ObsidianRobotCostClay = Int32.Parse(robots[2].Split("and")[1].Replace("clay", ""));
                GeodeRobotCostOre = Int32.Parse(robots[3].Split("and")[0].Replace("Each geode robot costs ", "").Replace("ore", ""));
                GeodeRobotCostObsidian = Int32.Parse(robots[3].Split("and")[1].Replace("obsidian", ""));
            }

        }

        class GeodeSimulation
        {
            public Blueprint Blueprint { get; }
            public int OreRobots { get; private set; }
            public int ClayRobots { get; private set; }
            public int ObsidianRobots { get; private set; }
            public int GeodeRobots { get; private set; }
            public int Ore { get; private set; }
            public int Clay { get; private set; }
            public int Obsidian { get; private set; }
            public int Geodes { get; private set; }
            public int CurrentMinute { get; private set; }
            public int MaxMinute { get; private set; }
            private Build currentBuild;
            
            public GeodeSimulation (Blueprint blueprint, int maxMinute)
            {
                Blueprint = blueprint;
                OreRobots = 1;
                ClayRobots = 0;
                ObsidianRobots = 0;
                GeodeRobots = 0;
                Ore = 0;
                Clay = 0;
                Obsidian = 0;
                Geodes = 0;
                CurrentMinute= 0;
                MaxMinute= maxMinute;
            }
            public void Simulate()
            {
                BuildRobot(determineBuild());
                Harvest();
                finishBuild();
                CurrentMinute++;
            }

            private Build determineBuild()
            {
                Build build = Build.None;

                if(Obsidian>= Blueprint.GeodeRobotCostObsidian && Ore >= Blueprint.GeodeRobotCostOre)
                {
                    build = Build.GeodeRobot;
                }
                else if (Clay >= Blueprint.ObsidianRobotCostClay && Ore >= Blueprint.ObsidianRobotCostOre)
                {
                    build = Build.ObsidianRobot;
                }
                else if (Ore >= Blueprint.ClayRobotCostOre && (Clay + ClayRobots < Blueprint.ObsidianRobotCostClay)
                        && (Ore + OreRobots - Blueprint.ClayRobotCostOre > )
                {
                    build = Build.ClayRobot;
                }
                else if (Ore >= Blueprint.OreRobotCostOre)
                {
                    build = Build.OreRobot;
                }

                return build;
            }

            private void BuildRobot(Build toBuild)
            {
                currentBuild = toBuild;
                switch(currentBuild)
                {
                    case Build.OreRobot: Ore -= Blueprint.OreRobotCostOre; break;
                    case Build.ClayRobot: Ore -= Blueprint.ClayRobotCostOre; break;
                    case Build.ObsidianRobot: Ore -= Blueprint.ObsidianRobotCostOre; Clay -= Blueprint.ObsidianRobotCostClay; break;
                    case Build.GeodeRobot: Ore -= Blueprint.GeodeRobotCostOre; Obsidian -= Blueprint.GeodeRobotCostObsidian; break;
                }
            }

            private void Harvest()
            {
                Ore += OreRobots;
                Clay += ClayRobots;
                Obsidian += ObsidianRobots;
                Geodes += GeodeRobots;
            }
            private void finishBuild()
            {
                switch (currentBuild)
                {
                    case Build.OreRobot: OreRobots++; break;
                    case Build.ClayRobot: ClayRobots++; break; 
                    case Build.ObsidianRobot: ObsidianRobots++; break;
                    case Build.GeodeRobot: GeodeRobots++; break;
                }
                currentBuild = Build.None;
            }

            public override string ToString()
            {
                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder.AppendLine(String.Format("Minute {0}", CurrentMinute));
                stringBuilder.AppendLine(String.Format("Ore {0}, Clay {1}, Obisidian {2}, Geode {3}", Ore, Clay, Obsidian, Geodes));
                stringBuilder.AppendLine(String.Format("OreRobot {0}, ClayRobot {1}, ObisidianRobot {2}, GeodeRobot {3}", OreRobots, ClayRobots, ObsidianRobots, GeodeRobots));
                return stringBuilder.ToString();
            }
        }

        static void Main(string[] args)
        {
            Console.WriteLine(PuzzleOutputFormatter.getPuzzleCaption("Day 19: Not Enough Minerals"));
            Console.WriteLine("Blueprints: ");
            PuzzleInput puzzleInput = new(PuzzleOutputFormatter.getPuzzleFilePath(), true);

            List<Blueprint> blueprints = GetBlueprints(puzzleInput.Lines);
            List<int> qualityLevels = GetQualityLevels(blueprints);
            Console.WriteLine("Quality Levels: {0}", qualityLevels.Sum());
        }

        private static List<int> GetQualityLevels(List<Blueprint> blueprints)
        {
            //Ressource-priority is obsidian > clay > ore.
            List<int> levels = new List<int>();

            foreach(Blueprint blueprint in blueprints)
            {
                levels.Add(SimulateBlueprint(blueprint));
            }
            
            return levels;
        }

        private static int SimulateBlueprint(Blueprint blueprint)
        {
            int qualityLevel = 0;
            GeodeSimulation simulation = new GeodeSimulation(blueprint, 24);

            while(simulation.CurrentMinute < simulation.MaxMinute)
            {
                simulation.Simulate();
                Console.WriteLine(simulation.ToString());
            }

            qualityLevel = simulation.Blueprint.ID * simulation.Geodes;
            return qualityLevel;
        }

        private static List<Blueprint> GetBlueprints(List<string> lines)
        {
            List<Blueprint> blueprints = new();

            foreach (string line in lines)
            {
                blueprints.Add(new Blueprint(line));
            }

            return blueprints;
        }
    }
}
