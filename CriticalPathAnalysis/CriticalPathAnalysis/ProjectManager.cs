using System;
using System.Reflection.Emit;
using System.Xml.Linq;

namespace CriticalPathAnalysis
{
	public class ProjectManager
	{
        private DirectedGraph graph;
        private int projectStartDate;

        public ProjectManager(int ProjectStartDate)
        {
            projectStartDate = ProjectStartDate;
            graph = new DirectedGraph();
        }

        public void populateGraph()
        {
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
        }

		public void printActivityData()
		{
            List<List<string>> table = new List<List<string>>();

            const int tableCol = 4;
            while (table.Count < tableCol)
                table.Add(new List<string>());

            int maxCount = geTableContent(table);
            int[] indices = Enumerable.Range(0, maxCount).ToArray();
            Array.Sort(indices, (i, j) => String.Compare(table[0][i], table[0][j], StringComparison.Ordinal));

            Console.WriteLine($"-------------|-------------|-------------|-------------");
            Console.WriteLine("{0,-15} {1,-15} {2,-15} {3,-15}", "Activity", "Earliest", "Latest", "Slack");

            for (int row = 0; row < maxCount; row++)
            {
                int index = indices[row];

                Console.WriteLine($"-------------|-------------|-------------|-------------");
                Console.WriteLine("{0,-15} {1,-15} {2,-15} {3,-15}",
                    table[0][index],
                    table[1].Count > index ? table[1][index] : "",
                    table[2].Count > index ? table[2][index] : "",
                    table[3].Count > index ? table[3][index] : "");
            }


            Console.WriteLine($"-------------|-------------|-------------|-------------\n");
        }

        public void printCriticalPaths()
        {
            var nodes = graph.GetNodes();

            PathFinder pathFinder = new PathFinder(projectStartDate);
            List<List<string>> criticalPaths = pathFinder.FindCriticalPaths(nodes[0], nodes[nodes.Count - 1], nodes);

            Console.WriteLine("Critical Paths:");
            foreach (var path in criticalPaths)
            {
                Console.WriteLine(string.Join(" -> ", path) + "\n");
            }
        }

        public void printProjectDuration()
        {
            Scheduler scheduler = new Scheduler(projectStartDate);
            int projectDuration = scheduler.CalculateProjectDuration(graph.GetNodes());
            Console.WriteLine($"\n\nTotal Project Duration: {projectDuration} units of time\n\n");
        }

        private int geTableContent(List<List<string>> table)
        {
            Scheduler scheduler = new Scheduler(start: this.projectStartDate);
            Dictionary<string, int> earliestStartTimes = scheduler.CalculateEarliestStartTimes(graph.GetNodes());
            Dictionary<string, int> latestStartTimes = scheduler.CalculateLateStartTimes(graph.GetNodes());

            int minCount = Math.Min(earliestStartTimes.Count, latestStartTimes.Count);
            for (int i = 0; i < minCount; i++)
            {
                var slack = latestStartTimes.ElementAt(i).Value - earliestStartTimes.ElementAt(i).Value;
                table[0].Add(earliestStartTimes.Count > i ? earliestStartTimes.ElementAt(i).Key : "");
                table[1].Add(earliestStartTimes.Count > i ? earliestStartTimes.ElementAt(i).Value.ToString() : "");
                table[2].Add(latestStartTimes.Count > i ? latestStartTimes.ElementAt(i).Value.ToString() : "");
                table[3].Add(slack.ToString());
            }

            return minCount;
        }
    }
}

