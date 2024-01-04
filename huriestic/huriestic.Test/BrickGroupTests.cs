/*
I learned how to develop this form of testing from several source,
the most notable resources used to learn were:

• the microsft official website for c#
https://learn.microsoft.com/en-us/visualstudio/test/walkthrough-creating-and-running-unit-tests-for-managed-code?view=vs-2022

• Youtube videos (example):
https://www.youtube.com/watch?v=w0SySMdRjk4
 */

using System;
using huriestic;

namespace huriestic.Test;

[TestClass]
public class BrickGroupTests
{
    [TestMethod]
    public void BrickGroup_With_ValidWeight()
    {
        // Arrange
        double expectedWeight = 50.0;
        var brickGroup = new BrickGroup(expectedWeight);

        // Act
        double actualWeight = brickGroup.Weight;

        // Assert
        Assert.AreEqual(expectedWeight, actualWeight
            , "When a new brick group is created wedight is set appropaitely.");
    }
}

[TestClass]
public class HeuristicAlgorithmTests
{
    private int iterationCount = 500;
    private int restratCount = 1;

    [TestMethod]
    public void CalculateFitness_Should_ReturnCorrectFitness()
    {
        // Arrange
        var lorries = new Lorry[3];
        lorries[0] = new Lorry();
        lorries[1] = new Lorry();
        lorries[2] = new Lorry();

        var brickGroups = new List<BrickGroup>
        {
            new BrickGroup(10.0),
            new BrickGroup(20.0),
            new BrickGroup(30.0)
        };


        lorries[0].AddBrickGroup(brickGroups[0]);
        lorries[1].AddBrickGroup(brickGroups[1]);
        lorries[2].AddBrickGroup(brickGroups[2]); 

        double maxWeight = lorries.Max(lorry => lorry.TotalWeight);
        double minWeight = lorries.Min(lorry => lorry.TotalWeight);


        var algorithm = new HeuristicAlgorithm(
            brickGroups, IterationLimit: iterationCount,
            RestartLimit: restratCount, lorries.Count());


        // Act
        double actualRange = algorithm.CalculateFitness(lorries);
        double expectedFitness = maxWeight - minWeight;

        // Assert
        Assert.AreEqual(expectedFitness, actualRange, "Fitness should be the differrence of weights across lorries.");
    }

    [TestMethod]
    public void MakeSmallChange_Should_ChnageLorriesBrick()
    {
        // Arrange
        var lorries = new Lorry[3];
        lorries[0] = new Lorry();
        lorries[1] = new Lorry();
        lorries[2] = new Lorry();

        var brickGroups = new List<BrickGroup>
        {
            new BrickGroup(10.0),
            new BrickGroup(20.0),
            new BrickGroup(30.0),
            new BrickGroup(40.0)
        };


        lorries[0].AddBrickGroup(brickGroups[0]);
        lorries[0].AddBrickGroup(brickGroups[1]);
        lorries[1].AddBrickGroup(brickGroups[2]);
        lorries[2].AddBrickGroup(brickGroups[3]);

        var algorithm = new HeuristicAlgorithm(
            BrickGroups: brickGroups,
            IterationLimit: iterationCount,
            RestartLimit: restratCount,
            LorriesCount: lorries.Count()
            );

        // Act
        var (sourceLorry, destinationLorry, brickGroup) = algorithm.MakeSmallChange(lorries);

        // Assert
        Assert.IsFalse(sourceLorry.BrickGroups.Contains(brickGroup), "Source lorry should not contain the moved brick group.");
        Assert.IsTrue(destinationLorry.BrickGroups.Contains(brickGroup), "Destination lorry should contain the moved brick group.");
    }
}


[TestClass]
public class LorryTests
{
    [TestMethod]
    public void AddingBrickGroup_Should_IncreaseWeights()
    {
        // Arrange
        double initialWeight = 60.0;
        double addingWeight = 40.0;

        var lorry = new Lorry();
        var initialBrickGroup = new BrickGroup(initialWeight);
        var addingWeightBrickGroup = new BrickGroup(addingWeight);


        // Act
        lorry.AddBrickGroup(initialBrickGroup);
        lorry.AddBrickGroup(addingWeightBrickGroup);
        double expectedWeight = initialWeight + addingWeight;

        // Assert
        Assert.AreEqual(expectedWeight, lorry.TotalWeight
            , "Lorry's total weight increases by the weight of the added brick group");
    }

    [TestMethod]
    public void AddBrickGroup_IncreasesTotalWeight()
    {
        // Arrange
        var lorry = new Lorry();
        var brickGroup = new BrickGroup(50.0);

        // Act
        lorry.AddBrickGroup(brickGroup);

        // Assert
        Assert.AreEqual(50.0, lorry.TotalWeight, "Adding a brick group should increase the total weight of the lorry.");
    }

    [TestMethod]
    public void RemoveBrickGroup_DecreasesTotalWeight()
    {
        // Arrange
        var lorry = new Lorry();
        var brickGroup = new BrickGroup(50.0);
        lorry.AddBrickGroup(brickGroup);

        // Act
        lorry.RemoveBrickGroup(brickGroup);

        // Assert
        Assert.IsTrue(50.0 != lorry.TotalWeight, "Removing a brick group should decrease the total weight of the lorry.");
    }

    [TestMethod]
    public void Lorry_TotalWeight_SumOfBrickGroups()
    {
        // Arrange
        var lorry = new Lorry();

        var brickGroups = new List<BrickGroup>
        {
            new BrickGroup(10.0),
            new BrickGroup(20.0),
            new BrickGroup(30.0)
        };

        lorry.AddBrickGroup(brickGroups[0]);
        lorry.AddBrickGroup(brickGroups[1]);
        lorry.AddBrickGroup(brickGroups[2]);

        // Act
        double totalWeight = lorry.TotalWeight;

        // Assert
        Assert.AreEqual(60.0, totalWeight, "Total weight should be the sum of all brick groups in the lorry.");
    }
}


