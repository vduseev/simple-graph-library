using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GraphLibrary.Tree;

namespace GraphLibrary
{
    public class Algorithms
    {
        // G must be connected
        public  static  GGraph          KruskalMST(GGraph G)
        {
            int n = G.VerticesAmount;

            int[,] resultEdges = new int[n, n];
            for (int i = 0; i < n; i++)
                for (int j = 0; j < n; j++)
                    resultEdges[i, j] = GGraph.INFINITY;

            // сформировать список ребёр графа G в возрастающем порядке
            // берём только правый верхний треугольник матрицы (т.к. граф ненаправленный, связный)
            List<GEdge> edgeList = new List<GEdge>();
            for (int i = 0; i < n; i++)
                for (int j = i; j < n; j++)
                    if (G[i, j].Exist)
                        edgeList.Add(G[i, j]);            
            int low = 0, high = edgeList.Count - 1;            
            QSort(ref edgeList, low, high);

            // будем выбирать и вставлять ребра пока не достинем значения n-1, как в остовном дереве
            bool[] addedVertices = new bool[n];
            for (int i = 0; i < n; i++) addedVertices[i] = false;
            int step = 0;
            int count = 0;
            while (step < n - 1 && edgeList.Count > 0)
            {
                count++;
                // копируем ребро
                GEdge edge = edgeList[0];
                // удаляем его из списка
                edgeList.RemoveAt(0);

                // проверяем не относятся ли вершины, соединяемые этим ребром, к уже присоединённым вершинам
                int rowIndex = edge.U.Adds,
                    colIndex = edge.V.Adds;
                // если одна из вершин уже добавлена, то идём к следующему ребру-кандидату
                if (addedVertices[colIndex] == true && addedVertices[rowIndex] == true)
                    continue;
                else
                {
                    // помечаем новые вершины, как добавленные
                    addedVertices[rowIndex] = true;
                    addedVertices[colIndex] = true;
                }

                // вставляем ребро в новый граф
                resultEdges[rowIndex, colIndex] = edge.Value;
                resultEdges[colIndex, rowIndex] = edge.Value;

                // вставили ребро, инкримент
                step++;
            }

            // формируем массив имён вершин G для инициализации нового графа
            string[] names = new string[n];
            for (int i = 0; i < n; i++) names[i] = G[i].Name;

            return new GGraph(names, resultEdges, "MST_" + G.Name);
        }

        private static  void            QSort(ref List<GEdge> edgeList, int low, int high)
        {
            int i = low;
            int j = high;
            int x = edgeList[(low + high) / 2].Value;
            do
            {
                while (edgeList[i].Value < x) ++i;
                while (edgeList[j].Value > x) --j;
                if (i <= j)
                {
                    GEdge temp = edgeList[i];
                    edgeList[i] = edgeList[j];
                    edgeList[j] = temp;
                    i++; j--;
                }
            } while (i <= j);

            if (low < j) QSort(ref edgeList, low, j);
            if (i < high) QSort(ref edgeList, i, high);
        }

        public  static  void            AStarPath(GGraph G, int startVertexIndex)
        {
            int n = G.VerticesAmount;
            int[] flags = new int[n]; // список меток для вершин
            for (int i = 0; i < n; i++)
                flags[i] = -1; // все вершины не посещены

            List<int> list = new List<int>(); // список непосещённых вершин
            list.Add(startVertexIndex);

            while (list.Count > 0) // пока есть вершины в списке для посещения
            {
                for (int i = 0; i < list.Count; i++) // для каждой вершины из списка
                {
                    int vertexIdx = list[i];
                    List<int> adj = G.VerticesAdjacentFrom(vertexIdx); // список вершин смежных с ней

                    for (int j = 0; j < adj.Count; j++) // добавим все вершины из смежных в список непосещённых
                    {
                        //for (int k = 0; k < 
                    }
                }
            }
        }

