using System;
using System.Diagnostics.Metrics;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace CriticalPathAnalysis;

public class DirectedGraph
{
    private List<Node> nodes;
    public List<Node> GetNodes() { return new List<Node>(nodes); }

    public DirectedGraph()
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
}