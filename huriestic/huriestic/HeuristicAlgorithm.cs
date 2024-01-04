using System;
namespace huriestic
{
    public class HeuristicAlgorithm
    {
        private List<BrickGroup> brickGroups;
        private Lorry[] lorries;
        private Random random;
        private int iterationLimit;
        private int restartLimit;
        private int lorriesCount;
        private Dictionary<string, double> bestFitnesses;

        public Dictionary<string, double> BestFitnesses
        {
            get { return bestFitnesses; }
        }

        public HeuristicAlgorithm(List<BrickGroup> BrickGroups, int IterationLimit, int RestartLimit, int LorriesCount)
        {
            brickGroups = BrickGroups;
            iterationLimit = IterationLimit;
            restartLimit = RestartLimit;
            random = new Random();
            bestFitnesses = new Dictionary<string, double>();
            lorriesCount = LorriesCount;
            InitializeLorries();
        }

        private void InitializeLorries()
        {
            lorries = new Lorry[3];
            for (int i = 0; i < lorries.Length; i++)
            {
                lorries[i] = new Lorry();
            }
        }

        private void RandomlyDistributeBricks()
        {
            foreach (var brickGroup in brickGroups)
            {
                int lorryIndex = random.Next(lorries.Length);
                lorries[lorryIndex].AddBrickGroup(brickGroup);
            }
        }

        public void RunAlgorithm()
        {
            double bestFitness;
            for (int restart = 0; restart < restartLimit; restart++)
            {
                ClearLorries();
                RandomlyDistributeBricks();
                bestFitness = CalculateFitness(lorries);

                for (int iteration = 0; iteration < iterationLimit; iteration++)
                {
                    bestFitnesses.Add(iteration + " ", bestFitness);

                    //Console.WriteLine(iteration+1 + " " + bestFitnesses[iteration+1]);


                    var (sourceLorry, destinationLorry, brickGroup) = MakeSmallChange(lorries);
                    double newFitness = CalculateFitness(lorries);

                    if (newFitness < bestFitness)
                    {
                        bestFitness = newFitness;
                    }
                    else
                    {
                        sourceLorry.AddBrickGroup(brickGroup);
                        destinationLorry.RemoveBrickGroup(brickGroup);
                    }
                }
            }

            DisplayResults();
        }

        private void ClearLorries()
        {
            foreach (var lorry in lorries)
            {
                lorry.BrickGroups.Clear();
            }
        }


        // I learned that C# can return multiple this in decemeber while playing around with stackoverlow: https://stackoverflow.com/questions/748062/return-multiple-values-to-a-method-caller
        public (Lorry, Lorry, BrickGroup) MakeSmallChange(Lorry[] _lorries) // made it publlic for the testing proejct to access.
        {
            Lorry sourceLorry;
            int sourceIndex;
            do
            {
                sourceIndex = random.Next(_lorries.Length);
                sourceLorry = _lorries[sourceIndex];
            } while (sourceLorry.BrickGroups.Count == 0);

            int destinationIndex;
            do
            {
                destinationIndex = random.Next(_lorries.Length);
            } while (destinationIndex == sourceIndex);

            var destinationLorry = _lorries[destinationIndex];

            var brickGroup = sourceLorry.BrickGroups[random.Next(sourceLorry.BrickGroups.Count)];
            sourceLorry.RemoveBrickGroup(brickGroup);
            destinationLorry.AddBrickGroup(brickGroup);

            return (sourceLorry, destinationLorry, brickGroup);
        }


        public double CalculateFitness(Lorry[] _lorries) // made it publlic for the testing proejct to access.
        {
            if (_lorries.Length == 0) return 0;

            double maxWeight = _lorries.Max(lorry => lorry.TotalWeight);
            double minWeight = _lorries.Min(lorry => lorry.TotalWeight);

            return maxWeight - minWeight;
        }


        // n * m + o + p (n is lorries, m is bricks,
        // o is bricks print, p is lorries print)
        // O(n*m) leave out the less significant.
        private void DisplayResults()
        {
            char[] lorryname = { 'A', 'B', 'C' };
            int[,] grid = new int[lorries.Length, brickGroups.Count];
            for (int i = 0; i < lorries.Length; i++)
            {
                foreach (var brickGroup in lorries[i].BrickGroups)
                {
                    int brickGroupIndex = brickGroups.IndexOf(brickGroup);
                    grid[i, brickGroupIndex] = 1;
                }
            }

            Console.Write("\nLorry/Group\t");
            for (int i = 0; i < brickGroups.Count; i++)
            {
                Console.Write($"{i + 1}\t");
            }
            Console.WriteLine();

            for (int i = 0; i < lorries.Length; i++)
            {
                Console.Write($"Lorry {lorryname[i]}\t\t");
                for (int j = 0; j < brickGroups.Count; j++)
                {
                    Console.Write($"{grid[i, j]}\t");
                }
                Console.WriteLine();
            }

            Console.WriteLine();
            for (var i = 0; i < lorries.Length; i++)
            {

                Console.WriteLine($"\nLorry {lorryname[i]} (Total Weight: {lorries[i].TotalWeight}) - Brick Weights: {string.Join(", ", lorries[i].BrickGroups.Select(bg => bg.Weight))}");
            }

            Console.WriteLine($"\nFitness: {CalculateFitness(lorries)}");
        }

    }
}

