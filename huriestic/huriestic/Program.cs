using System;
using System.Collections.Generic;
using System.Linq;

using System.IO;
using System.Reflection;


namespace huriestic;

public class Programs
{
    public static void Main()
    {

        FileSystem fileSystem = new FileSystem();
        string filename = "dataset2.csv";
        List<BrickGroup> brickGroups = fileSystem.ReadBrickGroupsFromFile(filename);

        HeuristicAlgorithm algorithm = new HeuristicAlgorithm(
            BrickGroups: brickGroups,
            IterationLimit: 10000,
            RestartLimit: 1,
            LorriesCount: 3
            );

        algorithm.RunAlgorithm();

        var bestFitnesses = algorithm.BestFitnesses;
        fileSystem.WriteBestFitnessesToCsv(bestFitnesses, "bestFitnesses.csv");

    }
}