using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;

namespace GraphLibrary
{
    public class GGraph
    {
        public  static  int INFINITY = 9999999;
        public  string      Name;

        private GVertices   vertices;
        private GEdge[,]    matrix;
        private int[,]      distance;
        private int[,]      adjacency;
        private GPath[,]    path;
        
        #region INTERFACE

        /// <summary>
        /// Get amount of vertices in this graph
        /// </summary>
        public  int         VerticesAmount
        {
            get
            {
                return vertices.Amount;
            }
        }

        /// <summary>
        /// Get amount of edges in this graph
        /// </summary>
        public  int         EdgesAmount
        {
            get
            {
                int amount = 0;

                for (int i = 0; i < VerticesAmount; i++)
                    for (int j = 0; j < VerticesAmount; j++)
                        if (matrix[i, j].Exist)
                            amount++;

                return amount;
            }
        }

        /// <summary>
        /// Get specified vertex
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public  GVertex     this[int i]
        {
            get
            {
                return vertices[i];
            }
        }
        
        public  GPath[,]    PathMatrix
        {
            get
            {
                return path;
            }
        }

        public  int[,]      DistanceMatrix
        {
            get
            {
                return distance;
            }
        }

        public  int[,]      AdjacencyMatrix
        {
            get
            {
                return adjacency;
            }
        }

        public  int[,]      WeightMatrix
        {
            get
            {
                int[,] result = new int[VerticesAmount, VerticesAmount];
                for (int i = 0; i < VerticesAmount; i++)
                    for (int j = 0; j < VerticesAmount; j++)
                        result[i, j] = Weight(i, j);
                return result;
            }
        }

        public  int         Weight(int i, int j)
        {
            if (!matrix[i, j].Exist)
                return INFINITY;
            else
                return matrix[i, j].Value;
        }

        public  GVertex     VertexByName(string name)
        {
            for (int i = 0; i < VerticesAmount; i++)
                if (vertices[i].Name == name)
                    return vertices[i];
            return new GVertex("null", -1);
        }

        public List<GGraph> StronglyConnectedComponents()
        {
            return Algorithms.WarshallStrongComponents(this);
        }

        public  int[,]      AccessMatrixFor(List<GVertex> list)
        {
            return Algorithms.WarshallAccessMatrixFor(this, list);
        }

        /// <summary>
        /// Get or set adjacency matrix elements by their indexes
        /// </summary>
        /// <param name="i"></param>
        /// <param name="j"></param>
        /// <returns>int</returns>
        public  GEdge this[int i, int j]
        {
            get
            {
                return matrix[i, j];
            }
            set
            {
                matrix[i, j] = value;
            }
        }

        public List<GEdge> GetEdgeList()
        {
            List<GEdge> list = new List<GEdge>();
            for (int i = 0; i < this.VerticesAmount; i++)
                for (int j = 0; j < this.VerticesAmount; j++)
                    if (this[i, j].Exist)
                        list.Add(this[i, j]);
            return list;
        }
                
        #endregion

        #region CONSTRUCTOR
        
        /// <summary>
        /// Create object from array of vertices names, formed adjacency matrix and name
        /// </summary>
        /// <param name="V"></param>
        /// <param name="matrix"></param>
        /// <param name="name"></param>
        public  GGraph(string[] V, int[,] weightMatrix, string name)
        {
            this.Name    = name;
            this.matrix  = new GEdge[V.Length, V.Length];
            this.vertices= new GVertices(V);

            for (int i = 0; i < V.Length; i++)
                for (int j = 0; j < V.Length; j++)
                    if (weightMatrix[i, j] != INFINITY)
                        matrix[i, j] = new GEdge(this[i], this[j], weightMatrix[i, j]);
                    else
                        matrix[i, j] = new GEdge(this[i], this[j]);

            distance    = Algorithms.FloydWarshallDistances(this);
            path        = Algorithms.FloydWarshallPaths(this);
            adjacency   = new int[VerticesAmount, VerticesAmount];
                for (int i = 0; i < VerticesAmount; i++)
                    for (int j = 0; j < VerticesAmount; j++)
                        if (weightMatrix[i, j] != INFINITY)
                            adjacency[i, j] = 1;
                        else
                            adjacency[i, j] = 0;
        }