        public  static  List<GGraph>    WarshallStrongComponents(GGraph G)
        {
            int n = G.VerticesAmount;
            int[,] B = G.AdjacencyMatrix;
            
            // B = E | M
            for (int i = 0; i < n; i++)
                for (int j = 0; j < n; j++)
                    if (i == j)
                        B[i, j] = B[i, j] | 1;
                    else
                        B[i, j] = B[i, j] | 0;

            // B(l)_ij = B(l-1)_ij | B(l-1)_il & B(l-1)_lj
            for (int k = 0; k < n; k++)
                for (int i = 0; i < n; i++)
                    for (int j = 0; j < n; j++)
                        B[i, j] = B[i, j] | B[i, k] & B[k, j];
#if DEBUG
            //Console.WriteLine("Матрица достижимости T(graph)");
            //for (int i = 0; i < n; i++)
            //{
            //    for (int j = 0; j < n; j++)
            //        Console.Write(B[i, j].ToString() + " ");
            //    Console.WriteLine();
            //}
            //Console.WriteLine();
#endif
            int[,] S = new int[n, n];
            for (int i = 0; i < n; i++)
                for (int j = 0; j < n; j++)
                {
                    S[i, j] = B[i, j] & B[j, i];
                    //if (B[i, j] == 1 && B[j, i] == 1)
                        //Console.WriteLine("{0}, {1} == 1, S[{0}, {1}] = {2}", i, j, S[i, j]);
                }
#if DEBUG
            //Console.WriteLine();
            //Console.WriteLine("Матрица сильной связности S(graph)");
            //for (int i = 0; i < n; i++)
            //{
            //    for (int j = 0; j < n; j++)
            //        Console.Write(S[i, j].ToString() + " ");
            //    Console.WriteLine();
            //}
            //Console.WriteLine();
#endif
            bool[] flags = new bool[n];
            for (int i = 0; i < n; i++)
                flags[i] = false;
            
            int markedRowsCount = 0;

            List<GGraph> result = new List<GGraph>();

            while (markedRowsCount < n)
            {
                List<GVertex> vertexList = new List<GVertex>();

                for (int i = 0; i < n; i++)
                {
                    if (flags[i] == false)
                    {
                        vertexList.Add(G[i]);

                        flags[i] = true;
                        markedRowsCount++;

                        for (int j = 0; j < n; j++)
                            if (j != i)
                                if (S[i, j] == 1)
                                {
                                    vertexList.Add(G[j]);

                                    flags[j] = true;
                                    markedRowsCount++;
                                }

                        result.Add(new GGraph(G, vertexList));

                        break;
                    }
                }
            }

            return result;
        }

        public static void WaysLengthK(GGraph G, int K)
        {
            int[,] M = G.AdjacencyMatrix;
            int[,] C = G.AdjacencyMatrix;

            for (int i = 0; i < K; i++)
            {
                C = SquadMatrixMultiply(M, C);
            }

            for (int i = 0; i < G.VerticesAmount; i++)
            {
                for (int j = 0; j < G.VerticesAmount; j++)
                    Console.Write(C[i, j].ToString() + " ");
                Console.WriteLine();
            }
        }

        public static int[,] SquadMatrixMultiply(int[,] A, int[,] B)
        {
            int n = (int)Math.Sqrt(A.Length);
            int[,] C = new int[n, n];
            for (int i = 0; i < n; i++)
                for (int j = 0; j < n; j++)
                {
                    int c = 0;
                    for (int k = 0; k < n; k++)
                        c = c | (A[i, k] & B[k, j]);
                    C[i, j] = c;
                }
            return C;
        }

        public  static  int[,]          WarshallAccessMatrixFor(GGraph G, List<GVertex> verticesList)
        {
            int n = G.VerticesAmount;
            int[,] B = G.AdjacencyMatrix;
            
            // B = E | M
            for (int i = 0; i < n; i++)
                B[i, i] = B[i, i] | 1;
            
            for (int m = 0; m < verticesList.Count; m++)
                for (int i = 0; i < n; i++)
                    for (int j = 0; j < n; j++)
                        B[i, j] = B[i, j] | B[i, verticesList[m].Adds] & B[verticesList[m].Adds, j];

            for (int i = 0; i < n; i++)
                B[i, i] = 0;

            //for (int i = 0; i < n; i++)
            //    for (int j = 0; j < n; j++)
            //        B[i, j] = ~(B[i, j] & G.AdjacencyMatrix[i, j]);

            return B;
        }

        public  static  GPath[,]        FloydWarshallPaths(GGraph G)
        {
            int [,] distanceMatrix  = new int[G.VerticesAmount, G.VerticesAmount];
            int [,] next            = new int[G.VerticesAmount, G.VerticesAmount];
 
            // initialize distance matrix
            for (int i = 0; i < G.VerticesAmount; i++)
                for (int j = 0; j < G.VerticesAmount; j++)
                {
                    next[i, j] = GGraph.INFINITY;
                    
                    if (i == j) 
                        distanceMatrix[i, j] = 0;
                    else if (!G[i, j].Exist)
                        distanceMatrix[i, j] = GGraph.INFINITY;
                    else
                        distanceMatrix[i, j] = G[i, j].Value;
                }
            
            for (int k = 0; k < G.VerticesAmount; k++)
                for (int i = 0; i < G.VerticesAmount; i++)
                    for (int j = 0; j < G.VerticesAmount; j++)
                    {
                        if (distanceMatrix[i, k] + distanceMatrix[k, j] < distanceMatrix[i, j])
                        {
                            distanceMatrix[i, j] = distanceMatrix[i, k] + distanceMatrix[k, j];
                            next[i, j] = k;
                        }
                    }

            GPath[,] paths = new GPath[G.VerticesAmount, G.VerticesAmount];

            //Console.WriteLine("Next matrix for " + G.Name + " graph:");
            for (int i = 0; i < G.VerticesAmount; i++)
            {
                for (int j = 0; j < G.VerticesAmount; j++)
                {
                    paths[i, j] = RPR(G, i, j, distanceMatrix, next);
                    //Console.Write((next[i, j] == GGraph.INFINITY) ? "~ " : next[i, j].ToString() + " ");
                }
                //Console.WriteLine();
            }

            return paths;
        }

