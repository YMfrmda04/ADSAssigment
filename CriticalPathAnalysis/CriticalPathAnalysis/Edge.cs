using System;
namespace CriticalPathAnalysis
{
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
}