        /// <summary>
        /// Constructor for subgraph creation. vertexList must be sublist of G vertices
        /// </summary>
        /// <param name="vertexList"></param>
        /// <param name="name"></param>
        public GGraph(GGraph G, List<GVertex> vertexList)
        {
            int n = vertexList.Count;

            this.Name = "sub_of_" + G.Name;
            for (int i = 0; i < n; i++)
                this.Name += "," + vertexList[i].Name;

            this.matrix = new GEdge[n, n];
            this.vertices = new GVertices(vertexList);

            for (int i = 0; i < n; i++)
                for (int j = 0; j < n; j++)
                    this.matrix[i, j] = G.matrix[ vertexList[i].Adds, vertexList[j].Adds ];

            distance    = Algorithms.FloydWarshallDistances(this);
            path        = Algorithms.FloydWarshallPaths(this);
            adjacency   = new int[VerticesAmount, VerticesAmount];
                for (int i = 0; i < VerticesAmount; i++)
                    for (int j = 0; j < VerticesAmount; j++)
                        if (matrix[i, j].Exist)
                            adjacency[i, j] = 1;
                        else
                            adjacency[i, j] = 0;
        }

        /// <summary>
        /// Create object from data file
        /// </summary>
        /// <param name="filePath"></param>
        public  GGraph(string filePath)
        {
            FileStream      stream;
            StreamReader    reader;

            try
            {
                stream  = new FileStream(filePath, FileMode.Open);
                reader  = new StreamReader(stream);

                // read amount of vertices
                string name = reader.ReadLine();
                
                char[]      delimeters = {' '};
                string[]    s_vertices = reader.ReadLine().Split(delimeters);
                
                this.Name     = name;
                this.vertices = new GVertices(s_vertices);
                this.matrix   = new GEdge[vertices.Amount, vertices.Amount];
                
                Console.WriteLine("Elements: {0}", s_vertices.Length.ToString());

                // read adjacency matrix
                for (int i = 0; i < vertices.Amount; i++)
                {
                    string[] line = reader.ReadLine().Split(delimeters);
                    Console.WriteLine("Elements in line{1}: {0}", line.Length.ToString(), i+1);


                    for (int j = 0; j < vertices.Amount; j++)
                    {
                        // read matrix element
                        if (line[j] != "~")
                            this.matrix[i, j] = new GEdge(this[i], this[j], int.Parse(line[j]));
                        else
                            this.matrix[i, j] = new GEdge(this[i], this[j]);
                    }
                }

                reader.Close();
                stream.Close();

                distance    = Algorithms.FloydWarshallDistances(this);
                path        = Algorithms.FloydWarshallPaths(this);
                this.adjacency   = new int[vertices.Amount, vertices.Amount];
                    for (int i = 0; i < vertices.Amount; i++)
                        for (int j = 0; j < vertices.Amount; j++)
                            if (this.matrix[i, j].Exist)
                                this.adjacency[i, j] = 1;
                            else
                                this.adjacency[i, j] = 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Unable to read from file: " + ex.Message);

                Name = "empty";
                vertices = new GVertices(new string[] {"empty"});
                matrix = new GEdge[1, 1];
            }
        }

        #endregion
        
        #region ATTRIBUTE FUNCTIONS

        /// <summary>
        /// Get address in which vertex belongs to by vertex name. Returns -1 if not found
        /// </summary>
        /// <param name="name"></param>
        /// <returns>int</returns>
        private int findVertexAddsByName(string name)
        {
            return vertices.GetAdds(name);
        }

        public bool IsUndirected
        {
            get
            {
                for (int i = 0; i < VerticesAmount; i++)
                    for (int j = i; j < VerticesAmount; j++)
                        if (matrix[i, j].Exist && matrix[j, i].Exist)
                        {
                            if (matrix[i, j].Value != matrix[j, i].Value)
                                return false;
                        }
                        else if (!matrix[i, j].Exist && !matrix[j, i].Exist)
                            continue;
                        else
                            return false;
                return true;
            }
        }