        public  static  int[,]          FloydWarshallDistances(GGraph G)
        {
            int [,] distanceMatrix  = new int[G.VerticesAmount, G.VerticesAmount];
 
            // initialize distance matrix
            for (int i = 0; i < G.VerticesAmount; i++)
                for (int j = 0; j < G.VerticesAmount; j++)
                {
                    if (i == j)
                        distanceMatrix[i, j] = 0;
                    else if (!G[i, j].Exist)
                        distanceMatrix[i, j] = GGraph.INFINITY;
                    else
                        distanceMatrix[i, j] = G[i, j].Value;
                }
            
            for (int k = 0; k < G.VerticesAmount; k++)
                for (int i = 0; i < G.VerticesAmount; i++)
                    for (int j = 0; j < G.VerticesAmount; j++)
                    {
                        if (distanceMatrix[i, k] + distanceMatrix[k, j] < distanceMatrix[i, j])
                        {
                            distanceMatrix[i, j] = distanceMatrix[i, k] + distanceMatrix[k, j];
                        }
                    }

            return distanceMatrix;
        }
        
        private static  GPath           RPR( // Recursive Path Reconstruction
            GGraph G, int from, int to, int[,] distanceMatrix, int[,] nextIndices)
        {
            try
            {
                // there is no path between this vertices
                if (distanceMatrix[from, to] == GGraph.INFINITY || from == to)
                {
                    return new GPath();
                }

                // there is edge from 'from' vertex to 'to' vertex
                int intermediate = nextIndices[from, to];
                if (intermediate == GGraph.INFINITY)
                {
                    return new GPath(G[from, to]);
                }
                else
                {
                    GPath leftPart  = RPR(G, from, intermediate, distanceMatrix, nextIndices);
                    GPath rightPart = RPR(G, intermediate, to, distanceMatrix, nextIndices);

                    // there is no path
                    if (!leftPart.Exist || !rightPart.Exist)
                        return new GPath();
                    // two parts with 1 or more edges inside
                    else
                        return leftPart + rightPart;
                        //return GPath.Summ(leftPart, rightPart);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new GPath();
            }
        }

        public static GGraph TreeToGraph(Tree.BinaryTree<string> tree)
        {            
            // amount of nodes
            int         n       = tree.Count;

            // empty arrays
            string[]    names   = new string[n];
            int[,]      matrix  = new int[n, n];

            // null weights of vertices
            for (int i = 0; i < n; i++)
                for (int j = 0; j < n; j++)
                    matrix[i, j] = GGraph.INFINITY;

            int currentStep = 0;

            Queue<BinaryTreeNode<string>>   vertexQueue = new Queue<BinaryTreeNode<string>>();
            Queue<int>                      sourceQueue = new Queue<int>();

            vertexQueue.Enqueue(tree.Root);
            sourceQueue.Enqueue(0);

            while (vertexQueue.Count > 0)
            {
                BinaryTreeNode<string>  current = vertexQueue.Dequeue();
                int                     source  = sourceQueue.Dequeue();

                if (source != currentStep)
                {
                    matrix[source, currentStep] = 1;
                }

                names[currentStep] = current.Value;

                if (current.Left != null)
                {
                    vertexQueue.Enqueue(current.Left);
                    sourceQueue.Enqueue(currentStep);
                }
                if (current.Right != null)
                {
                    vertexQueue.Enqueue(current.Right);
                    sourceQueue.Enqueue(currentStep);
                }

                currentStep++;
            }

            return new GGraph(names, matrix, "BT_" + names[0].ToUpper() + names[n - 1].ToUpper());
        }

        public static void UNT(GGraph G)
        {
            int n = G.VerticesAmount;

            List<List<int>> combinations = GetVertexCombinations(G);
            
            int[,] dist = G.DistanceMatrix;

            // для каждого подмножества Xp проверить минимальность
            foreach (List<int> comb in combinations)
            {
                // 
                int min = GGraph.INFINITY;
                int[] d_Xp_xj = new int[n];
                foreach (int xi in comb)
                {
                    for (int j = 0; j < n; j++)
                    {
                        
                        if (dist[xi, j] < min)
                            min = dist[xi, j];
                    }
                }
                int[] d_xj_Xp = new int[n];

            }
        }

        static List<List<int>> GetVertexCombinations(GGraph G)
        {
            int n = G.VerticesAmount;

            List<List<int>> vertexCombinations = new List<List<int>>();

            for (int i = 0; i < (2 << n); i++)
            {
                int y = i;
                int j = 1;

                List<int> combination = new List<int>();

                while (y > 0)
                {
                    int x = y;
                    y = y / 2;

                    if ((x - 2 * y) == 1)
                       combination.Add(j);
                    j++;
                }

                vertexCombinations.Add(combination);
            }

            return vertexCombinations;
        }
    }
}
