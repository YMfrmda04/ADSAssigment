using System;
using System.Reflection;

namespace huriestic
{
    public class FileSystem
    {
        public List<BrickGroup> ReadBrickGroupsFromFile(string filename)
        {
            List<BrickGroup> brickGroups = new List<BrickGroup>();
            try
            {
                string filePath = findCurrentPath(filename);
                string[] lines = File.ReadAllLines(filePath);

                foreach (string line in lines)
                {
                    // Splitting the line by comma to handle CSV format
                    string[] values = line.Split(',');

                    foreach (string value in values)
                    {
                        if (double.TryParse(value, out double weight))
                        {
                            brickGroups.Add(new BrickGroup(weight));
                        }
                        else
                        {
                            Console.WriteLine($"Invalid weight format: {value}");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while reading the file: {ex.Message}");
            }

            return brickGroups;
        }

        public void WriteBestFitnessesToCsv(Dictionary<string, double> bestFitnesses, string fileName)
        {
            string filePath = findCurrentPath(fileName);
            using (StreamWriter file = new StreamWriter(filePath))
            {
                file.WriteLine("Iteration,BestFitness"); // Header line
                foreach (var entry in bestFitnesses)
                {
                    file.WriteLine($"{entry.Key},{entry.Value}");
                }
            }
        }

        private string findCurrentPath(string filename)
        {

            // Get the full path of the current executable
            string exePath = Assembly.GetExecutingAssembly().Location;

            // Get the directory of the executable
            DirectoryInfo exeDirectory = new DirectoryInfo(Path.GetDirectoryName(exePath));

            // Move up three directories
            DirectoryInfo targetDirectory = exeDirectory.Parent?.Parent?.Parent;

            string filePath = Path.Combine(targetDirectory.FullName, filename);

            return filePath;
        }
    }

}