        /// <summary>
        /// Returns the indegree of vertex u
        /// </summary>
        /// <param name="u"></param>
        /// <returns></returns>
        public int InDegreeOf(GVertex u)
        {
            int degree = 0;

            for (int i = 0; i < this.VerticesAmount; i++)
            {
                if (this[u.Adds, i].Exist)
                {
                    degree++;
                }
            }

            return degree;
        }

        /// <summary>
        /// Returns the outdegree of vertex u
        /// </summary>
        /// <param name="u"></param>
        /// <returns></returns>
        public int OutDegreeOf(GVertex u)
        {
            int degree = 0;

            for (int i = 0; i < this.VerticesAmount; i++)
            {
                if (this[i, u.Adds].Exist)
                {
                    degree++;
                }
            }

            return degree;
        }

        /// <summary>
        /// Returns a list of the vertices adjacent to vertex u
        /// </summary>
        /// <param name="u"></param>
        /// <returns></returns>
        public List<GVertex> VerticesAdjacentTo(GVertex u)
        {
            List<GVertex> adjTo = new List<GVertex>();

            for (int i = 0; i < this.VerticesAmount; i++)
            {
                if (this[i, u.Adds].Exist)
                {
                    adjTo.Add(this.vertices[i]);
                }
            }

            return adjTo;
        }

        /// <summary>
        /// Returns a list of the vertices adjacent from vertex u
        /// </summary>
        /// <param name="u"></param>
        /// <returns></returns>
        public List<GVertex>    VerticesAdjacentFrom(GVertex u)
        {
            List<GVertex> adjFrom = new List<GVertex>();

            for (int i = 0; i < this.VerticesAmount; i++)
            {
                if (this[u.Adds, i].Exist)
                {
                    adjFrom.Add(this.vertices[i]);
                }
            }

            return adjFrom;
        }

        public List<GEdge>      EdgesAdjacentFrom(int vertexIdx)
        {
            List<GEdge> adjFrom = new List<GEdge>();

            for (int i = 0; i < this.VerticesAmount; i++)
            {
                if (this[vertexIdx, i].Exist)
                {
                    adjFrom.Add(this[i, vertexIdx]);
                }
            }

            return adjFrom;
        }

        public  List<int>       VerticesAdjacentFrom(int vertexIdx)
        {
            List<int> adjFrom = new List<int>();

            for (int i = 0; i < this.VerticesAmount; i++)
            {
                if (this[vertexIdx, i].Exist && vertexIdx != i)
                {
                    adjFrom.Add(i);
                }
            }

            return adjFrom;
        }

        /// <summary>
        /// Returns TRUE if vertices u and v are connected, FALSE otherwise
        /// </summary>
        /// <param name="u"></param>
        /// <param name="v"></param>
        /// <returns></returns>
        public bool Adjacent(GVertex u, GVertex v)
        {
            if (this[u.Adds, v.Adds].Exist)
                return true;
            return false;
        }

        private int eccentricityOf(int v)
        {
            int max = -INFINITY;
            for (int i = 0; i < VerticesAmount; i++)
                if (distance[v, i] != INFINITY && distance[v, i] > max)
                    max = distance[v, i];
            return max;
        }

        /// <summary>
        /// D(G) = max d(u,v)
        /// </summary>
        public int  Diameter
        {
            get
            {
                int max = -INFINITY;
                for (int i = 0; i < VerticesAmount; i++)
                    for (int j = 0; j < VerticesAmount; j++)
                        if (distance[i, j] != INFINITY && distance[i, j] > max)
                            max = distance[i, j];
                return max;
            }
        }

        /// <summary>
        /// R(G) = min max d(u,v)
        /// </summary>
        public int  Radius
        {
            get
            {
                int min = INFINITY;
                for (int i = 0; i < VerticesAmount; i++)
                {
                    int max = eccentricityOf(i);
                    if (max != -INFINITY && max < min)
                        min = max;
                }
                return min;
            }
        }

