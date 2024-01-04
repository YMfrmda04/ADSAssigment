using System;
using System.Xml.Linq;
using CriticalPathAnalysis;

namespace CriticalPathAnalysis
{
	public class PathFinder
	{
        private int start;

        public PathFinder(int Start)
        {
            start = Start;
        }

        public List<List<string>> FindCriticalPaths(Node startNode, Node endNode, List<Node> nodes)
        {
            List<List<Edge>> allPaths = new List<List<Edge>>();
            List<Edge> currentPath = new List<Edge>();

            ExplorePaths(startNode, endNode, currentPath, allPaths);
            List<List<string>> criticalPaths = allPaths
                .Where(path => IsCriticalPath(path.Select(edge => edge.EdgeName).ToList(), nodes))
                .Select(path => path.Select(edge => edge.EdgeName).ToList())
                .ToList();

            return criticalPaths;
        }



        public void ExplorePaths(Node currentNode, Node endNode, List<Edge> currentPath, List<List<Edge>> allPaths)
        {
            if (currentNode == endNode)
            {
                allPaths.Add(new List<Edge>(currentPath));
                return;
            }

            foreach (var outgoingEdge in currentNode.OutgoingEdges)
            {
                currentPath.Add(outgoingEdge);
                ExplorePaths(outgoingEdge.Successor, endNode, currentPath, allPaths);
                currentPath.RemoveAt(currentPath.Count - 1);
            }
        }


        // O(N+E) where N is no of nodes and E is no of Edges.
        public List<Node> TopologicalSort(List<Node> nodes)
        {
            List<Node> sortedNodes = new List<Node>();
            HashSet<Node> visited = new HashSet<Node>();

            foreach (var node in nodes)
                TopologicalSortUtil(node, visited, sortedNodes);

            return sortedNodes;
        }

        private void TopologicalSortUtil(Node node, HashSet<Node> visited, List<Node> sortedNodes)
        {
            if (!visited.Contains(node))
            {
                visited.Add(node);
                List<Edge> outgoingEdges = node.OutgoingEdges;

                foreach (var edge in outgoingEdges)
                    TopologicalSortUtil(edge.Successor, visited, sortedNodes);

                sortedNodes.Insert(0, node);
            }
        }

        public List<string> CalculateCriticalPath(List<Node> nodes)
        {
            Scheduler scheduler = new Scheduler(start);
            Dictionary<string, int> earliestStartTimes = scheduler.CalculateEarliestStartTimes(nodes);
            Dictionary<string, int> latestStartTimes = scheduler.CalculateLateStartTimes(nodes);
            Dictionary<string, int> slackTimes = new Dictionary<string, int>();


            int minCount = Math.Min(earliestStartTimes.Count, latestStartTimes.Count);
            for (int i = 0; i < minCount; i++)
            {
                slackTimes[latestStartTimes.ElementAt(i).Key] = latestStartTimes.ElementAt(i).Value - earliestStartTimes.ElementAt(i).Value;
            }

            List<string> criticalPath = nodes
                .SelectMany(node => node.OutgoingEdges)
                .Where(edge => slackTimes[edge.EdgeName] == 0)
                .Select(edge => edge.EdgeName)
                .ToList();

            return criticalPath;
        }


        private bool IsCriticalPath(List<string> path, List<Node> nodes)
        {
            foreach (var edgeName in path)
                if (!IsCriticalEdge(edgeName, nodes))
                    return false;

            return true;
        }

        private bool IsCriticalEdge(string edgeName, List<Node> nodes)
        {
            List<string> criticalPath = CalculateCriticalPath(nodes);
            return criticalPath.Contains(edgeName);
        }
    }
}


/*
 * 
public List<List<string>> FindAllPaths(string startNodeName, string endNodeName)
{
    DirectedGraph directedGraph = new DirectedGraph();
    var nodes = directedGraph.GetNodes();

    List<List<Edge>> allPaths = new List<List<Edge>>();
    Node startNode = nodes.Find(n => n.NodeName == startNodeName) ?? throw new InvalidOperationException($"Node {startNodeName} not found");
    Node endNode = nodes.Find(n => n.NodeName == endNodeName) ?? throw new InvalidOperationException($"Node {endNodeName} not found");

    List<Edge> currentPath = new List<Edge>();
    ExplorePaths(startNode, endNode, currentPath, allPaths);

    // Convert the list of edges to a list of edge names
    List<List<string>> allPathNames = allPaths.Select(path => path.Select(edge => edge.EdgeName).ToList()).ToList();

    return allPathNames;
}
*/