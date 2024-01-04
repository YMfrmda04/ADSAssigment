/*
using System;
using System.Collections.Generic;

public class Node
{
    private string nodeName;
    private List<Edge> incomingEdges;
    private List<Edge> outgoingEdges;

    public string NodeName { get => nodeName; private set => nodeName = value; }
    public List<Edge> IncomingEdges { get => incomingEdges; }
    public List<Edge> OutgoingEdges { get => outgoingEdges; }

    public Node(string nodeName)
    {
        this.nodeName = nodeName;
        this.incomingEdges = new List<Edge>();
        this.outgoingEdges = new List<Edge>();
    }

    public void AddIncomingEdge(Edge edge)
    {
        incomingEdges.Add(edge);
    }

    public void AddOutgoingEdge(Edge edge)
    {
        outgoingEdges.Add(edge);
    }
}

public class Edge
{
    private string edgeName;
    private int duration;
    private Node predecessor;
    private Node successor;

    public string EdgeName { get => edgeName; private set => edgeName = value; }
    public int Duration { get => duration; private set => duration = value; }
    public Node Predecessor { get => predecessor; }
    public Node Successor { get => successor; }

    public Edge(string edgeName, int duration, Node predecessor, Node successor)
    {
        this.edgeName = edgeName;
        this.duration = duration;
        this.predecessor = predecessor;
        this.successor = successor;

        predecessor.AddOutgoingEdge(this);
        successor.AddIncomingEdge(this);
    }
}

public class DirectedGraph
{
    private List<Node> nodes;
    private List<Edge> edges;

    public DirectedGraph()
    {
        this.nodes = new List<Node>();
        this.edges = new List<Edge>();
    }

    public void AddNode(Node node)
    {
        nodes.Add(node);
    }

    public void AddEdge(Edge edge)
    {
        edges.Add(edge);
    }

    // Additional methods for graph traversal, manipulation, etc.

    // Example method to get successors of a node
    public List<Node> GetSuccessors(Node node)
    {
        List<Node> successors = new List<Node>();
        foreach (Edge edge in node.OutgoingEdges)
        {
            successors.Add(edge.Successor);
        }
        return successors;
    }

    // Example method to get predecessors of a node
    public List<Node> GetPredecessors(Node node)
    {
        List<Node> predecessors = new List<Node>();
        foreach (Edge edge in node.IncomingEdges)
        {
            predecessors.Add(edge.Predecessor);
        }
        return predecessors;
    }


    public List<List<Node>> FindAllPaths(Node startNode, Node endNode)
    {
        List<List<Node>> allPaths = new List<List<Node>>();
        List<Node> currentPath = new List<Node>();
        HashSet<Node> visitedNodes = new HashSet<Node>();
        FindAllPathsRecursive(startNode, endNode, currentPath, allPaths, visitedNodes);
        return allPaths;
    }

    private void FindAllPathsRecursive(Node currentNode, Node endNode, List<Node> currentPath, List<List<Node>> allPaths, HashSet<Node> visitedNodes)
    {
        visitedNodes.Add(currentNode);
        currentPath.Add(currentNode);

        if (currentNode == endNode)
        {
            // Found a path to the end node
            allPaths.Add(new List<Node>(currentPath));
        }
        else
        {
            // Continue the traversal for each successor
            foreach (Node successor in GetSuccessors(currentNode))
            {
                if (!visitedNodes.Contains(successor))
                {
                    FindAllPathsRecursive(successor, endNode, currentPath, allPaths, visitedNodes);
                }
            }
        }

        // Backtrack
        visitedNodes.Remove(currentNode);
        currentPath.RemoveAt(currentPath.Count - 1);
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

                int startTime = edge.Duration;

                foreach (var predecessor in edge.Predecessors)
                {
                    earliestStartTimes[edge.Destination] = Math.Max(earliestStartTimes[predecessor] + edge.Duration, earliestStartTime);

                    early[edge.Name] = earliestStartTimes[predecessor];
                }

            }
        }

        return early;
    }

    public int CalculateES(List<Node> path)
    {
        int earliestStart = 1;
        foreach (Node node in path)
        {
            int maxPredecessorFinishTime = 0;
            foreach (Edge incomingEdge in node.IncomingEdges)
            {
                Node predecessor = incomingEdge.Predecessor;
                // Calculate finish time for the predecessor
                int finishTime = earliestStart + incomingEdge.Duration;
                maxPredecessorFinishTime = Math.Max(maxPredecessorFinishTime, finishTime);
            }
            earliestStart = maxPredecessorFinishTime;
        }
        return earliestStart;
    }
}

class Programz
{
    static void Mainz()
    {
        // Creating nodes
        Node v1 = new Node("V1");
        Node v2 = new Node("V2");
        Node v3 = new Node("V3");
        Node v4 = new Node("V4");
        Node v5 = new Node("V5");
        Node v6 = new Node("V6");
        Node v7 = new Node("V7");
        Node v8 = new Node("V8");
        Node v9 = new Node("V9");

        // Creating edges
        Edge e1 = new Edge("V1..V2", 1, v1, v2);
        Edge e2 = new Edge("V1..V3", 3, v1, v3);
        Edge e3 = new Edge("V1..V4", 4, v1, v4);
        Edge e4 = new Edge("V2..V5", 7, v2, v5);
        Edge e5 = new Edge("V3..V5", 7, v3, v5);
        Edge e6 = new Edge("V4..V6", 9, v4, v6);
        Edge e7 = new Edge("V5..V7", 8, v5, v7);
        Edge e8 = new Edge("V5..V8", 8, v5, v8);
        Edge e9 = new Edge("V6..V8", 11, v6, v8);
        Edge e10 = new Edge("V7..V9", 17, v7, v9);
        Edge e11 = new Edge("V8..V9", 15, v8, v9);

        // Creating a graph
        DirectedGraph graph = new DirectedGraph();
        graph.AddNode(v1);
        graph.AddNode(v2);
        graph.AddNode(v3);
        graph.AddNode(v4);
        graph.AddNode(v5);
        graph.AddNode(v6);
        graph.AddNode(v7);
        graph.AddNode(v8);
        graph.AddNode(v9);

        graph.AddEdge(e1);
        graph.AddEdge(e2);
        graph.AddEdge(e3);
        graph.AddEdge(e4);
        graph.AddEdge(e5);
        graph.AddEdge(e6);
        graph.AddEdge(e7);
        graph.AddEdge(e8);
        graph.AddEdge(e9);
        graph.AddEdge(e10);
        graph.AddEdge(e11);

        // Example usage
        List<Node> successorsOfV5 = graph.GetSuccessors(v5);
        Console.WriteLine("Successors of V5:");
        foreach (Node successor in successorsOfV5)
        {
            Console.WriteLine(successor.NodeName);
        }

        List<Node> predecessorsOfV5 = graph.GetPredecessors(v5);
        Console.WriteLine("Predecessors of V5:");
        foreach (Node predecessor in predecessorsOfV5)
        {
            Console.WriteLine(predecessor.NodeName);
        }

        int rr = 1;


        Console.WriteLine();


        List<List<Node>> allPathsFromV9ToV1 = graph.FindAllPaths(v1, v9);

        Console.WriteLine("All paths from V9 to V1:");
        foreach (List<Node> path in allPathsFromV9ToV1)
        {
            foreach (Node node in path)
            {
                Console.Write(node.NodeName + " ");
            }
            Console.WriteLine("ES: " + graph.CalculateES(path));
            Console.WriteLine();
        }

    }
}
//*/