        public bool IsConnected
        {
            get
            {
                for (int i = 0; i < VerticesAmount; i++)
                    for (int j = 0; j < VerticesAmount; j++)
                        if (distance[i, j] == INFINITY)
                            return false;
                return true;
            }
        }

        public List<GVertex> Centers
        {
            get
            {
                List<GVertex> resultList = new List<GVertex>();

                int R = Radius;
                for (int i = 0; i < VerticesAmount; i++)
                    if (eccentricityOf(i) == R)
                        resultList.Add(this[i]);

                return resultList;
            }
        }

        public int GetExtMedian()
        {
            int[] sigmaO = new int[this.VerticesAmount];
            for (int i = 0; i < this.VerticesAmount; i++)
            {
                sigmaO[i] = GGraph.INFINITY;
            }
            for (int i = 0; i < this.VerticesAmount; i++)
            {
                for (int j = 0; j < this.VerticesAmount; j++)
                {
                    if (i != j)
                    {
                        sigmaO[i] += this.DistanceMatrix[i, j];
                    }
                }
            }
            int min1 = 0;
            for (int i = 0; i < this.VerticesAmount; i++)
            {
                if (sigmaO[i] < sigmaO[min1])
                    min1 = i;
            }
            return min1;
        }

        public int GetIntMedian()
        {
            int[] sigmaT = new int[this.VerticesAmount];
            for (int i = 0; i < this.VerticesAmount; i++)
            {
                sigmaT[i] = GGraph.INFINITY;
            }
            for (int i = 0; i < this.VerticesAmount; i++)
            {
                for (int j = 0; j < this.VerticesAmount; j++)
                {
                    if (i != j)
                    {
                        sigmaT[i] += this.DistanceMatrix[j, i];
                    }
                }
            }
            int min2 = 0;
            for (int i = 0; i < this.VerticesAmount; i++)
            {
                if (sigmaT[i] < sigmaT[min2])
                    min2 = i;
            }
            return min2;
        }

        #endregion

        #region OPERATIONS ON TWO GRAPHS

        /// <summary>
        /// Combine G1 and G2, e.g. create one graph from two. It doesn`t matter in which
        /// order you enter the arguments
        /// </summary>
        /// <param name="G1"></param>
        /// <param name="G2"></param>
        /// <returns>GGraph object</returns>
        public static GGraph operator +(GGraph G1, GGraph G2)
        {
            // combine vertices
            string[]    names   = Misc.Combine(G1.vertices, G2.vertices);

            int         N       = names.Length;
            
            int[,]      M       = new int[N, N];

            for (int i = 0; i < names.Length; i++)
            {
                for (int j = 0; j < names.Length; j++)
                {
                    int i_in_G1 = G1.findVertexAddsByName(names[i]),
                        j_in_G1 = G1.findVertexAddsByName(names[j]),
                        i_in_G2 = G2.findVertexAddsByName(names[i]),
                        j_in_G2 = G2.findVertexAddsByName(names[j]);

                    // ** -1 means that vertex with name names[i] is not found in graph G1

                    // both vertices found in G1 and G2 graphs
                    if (i_in_G1 != -1 && j_in_G1 != -1 && i_in_G2 != -1 && j_in_G2 != -1)
                    {
                        GEdge g1Edge = G1[i_in_G1, j_in_G1];
                        GEdge g2Edge = G2[i_in_G2, j_in_G2];

                        if (!g1Edge.Exist && !g2Edge.Exist)
                            M[i, j] = INFINITY;
                        else if (!g1Edge.Exist && g2Edge.Exist)
                            M[i, j] = g2Edge.Value;
                        else if (g1Edge.Exist && !g2Edge.Exist)
                            M[i, j] = g1Edge.Value;
                        else
                            M[i, j] = g1Edge.Value + g2Edge.Value;

                        //M[i, j] = Math.Max(G1[i_in_G1, j_in_G1].Value, G2[i_in_G2, j_in_G2].Value);
                        //M[i, j] = G1[i_in_G1, j_in_G1] | G2[i_in_G2, j_in_G2];
                    }
                    else if (i_in_G1 != -1 && j_in_G1 != -1)
                    {
                        GEdge g1Edge = G1[i_in_G1, j_in_G1];

                        if (!g1Edge.Exist)
                            M[i, j] = INFINITY;
                        else
                            M[i, j] = G1[i_in_G1, j_in_G1].Value;
                    }
                    else if (i_in_G2 != -1 && j_in_G2 != -1)
                    {
                        GEdge g2Edge = G2[i_in_G2, j_in_G2];

                        if (!g2Edge.Exist)
                            M[i, j] = INFINITY;
                        else
                            M[i, j] = G2[i_in_G2, j_in_G2].Value;
                    }
                    else
                    {
                        M[i, j] = INFINITY;
                    }
                }
            }

            return new GGraph(names, M, "CB_" + G1.Name + "_" + G2.Name);
        }

