using System;
using Project4;

namespace Project4.Test;

[TestClass]
public class AlgorithmsUnitTest
{
    SortingAlgorithm algorithm = new SortingAlgorithm();

    [TestMethod]
    public void BubbleSort_Working_Correctly()
    {
        int[] testArray = { 4, 2, 5, 1, 3 };
        int[] expected = { 1, 2, 3, 4, 5 };
        algorithm.BubbleSort(testArray);
        CollectionAssert.AreEqual(expected, testArray,
            "Is Bubble Sort working as inteneded?");
    }

    [TestMethod]
    public void QuickSort_Working_Correctly()
    {
        int[] testArray = { 4, 2, 5, 1, 3 };
        int[] expected = { 1, 2, 3, 4, 5 };
        algorithm.QuickSort(testArray, 0, testArray.Length - 1);
        CollectionAssert.AreEqual(expected, testArray,
            "Is Quick Sort working as inteneded?");
    }

    [TestMethod]
    public void Swap_Working_Correctly()
    {
        int expectedValueX = 5;
        int expectedValueY = 8;

        int x = 8;
        int y = 5;

        algorithm.Swap(ref x, ref y);

        Assert.IsTrue(x == expectedValueX,
            "Has the value X been switched with Y?");

        Assert.IsTrue(y == expectedValueY,
            "Has the value Y been switched with X?");
    }


    [TestMethod]
    public void Partition_Working_Correctly()
    {
        int[] testArray = { 4, 2, 5, 1, 3 };
        int low = 0;
        int high = testArray.Length - 1;
        int pivotIndex = algorithm.Partition(testArray, low, high);

        int pivotValue = testArray[pivotIndex];
        for (int i = 0; i < pivotIndex; i++)
        {
            Assert.IsTrue(testArray[i] <= pivotValue,
                "Element on the left of pivot is greater than pivot.");
        }

        for (int i = pivotIndex + 1; i <= high; i++)
        {
            Assert.IsTrue(testArray[i] > pivotValue,
                "Element on the right of pivot is less than or equal to pivot.");
        }

        Assert.IsTrue(pivotIndex >= low && pivotIndex <= high,
            "Pivot index is outside the range of low and high.");
    }
}
