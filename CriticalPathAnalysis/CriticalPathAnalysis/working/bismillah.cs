using System;
using System.Collections.Generic;
using System.Linq;

namespace CriticalPathAnalysis;

/*class Programs
{
    static void Main()
    {
        DirectedGraph graph = new DirectedGraph();

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
            Console.WriteLine($"Node: {node.NodeName}");
            Console.WriteLine("Incoming Edges:");
            foreach (var incomingEdge in node.IncomingEdges)
            {
                Console.WriteLine($"  EdgeName: {incomingEdge.EdgeName}, Duration: {incomingEdge.Duration}");
            }

            Console.WriteLine("Outgoing Edges:");
            foreach (var outgoingEdge in node.OutgoingEdges)
            {
                Console.WriteLine($"  EdgeName: {outgoingEdge.EdgeName}, Duration: {outgoingEdge.Duration}");
            }

            Console.WriteLine();
        }

        Console.WriteLine();

        Dictionary<string, int> earliestStartTimes = graph.CalculateEarliestStartTimes();
        Console.WriteLine("Earliest Start Times:");
        foreach (var kvp in earliestStartTimes)
        {
            Console.WriteLine($"Edge: {kvp.Key}, Earliest Start Time: {kvp.Value}");
        }

        Console.WriteLine();

        int projectDuration = graph.CalculateProjectDuration();
        Console.WriteLine($"Total Project Duration: {projectDuration} units of time");

        Console.WriteLine();

        Dictionary<string, int> latestStartTimes = graph.CalculateLateStartTimes();
        Console.WriteLine("Latest Start Times:");
        foreach (var kvp in latestStartTimes)
        {
            Console.WriteLine($"Edge: {kvp.Key}, Latest Start Time: {kvp.Value}");
        }


        Console.WriteLine();
        Console.WriteLine();

        Dictionary<string, int> slackTimes = graph.CalculateSlack();

        // slackTimes dictionary now contains slack times for each activity
        foreach (var kvp in slackTimes)
        {
            Console.WriteLine($"Activity: {kvp.Key}, Slack: {kvp.Value}");
        }


        Console.WriteLine();
        Console.WriteLine();

        List<string> criticalPath = graph.CalculateCriticalPath();

        // Now 'criticalPath' contains the names of edges in the critical path
        Console.WriteLine("Critical Path: " + string.Join(" -> ", criticalPath));


        string startNodeName = "v1";
        string endNodeName = "v9";

        List<List<string>> allPaths = graph.FindAllPaths(startNodeName, endNodeName);

        // Now 'allPaths' contains a list of all paths from the start node to the end node
        foreach (var path in allPaths)
        {
            Console.WriteLine("Path: " + string.Join(" -> ", path));
        }


        List<List<string>> criticalPaths = graph.FindCriticalPaths(startNodeName, endNodeName);

        // Now 'criticalPaths' contains a list of critical paths from the start node to the end node
        Console.WriteLine("Critical Paths:");

        foreach (var path in criticalPaths)
        {
            Console.WriteLine(string.Join(" -> ", path));
        }

    }
}
*/