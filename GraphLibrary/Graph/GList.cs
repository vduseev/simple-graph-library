using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphLibrary
{
    public class GNode : GVertex
    {
        public List<GNode>  AdjacencyNodes;

        public GNode(string name, int adds) : base(name, adds)
        {
            AdjacencyNodes = new List<GNode>();
        }
    }
    
    public class GList
    {
        public List<GNode>  Nodes;

        public GList(GGraph G)
        {
            Nodes = new List<GNode>();

            for (int i = 0; i < G.VerticesAmount; i++)
            {
                // create node
                GNode node  = new GNode(G[i].Name, G[i].Adds);   
                Nodes.Add(node);
            }

            for (int i = 0; i < G.VerticesAmount; i++)
            {
                // create a list of output adjacency nodes for every node
                for (int j = 0; j < G.VerticesAmount; j++)
                {
                    if (G[i, j].Exist)
                    {
                        Nodes[i].AdjacencyNodes.Add(Nodes[j]);
                    }
                }
            }
        }

        public void Print()
        {
            for (int i = 0; i < Nodes.Count; i++)
            {
                Console.Write(Nodes[i].Name + " -> ");

                foreach (GNode node in Nodes[i].AdjacencyNodes)
                {
                    Console.Write(node.Name + " ");
                }
                Console.WriteLine();
            }
        }
    }
}