        /// <summary>
        /// Combine G1 and G2, e.g. create one graph from two and connect
        /// each vertex of G1 to each vertex from G2, and vice versa. 
        /// It doesn`t matter in which order you`re entering the arguments
        /// </summary>
        /// <param name="G1"></param>
        /// <param name="G2"></param>
        /// <returns>GGraph object</returns>
        public static GGraph operator *(GGraph G1, GGraph G2)
        {
            // combine vertices
            string[]    names   = Misc.Combine(G1.vertices, G2.vertices);

            int         N       = names.Length;
            
            int[,]      M       = new int[N, N];

            for (int i = 0; i < names.Length; i++)
            {
                for (int j = 0; j < names.Length; j++)
                {
                    int i_in_G1 = G1.findVertexAddsByName(names[i]),
                        j_in_G1 = G1.findVertexAddsByName(names[j]),
                        i_in_G2 = G2.findVertexAddsByName(names[i]),
                        j_in_G2 = G2.findVertexAddsByName(names[j]);

                    // ** -1 means that vertex with name as names[i] is not found in graph G1

                    // both vertices found in G1 and G2 graphs
                    if (i_in_G1 != -1 && j_in_G1 != -1 && i_in_G2 != -1 && j_in_G2 != -1)
                    {
                        GEdge g1Edge = G1[i_in_G1, j_in_G1];
                        GEdge g2Edge = G2[i_in_G2, j_in_G2];

                        if (!g1Edge.Exist && !g2Edge.Exist)
                            M[i, j] = INFINITY;
                        else if (!g1Edge.Exist && g2Edge.Exist)
                            M[i, j] = g2Edge.Value;
                        else if (g1Edge.Exist && !g2Edge.Exist)
                            M[i, j] = g1Edge.Value;
                        else
                            M[i, j] = g1Edge.Value + g2Edge.Value;

                        //M[i, j] = Math.Max(G1[i_in_G1, j_in_G1].Value, G2[i_in_G2, j_in_G2].Value);
                        //M[i, j] = G1[i_in_G1, j_in_G1] | G2[i_in_G2, j_in_G2];
                    }
                    else if (i_in_G1 != -1 && j_in_G1 != -1)
                    {
                        GEdge g1Edge = G1[i_in_G1, j_in_G1];

                        if (!g1Edge.Exist)
                            M[i, j] = INFINITY;
                        else
                            M[i, j] = G1[i_in_G1, j_in_G1].Value;
                    }
                    else if (i_in_G2 != -1 && j_in_G2 != -1)
                    {
                        GEdge g2Edge = G2[i_in_G2, j_in_G2];

                        if (!g2Edge.Exist)
                            M[i, j] = INFINITY;
                        else
                            M[i, j] = G2[i_in_G2, j_in_G2].Value;
                    }
                    else
                    {
                        M[i, j] = 1;
                    }
                }
            }

            return new GGraph(names, M, "JN_" + G1.Name + "_" + G2.Name);
        }

