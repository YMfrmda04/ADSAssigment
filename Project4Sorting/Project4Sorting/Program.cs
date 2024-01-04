using System;
using System.Diagnostics;
using System.Linq;
using System.IO;
using System.Reflection;

namespace Project4
{

    public class SortingAlgorithm
    {
        public void Swap(ref int x, ref int y)
        {
            int temp = x;
            x = y;
            y = temp;
        }

        public int Partition(int[] data, int low, int high)
        {
            int pivot = data[high];
            int i = low - 1;
            for (int j = low; j < high; j++)
            {
                if (data[j] < pivot)
                {
                    i++;
                    Swap(ref data[i], ref data[j]);
                }
            }
            Swap(ref data[i + 1], ref data[high]);
            return i + 1;
        }

        public void QuickSort(int[] data, int low, int high)
        {
            if (low < high)
            {
                int pi = Partition(data, low, high);
                QuickSort(data, low, pi - 1);
                QuickSort(data, pi + 1, high);
            }
        }

        public void BubbleSort(int[] data)
        {
            int n = data.Length;
            bool noSwap;

            do
            {
                noSwap = true;
                for (int i = 0; i < n - 1; i++)
                {
                    if (data[i] > data[i + 1])
                    {
                        Swap(ref data[i], ref data[i + 1]);
                        noSwap = false;
                    }
                }
            }
            while (!noSwap);
        }

        static void Main()
        {
            SortingAlgorithm algorithm = new SortingAlgorithm();
            int[] sizes = { 500, 1_000, 2_000, 5_000, 10_000 };

            string fileName = "sortTimes.csv";
            string filePath = findCurrentPath(fileName);

            if (!File.Exists(filePath))
            {
                File.WriteAllText(filePath, "Size,BubbleSortTime,QuickSortTime\n");
            }

            foreach (var size in sizes)
            {
                int[] bubbleSortData = Enumerable.Range(1, size).OrderBy(x => Guid.NewGuid()).ToArray();
                int[] quickSortData = (int[])bubbleSortData.Clone();
                Stopwatch sw = Stopwatch.StartNew();
                algorithm.BubbleSort(bubbleSortData);
                sw.Stop();

                double bubbleSortTime = sw.Elapsed.TotalSeconds;
                Console.WriteLine($"Bubble Sort Time: {bubbleSortTime} seconds");

                sw.Restart();
                algorithm.QuickSort(quickSortData, 0, quickSortData.Length - 1);
                sw.Stop();

                double quickSortTime = sw.Elapsed.TotalSeconds;
                Console.WriteLine($"Quick Sort took: {quickSortTime} s\n");

                File.AppendAllText(filePath, $"{size},{bubbleSortTime},{quickSortTime}\n");
            }
        }

        static string findCurrentPath(string filename)
        {

            string exePath = Assembly.GetExecutingAssembly().Location;

            DirectoryInfo exeDirectory = new DirectoryInfo(Path.GetDirectoryName(exePath));
            DirectoryInfo targetDirectory = exeDirectory.Parent?.Parent?.Parent;

            string filePath = Path.Combine(targetDirectory.FullName, filename);

            return filePath;
        }


    }
}