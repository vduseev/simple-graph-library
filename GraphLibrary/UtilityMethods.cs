using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphLibrary
{
    public class UtilityMethods
    {
        public  static void     qSort(int[] A, int low, int high)
        {
            int i = low;
            int j = high;
            int x = A[(low + high) / 2];
            do
            {
                while (A[i] < x) ++i;
                while (A[j] > x) --j;
                if (i <= j)
                {
                    int temp = A[i];
                    A[i] = A[j];
                    A[j] = temp;
                    i++; j--;
                }
            } while (i <= j);

            if (low < j) qSort(A, low, j);
            if (i < high) qSort(A, i, high);
        }
        
        // соединение   V = V1 v V2, E = E1 v E2 v {e: соединяющие каждую вершину из V1 с вершиной из V2}
        public  static GGraph    JoinGraphs(GGraph G1, GGraph G2)
        {
            // create a graph with e = (v1, v2) : foreach v1 from V1, v2 from V2;
            GGraph g             = CombineGraphs(G1, G2);

            GGraph g1Inv         = ComplementGraph(G1);
            GGraph g2Inv         = ComplementGraph(G2);

            GGraph g1Full        = CombineGraphs(G1, g1Inv);
            GGraph g2Full        = CombineGraphs(G2, g2Inv);
            GGraph gPrepare      = CombineGraphs(g1Full, g2Full);

            GGraph gPrepareInv   = ComplementGraph(gPrepare);
            
            //combine results
            g = CombineGraphs(gPrepareInv, g);

            return g;
        }

        public  static GGraph    ComplementGraph(GGraph G1)
        {
            GGraph G2 = new GGraph(G1.vertices.Amount);
            G2.vertices.items = G1.vertices.items;

            for (int i = 0; i < G2.vertices.Amount; i++)
                for (int j = 0; j < G2.vertices.Amount; j++)
                {
                    if (G1.matrix[i, j] != 0)
                        G2.matrix[i, j] = 0;
                    else
                        G2.matrix[i, j] = 1;
                }

            return G2;
        }

        public  static GGraph    ContractSubgraph(GGraph G, GGraph g)
        {
            if (G.Includes(g))
            {
                // res = G - g + new empty Vertex
                GGraph   substr  = G.Substract(g);
                GVertices  res_v   = substr.vertices.AddVertex();
                GGraph   res     = substr.Extend(res_v);

                for (int i = 0; i < substr.vertices.Amount; i++)
                {
                    bool edge_exist = false;
                    for (int j = 0; j < G.vertices.Amount; j++)
                    {
                        for (int k = 0; k < g.vertices.Amount; k++)
                        {
                            if (G.vertices.items[j] == g.vertices.items[k])
                            {

                            }
                        }
                    }
                }
            }
            return g;
        }
    }
}
