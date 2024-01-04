/*
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Runtime.Intrinsics;
using System.Xml.Linq;

public class DirectedGraph<T>
{
    private Dictionary<T, List<Edge<T>>> adjacencyList;

    public DirectedGraph()
    {
        adjacencyList = new Dictionary<T, List<Edge<T>>>();
    }

    public void AddNode(T node)
    {
        if (!adjacencyList.ContainsKey(node))
        {
            adjacencyList[node] = new List<Edge<T>>();
        }
    }

    public void AddEdge(T source, T destination, string name, int duration)
    {
        if (!adjacencyList.ContainsKey(source))
        {
            AddNode(source);
        }

        if (!adjacencyList.ContainsKey(destination))
        {
            AddNode(destination);
        }

        Edge<T> edge = new Edge<T>(destination, name, duration);
        edge.Predecessors.Add(source);  // Add the source as a predecessor
        adjacencyList[source].Add(edge);
    }


    public List<Edge<T>> GetNeighbors(T node)
    {
        if (adjacencyList.ContainsKey(node))
        {
            return adjacencyList[node];
        }
        else
        {
            throw new KeyNotFoundException("Node not found in the graph.");
        }
    }

    public List<T> GetNodes()
    {
        return new List<T>(adjacencyList.Keys);
    }


    public Dictionary<string, int> CalculateEarliestStartTimes()
    {
        Dictionary<T, int> earliestStartTimes = new Dictionary<T, int>();
        List<T> nodes = TopologicalSort();

        const int earliestStartTime = 1; // project starts on Day 1

        Dictionary<string, int> early = new Dictionary<string, int>();

        foreach (var node in nodes)
        {
            List<Edge<T>> neighbors = GetNeighbors(node);
            foreach (var edge in neighbors)
            {
                if (!earliestStartTimes.ContainsKey(node))
                {
                    earliestStartTimes[node] = earliestStartTime;
                }

                int startTime =  edge.Duration;

                foreach (var predecessor in edge.Predecessors)
                {
                    earliestStartTimes[edge.Destination] = Math.Max(earliestStartTimes[predecessor] + edge.Duration, earliestStartTime);

                    early[edge.Name] = earliestStartTimes[predecessor];
                }

            }
        }

        return early;
    }

    public Dictionary<string, int> CalculateLateStartTimes()
    {
        int projectDuration = CalculateProjectDuration() + 2;

        Dictionary<T, int> lateStartTimes = new Dictionary<T, int>();
        List<T> nodes = TopologicalSort();

        Dictionary<string, int> late = new Dictionary<string, int>();

        T lastNode = nodes.Last();
        lateStartTimes[lastNode] = projectDuration;

        for (int i = nodes.Count - 1; i >= 0; i--)
        {
            T currentNode = nodes[i];

            if (!lateStartTimes.ContainsKey(currentNode))
            {
                lateStartTimes[currentNode] = projectDuration;
            }

            List<Edge<T>> neighbors = GetNeighbors(currentNode);

            foreach (var edge in neighbors)
            {

                // Update late start time for the current node based on its successors
                lateStartTimes[currentNode] = Math.Min(lateStartTimes[currentNode], lateStartTimes[edge.Destination] - edge.Duration);

                Console.WriteLine($"Edge: {currentNode} {edge.Destination} | {edge.Name} | {lateStartTimes[currentNode]}   | {edge.Duration} | {projectDuration}");
            }
        }

        Console.WriteLine();
        Console.WriteLine();
        return late;
    }






    // how it should be written   | how its saed in T
    //Console.WriteLine($"{edge.Name} | {predecessor}");

    //late[edge.Name] = lateStartTimes[predecessor];

    //Console.WriteLine($"{lateStartTimes[edge.Destination]}");

    //Console.WriteLine($"{edge.Destination} {late[edge.Name]} | {predecessor} {lateStartTimes[predecessor]}");



    public int CalculateProjectDuration()
    {
        Dictionary<string, int> earliestStartTimes = CalculateEarliestStartTimes();

        // Find the maximum completion time among all nodes
        int projectDuration = earliestStartTimes.Values.Max();

        return projectDuration;
    }


    public int CalculatePathDuration(List<T> path)
    {
        int duration = 0;
        for (int i = 0; i < path.Count - 1; i++)
        {
            T currentNode = path[i];
            T nextNode = path[i + 1];

            // Assuming adjacencyList stores Edge<T> objects
            List<Edge<T>> edges = adjacencyList[currentNode];
            Edge<T> edge = edges.Find(e => EqualityComparer<T>.Default.Equals(e.Destination, nextNode));

            if (edge != null)
            {
                duration += edge.Duration;
            }
            else
            {
                throw new InvalidOperationException($"No edge found between {currentNode} and {nextNode}");
            }
        }
        return duration;
    }


    //duration = projectGraph.CalculatePathDuration(path);
    //Console.WriteLine($"Path: {string.Join(" -> ", path.Select(v => v.getName))}, Duration: {duration} days");


    public List<List<T>> GetAllPaths(T start, T end)
    {
        List<List<T>> allPaths = new List<List<T>>();
        HashSet<T> visited = new HashSet<T>();
        List<T> currentPath = new List<T>();

        GetAllPathsUtil(start, end, visited, currentPath, allPaths);

        return allPaths;
    }
    private void GetAllPathsUtil(T current, T end, HashSet<T> visited, List<T> currentPath, List<List<T>> allPaths)
    {
        visited.Add(current);
        currentPath.Add(current);

        if (current.Equals(end))
        {
            // Add a copy of the current path to the list of all paths
            allPaths.Add(new List<T>(currentPath));
        }
        else
        {
            List<Edge<T>> neighbors = GetNeighbors(current);
            foreach (var neighbor in neighbors)
            {
                if (!visited.Contains(neighbor.Destination))
                {
                    GetAllPathsUtil(neighbor.Destination, end, visited, currentPath, allPaths);
                }
            }
        }

        // Backtrack: remove the current node from the path and mark it as unvisited
        currentPath.Remove(current);
        visited.Remove(current);
    }





    private List<T> TopologicalSort()
    {
        List<T> sortedNodes = new List<T>();
        HashSet<T> visited = new HashSet<T>();

        foreach (var node in GetNodes())
        {
            TopologicalSortUtil(node, visited, sortedNodes);
        }

        return sortedNodes;
    }


    private void TopologicalSortUtil(T node, HashSet<T> visited, List<T> sortedNodes)
    {
        if (!visited.Contains(node))
        {
            visited.Add(node);
            List<Edge<T>> neighbors = GetNeighbors(node);

            foreach (var edge in neighbors)
            {
                TopologicalSortUtil(edge.Destination, visited, sortedNodes);
            }

            sortedNodes.Insert(0, node);
        }
    }

}

public class Edge<T>
{
    public T Destination { get; }
    public string Name { get; }
    public int Duration { get; }
    public List<T> Predecessors { get; }  // New property for predecessors

    public Edge(T destination, string name, int duration)
    {
        Destination = destination;
        Name = name;
        Duration = duration;
        Predecessors = new List<T>();
    }
}

class Program
{
    static void Main()
    {
        DirectedGraph<string> graph = new DirectedGraph<string>();

        graph.AddNode("v1");
        graph.AddNode("v2");
        graph.AddNode("v3");
        graph.AddNode("v4");
        graph.AddNode("v5");
        graph.AddNode("v6");
        graph.AddNode("v7");
        graph.AddNode("v8");
        graph.AddNode("v9");


        graph.AddEdge("v1", "v2", "v1..v2", 6);
        graph.AddEdge("v1", "v3", "v1..v3", 4);
        graph.AddEdge("v1", "v4", "v1..v4", 5);

        graph.AddEdge("v2", "v5", "v2..v5", 1);

        graph.AddEdge("v3", "v5", "v3..v5", 1);

        graph.AddEdge("v4", "v6", "v4..v6", 2);

        graph.AddEdge("v5", "v7", "v5..v7", 9);
        graph.AddEdge("v5", "v8", "v5..v8", 7);

        graph.AddEdge("v6", "v8", "v6..v8", 4);

        graph.AddEdge("v7", "v9", "v7..v9", 2);

        graph.AddEdge("v8", "v9", "v8..v9", 4);







        foreach (var node in graph.GetNodes())
        {
            Console.WriteLine($"Neighbors of Node: {node}:");
            List<Edge<string>> neighbors = graph.GetNeighbors(node);
            foreach (var edge in neighbors)
            {
                Console.WriteLine($"  EdgeName: {edge.Name}, Destination: {edge.Destination}, Duration: {edge.Duration}");
            }
            Console.WriteLine();
        }

        Dictionary<string, int> earliestStartTimes = graph.CalculateEarliestStartTimes();

        Console.WriteLine("Earliest Start Times:");
        foreach (var kvp in earliestStartTimes)
        {
            Console.WriteLine($"Edge: {kvp.Key}, Earliest Start Time: {kvp.Value}");
        }

        Console.WriteLine();
        Console.WriteLine();


        //int projectDuration = graph.CalculateProjectDuration();
        //Console.WriteLine($"Total Project Duration: {projectDuration} units of time");


        int projectDuration = graph.CalculateProjectDuration();
        Console.WriteLine($"Total Project Duration: {projectDuration} units of time");


        Console.WriteLine();
        Console.WriteLine();

        Dictionary<string, int> LatestStartTimes = graph.CalculateLateStartTimes();

        Console.WriteLine("Latest Start Times:");
        foreach (var kvp in LatestStartTimes)
        {
            Console.WriteLine($"Edge: {kvp.Key}, Latest Start Time: {kvp.Value}");
        }

    }
}


/*  AMAZING WORKS.
List<List<string>> allPaths = graph.GetAllPaths("v1", "v9");

Console.WriteLine("All Paths from v1 to v9:");
foreach (var path in allPaths)
{
    Console.WriteLine(string.Join(" -> ", path));

    // Calculate and print the duration of each path
    int pathDuration = graph.CalculatePathDuration(path);
    Console.WriteLine($"Duration: {pathDuration} units of time");
    Console.WriteLine();
}
*/