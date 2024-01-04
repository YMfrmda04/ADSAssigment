/*using System;
using System.Diagnostics.Metrics;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace CriticalPathAnalysis
{
    public class DirectedGraphs
    {
        private List<Node> nodes;

        public DirectedGraphs()
        {
            nodes = new List<Node>();
        }

        public void AddNode(string nodeName)
        {
            if (!nodes.Any(n => n.NodeName == nodeName))
                nodes.Add(new Node(nodeName));
        }

        public void AddEdge(string sourceNodeName, string destinationNodeName, string edgeName, int duration)
        {
            Node sourceNode = nodes.Find(n => n.NodeName == sourceNodeName) ?? throw new InvalidOperationException($"Node {sourceNodeName} not found");
            Node destinationNode = nodes.Find(n => n.NodeName == destinationNodeName) ?? throw new InvalidOperationException($"Node {destinationNodeName} not found");

            Edge edge = new Edge(edgeName, duration, sourceNode, destinationNode);
        }

        public List<Node> GetNodes()
        {
            return new List<Node>(nodes);
        }

        public Dictionary<string, int> CalculateEarliestStartTimes()
        {
            Dictionary<string, int> earliestStartTimes = new Dictionary<string, int>();
            List<Node> nodes = TopologicalSort();

            const int PROJECT_START = 1;

            foreach (var node in nodes)
            {
                List<Edge> outgoingEdges = node.OutgoingEdges;
                foreach (var edge in outgoingEdges)
                {
                    earliestStartTimes[edge.EdgeName] = PROJECT_START + CalculateTotalDuration(nodes[0], node);
                }
            }

            return earliestStartTimes;
        }

        public Dictionary<string, int> CalculateLateStartTimes()
        {
            Dictionary<string, int> latestStartTimes = new Dictionary<string, int>();
            List<Node> nodes = TopologicalSort();
            int projectDuration = CalculateProjectDuration() + 2;

            var count = 0;
            foreach (var node in nodes)
            {
                List<Edge> outgoingEdges = node.OutgoingEdges;
                string nodeName = "";

                foreach (var edge in outgoingEdges)
                {
                    latestStartTimes[edge.EdgeName] = projectDuration - CalculateTotalDuration(edge.Successor, nodes.Last()) - edge.Duration;
                }
            }

            return latestStartTimes;
        }

        public Dictionary<string, int> CalculateSlack()
        {
            Dictionary<string, int> slackTimes = new Dictionary<string, int>();
            Dictionary<string, int> earliestStartTimes = CalculateEarliestStartTimes();
            Dictionary<string, int> latestStartTimes = CalculateLateStartTimes();

            foreach (var node in nodes)
            {
                List<Edge> outgoingEdges = node.OutgoingEdges;
                foreach (var edge in outgoingEdges)
                {
                    string edgeName = edge.EdgeName;

                    // Slack is the difference between late start time and early start time
                    int slack = latestStartTimes[edgeName] - earliestStartTimes[edgeName];

                    slackTimes[edgeName] = slack;
                }
            }

            return slackTimes;
        }

        public int CalculateProjectDuration()
        {
            Dictionary<string, int> earliestStartTimes = CalculateEarliestStartTimes();
            int projectDuration = earliestStartTimes.Values.Max();

            return projectDuration;
        }

        public int CalculateTotalDuration(Node startNode, Node endNode)
        {
            List<List<Edge>> allPaths = new List<List<Edge>>();
            List<Edge> currentPath = new List<Edge>();

            ExplorePaths(startNode, endNode, currentPath, allPaths);

            int totalDuration = int.MinValue;

            foreach (var path in allPaths)
            {
                totalDuration = Math.Max(totalDuration, path.Sum(edge => edge.Duration));
            }

            return totalDuration;
        }

        private void ExplorePaths(Node currentNode, Node endNode, List<Edge> currentPath, List<List<Edge>> allPaths)
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
        private List<Node> TopologicalSort()
        {
            List<Node> sortedNodes = new List<Node>();
            HashSet<Node> visited = new HashSet<Node>();

            foreach (var node in GetNodes())
            {
                TopologicalSortUtil(node, visited, sortedNodes);
            }

            return sortedNodes;
        }

        private void TopologicalSortUtil(Node node, HashSet<Node> visited, List<Node> sortedNodes)
        {
            if (!visited.Contains(node))
            {
                visited.Add(node);
                List<Edge> outgoingEdges = node.OutgoingEdges;

                foreach (var edge in outgoingEdges)
                {
                    TopologicalSortUtil(edge.Successor, visited, sortedNodes);
                }

                sortedNodes.Insert(0, node);
            }
        }

        public List<string> CalculateCriticalPath()
        {
            List<string> criticalPath = new List<string>();
            Dictionary<string, int> slackTimes = CalculateSlack();


            // Iterate through all edges to find critical path
            foreach (var node in nodes)
            {
                List<Edge> outgoingEdges = node.OutgoingEdges;
                foreach (var edge in outgoingEdges)
                {
                    int slack = slackTimes[edge.EdgeName];

                    // If slack is zero, the edge is part of the critical path
                    if (slack == 0)
                    {
                        criticalPath.Add(edge.EdgeName);
                    }
                }
            }

            return criticalPath;
        }

        public List<List<string>> FindAllPaths(string startNodeName, string endNodeName)
        {
            List<List<Edge>> allPaths = new List<List<Edge>>();
            Node startNode = nodes.Find(n => n.NodeName == startNodeName) ?? throw new InvalidOperationException($"Node {startNodeName} not found");
            Node endNode = nodes.Find(n => n.NodeName == endNodeName) ?? throw new InvalidOperationException($"Node {endNodeName} not found");

            List<Edge> currentPath = new List<Edge>();
            ExplorePaths(startNode, endNode, currentPath, allPaths);

            // Convert the list of edges to a list of edge names
            List<List<string>> allPathNames = allPaths.Select(path => path.Select(edge => edge.EdgeName).ToList()).ToList();

            return allPathNames;
        }


        public List<List<string>> FindCriticalPaths(string startNodeName, string endNodeName)
        {
            List<List<Edge>> allPaths = new List<List<Edge>>();
            Node startNode = nodes.Find(n => n.NodeName == startNodeName) ?? throw new InvalidOperationException($"Node {startNodeName} not found");
            Node endNode = nodes.Find(n => n.NodeName == endNodeName) ?? throw new InvalidOperationException($"Node {endNodeName} not found");

            List<Edge> currentPath = new List<Edge>();
            ExplorePaths(startNode, endNode, currentPath, allPaths);

            List<List<string>> criticalPaths = allPaths
                .Where(path => IsCriticalPath(path.Select(edge => edge.EdgeName).ToList()))
                .Select(path => path.Select(edge => edge.EdgeName).ToList())
                .ToList();

            return criticalPaths;
        }

        private bool IsCriticalPath(List<string> path)
        {
            foreach (var edgeName in path)
            {
                if (!IsCriticalEdge(edgeName))
                {
                    return false;
                }
            }
            return true;
        }

        private bool IsCriticalEdge(string edgeName)
        {
            // Assuming 'CalculateCriticalPath' returns the names of critical edges
            List<string> criticalPath = CalculateCriticalPath();
            return criticalPath.Contains(edgeName);
        }
    }
}*/