        /// <summary>
        /// Returns true if G1 and G2 have same vertices and same edges
        /// </summary>
        /// <param name="G1"></param>
        /// <param name="G2"></param>
        /// <returns>True if G1 == G2</returns>
            public static bool  operator ==(GGraph G1, GGraph G2)
        {
            if (G1.vertices == G2.vertices)
            {
                for (int i = 0; i < G1.VerticesAmount; i++)
                    for (int j = 0; j < G1.VerticesAmount; j++)
                    {
                        if (G1[i, j].Exist && G2[i, j].Exist)
                        {
                            if (G1[i, j].Value != G2[i, j].Value)
                                return false;
                        }
                        else if (G1[i, j].Exist ^ G2[i, j].Exist)
                            return false;
                    }
                return true;
            }
            else
                return false;
        }

        /// <summary>
        /// Returns true if G1 and G2 have different vertices or different edges
        /// </summary>
        /// <param name="G1"></param>
        /// <param name="G2"></param>
        /// <returns></returns>
        public static bool  operator !=(GGraph G1, GGraph G2)
        {
            if (G1.vertices == G2.vertices)
            {
                for (int i = 0; i < G1.VerticesAmount; i++)
                    for (int j = 0; j < G1.VerticesAmount; j++)
                        if (G1[i, j].Exist && G2[i, j].Exist)
                        {
                            if (G1[i, j].Value != G2[i, j].Value)
                                return true;
                        }
                        else if (!G1[i, j].Exist && !G2[i, j].Exist)
                            continue;
                        else
                            return true;
                return false;
            }
            else
                return true;
        }

        /// <summary>
        /// Checks if g is a subgraph of G.
        /// Returns true if G contains all vertices and edges from g.
        /// </summary>
        /// <param name="g"></param>
        /// <param name="G"></param>
        /// <returns></returns>
        public static bool operator <=(GGraph g, GGraph G)
        {
            if (g.vertices <= G.vertices)
            {
                for (int i = 0; i < g.VerticesAmount; i++)
                    for (int j = 0; j < g.VerticesAmount; j++)
                    {
                        int i_in_G = G.findVertexAddsByName(g.vertices[i].Name),
                            j_in_G = G.findVertexAddsByName(g.vertices[j].Name);

                        if (G[i_in_G, j_in_G] != g[i, j])
                            return false;
                    }
                return true;
            }
            else
                return false;
        }

        /// <summary>
        /// Checks if g is a subgraph of G.
        /// Returns true if G contains all vertices and edges from g.
        /// </summary>
        /// <param name="G"></param>
        /// <param name="g"></param>
        /// <returns></returns>
        public static bool operator >=(GGraph G, GGraph g)
        {
            if (G.vertices >= g.vertices)
            {
                for (int i = 0; i < g.VerticesAmount; i++)
                    for (int j = 0; j < g.VerticesAmount; j++)
                    {
                        int i_in_G = G.findVertexAddsByName(g.vertices[i].Name),
                            j_in_G = G.findVertexAddsByName(g.vertices[j].Name);

                        if (G[i_in_G, j_in_G] != g[i, j])
                            return false;
                    }
                return true;
            }
            else
                return false;
        }

