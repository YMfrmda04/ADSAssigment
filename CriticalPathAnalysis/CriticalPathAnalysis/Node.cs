using System;

namespace CriticalPathAnalysis
{
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
}