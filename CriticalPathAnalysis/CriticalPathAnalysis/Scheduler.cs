using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace CriticalPathAnalysis
{
	public class Scheduler
	{
        private int projectStart;

        public Scheduler(int start)
        {
            projectStart = start;
        }

        public Dictionary<string, int> CalculateEarliestStartTimes(List<Node> nodes)
        {
            var pathFinder = new PathFinder(projectStart);
            Dictionary<string, int> earliestStartTimes = new Dictionary<string, int>();
            List<Node> sortedNodes= pathFinder.TopologicalSort(nodes);

            foreach (var edge in sortedNodes.SelectMany(node => node.OutgoingEdges))
                earliestStartTimes[edge.EdgeName] = projectStart + CalculateTotalDuration(nodes[0], edge.Predecessor);

            return earliestStartTimes;
        }

        public Dictionary<string, int> CalculateLateStartTimes(List<Node> nodes)
        {
            var pathFinder = new PathFinder(projectStart);
            List<Node> sortedNodes = pathFinder.TopologicalSort(nodes);
            Dictionary<string, int> latestStartTimes = new Dictionary<string, int>();

            const int LAST_NODE_START_OFFSETT = 1;
            int projectDuration = CalculateProjectDuration(nodes) + LAST_NODE_START_OFFSETT;

            foreach (var edge in sortedNodes.SelectMany(node => node.OutgoingEdges))
                latestStartTimes[edge.EdgeName] = projectDuration - CalculateTotalDuration(edge.Successor, nodes.Last()) - edge.Duration;

            return latestStartTimes;
        }

        public int CalculateProjectDuration(List<Node> nodes)
        {
            Dictionary<string, int> earliestStartTimes = CalculateEarliestStartTimes(nodes);
            int projectDuration = earliestStartTimes.Values.Max() + projectStart;

            return projectDuration;
        }

        public int CalculateTotalDuration(Node startNode, Node endNode)
        {
            var pathFinder = new PathFinder(projectStart);
            List<List<Edge>> allPaths = new List<List<Edge>>();
            List<Edge> currentPath = new List<Edge>();

            pathFinder.ExplorePaths(startNode, endNode, currentPath, allPaths);

            int totalDuration = int.MinValue;
            foreach (var path in allPaths)
                totalDuration = Math.Max(totalDuration, path.Sum(edge => edge.Duration));

            return totalDuration;
        }
    }
}