        /// <summary>
        /// Pull off subgraph g of graph G to one vertex
        /// with new name which doesn`t exist amont vertices of G
        /// </summary>
        /// <param name="G"></param>
        /// <param name="g"></param>
        /// <returns></returns>
        public static GGraph operator /(GGraph G, GGraph g)
        {
            int         N       = G.VerticesAmount - g.VerticesAmount + 1;
            int[,]      A       = new int[N, N];
            string[]    names   = new string[N];
            string[]    Gnames  = new string[G.VerticesAmount];
            
            for (int i = 0; i < G.VerticesAmount; i++)
                Gnames[i] = G[i].Name;

            // forming substracted vertices
            int k = 0;
            for (int i = 0; i < G.VerticesAmount; i++)
            {
                if (!g.vertices.Contain(G.vertices[i]))
                    names[k++] = G[i].Name;
            }

            // find new name for additional vertex
            string v        = UnusedName(Gnames);
            // add new vertex to names
            names[N - 1]    = v;
            // sort vertices
            names           = Misc.SortStrings(names);

            // 1. array stores all vertices from G.
            // 2. since 'names' array <= 'Gnames' array
            // 3. look for every vertex from G in 'names' array
            // 4. and of course a many of vertices from G does not exist in 'names' array
            // so they get -1 address
            // ->
            // That means, this array represents address of each vertex from G in 'names' array
            int[] v_from_G_in_A= new int[G.VerticesAmount];
            for (int i = 0; i < G.VerticesAmount; i++)
            {
                bool    found       = false;
                int     foundIdx    = 0;

                for (int j = 0; j < N; j++)
                    if (names[j] == G.vertices[i].Name)
                    {
                        found       = true;
                        foundIdx    = j;
                        break;
                    }
                
                if (!found)
                    v_from_G_in_A[i] = -1;
                else
                    v_from_G_in_A[i] = foundIdx;
            }
            int v_in_V = 0;
            for (int i = 0; i < N; i++)
                if (names[i] == v)
                {
                    v_in_V = i;
                    break;
                }

            // This array represents addres of each vertex from G in small subgraph 'g'.
            int[] g_adds    = new int[G.VerticesAmount];
            for (int i = 0; i < G.VerticesAmount; i++)
                g_adds[i]   = g.findVertexAddsByName(G.vertices[i].Name);

            // fill whole feature graph with zeros
            // for cases which are not covered with algorithm
            for (int i = 0; i < N; i++)
                for (int j = 0; j < N; j++)
                    A[i, j] = INFINITY;
            
            // fill new graph`s weight matrix with the following algorithm
            // ... hard to explain in two rows ...
            for (int i = 0; i < G.VerticesAmount; i++)
                for (int j = 0; j < G.VerticesAmount; j++)
                {
                    // there is no vertex[j] from 'G' in 'g' subgraph
                    // there is vertex[i] in 'g' instead
                    if (g_adds[i] == -1 && g_adds[j] != -1)
                    {
                        // if some edge from vertex[i] to vertex[j] exist
                        // that means, it is output edge from subgraph 'g' to graph 'G'
                        if (G[i, j].Exist)
                        {
                            if (A[ v_from_G_in_A[i], v_in_V ] == INFINITY)
                                A[ v_from_G_in_A[i], v_in_V ] = G[i, j].Value;
                            else
                                A[ v_from_G_in_A[i], v_in_V ] += G[i, j].Value;
                        }
                    }
                    // reverse: there is column vertex, but no row vertex in 'g'
                    else if (g_adds[i] != -1 && g_adds[j] == -1)
                    {
                        if (G[i, j].Exist)
                        {
                            if (A[ v_in_V, v_from_G_in_A[j] ] == INFINITY)
                                A[ v_in_V, v_from_G_in_A[j] ] = G[i, j].Value;
                            else
                                A[ v_in_V, v_from_G_in_A[j] ] += G[i, j].Value;
                        }
                    }
                    // totally edge from 'G'
                    else if (g_adds[i] == -1 && g_adds[j] == -1)
                    {
                        if (G[i, j].Exist)
                            A[ v_from_G_in_A[i], v_from_G_in_A[j] ] = G[i, j].Value;
                        else
                            A[ v_from_G_in_A[i], v_from_G_in_A[j] ] = INFINITY;
                    }
                }
            
            return new GGraph(names, A, "PO_" + G.Name + "_" + g.Name);
        }

        /// <summary>
        /// Find unused shortest possible name in the array.
        /// </summary>
        /// <param name="names"></param>
        /// <returns></returns>
        private static string UnusedName(string[] names)
        {
            int     i      = 97;
            string  name   = (122).ToString();

            // find unused char symbol
            while (i <= 122)
            {
                string s = new string(new char[] {(char)i});
                
                bool found = false;
                for (int j = 0; j < names.Length; j++)
                    if (names[j] == s)
                    {
                        found = true;
                        break;
                    }

                if (!found)
                {
                    name = s;
                    break;
                }
                else
                    i++;
            }

            return name;
        }

        #endregion
    }
}
