using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using GraphLibrary;

namespace GraphLibrary
{
    public static class Print
    {        
        /////////////////////////////////////////////////////////////////////////////////////////

        public  static  void    FilesInDirectory(string dirPath)
        {
            string[] names = API.GetFilesInDirectory(dirPath);

            if (names.Length > 0)
            {
                Console.WriteLine("Files in directory " + dirPath + ":");
                for (int i = 0; i < names.Length; i++)
                    Console.WriteLine(names[i]);
            }
            else
                Console.WriteLine("No files in directory");
        }

        public  static  void    AllFilesInDirectory(string dirPath)
        {
            string[] names = API.GetAllFilesInDirectory(dirPath);

            if (names.Length > 0)
            {
                Console.WriteLine("Files in directory and subdirectories " + dirPath + ":");
                for (int i = 0; i < names.Length; i++)
                    Console.WriteLine(names[i]);
            }
            else
                Console.WriteLine("No files in directory");
        }

        public  static  void    Graph(string name)
        {
            GGraph g = InternalAPI.Get(name);

            if (g.Name != "empty")
            {
                if (g.VerticesAmount > 1)
                {
                    Window present = new Window
                    {
                        Title = "Граф " + name,
                        Content = new PresentWindow(g)
                    };

                    present.ShowDialog();
                }
                else
                    Console.WriteLine("В графе {0} всего 1 вершина. Нужно 2 или более вершин для отрисовки", g.Name);
            }
        }

        public  static  void    StronglyConnectedComponents(string name)
        {
            GGraph g = InternalAPI.Get(name);

            if (g.Name != "empty")
            {
                List<GGraph> components = g.StronglyConnectedComponents();

                for (int i = 0; i < components.Count; i++)
                {
                    Console.WriteLine("Component " + (i + 1).ToString() + ": " + components[i].Name);
                    InternalAPI.Print(components[i], components[i].WeightMatrix, false);
                }
            }            
        }

        public  static  void    WayThrough(string name, string[] verticesNames)
        {
            GGraph graph = InternalAPI.Get(name);

            if (graph.Name != "empty" && verticesNames.Length > 0)
            {
                List<GVertex> verticesList = new List<GVertex>();

                for (int i = 0; i < verticesNames.Length; i++)
                {
                    GVertex v = graph.VertexByName(verticesNames[i]);

                    if (v.Name == "null")
                    {
                        Console.WriteLine("Wrong vertex name");
                        return;
                    }

                    verticesList.Add(v);
                }

                int[,] matrix = graph.AccessMatrixFor(verticesList);

                int n = (int)Math.Sqrt(matrix.Length);
                Console.Write("Матрица достижимости для путей проходящих через ");
                for (int i = 0; i < verticesList.Count; i++)
                    Console.Write(verticesNames[i] + " ");
                Console.WriteLine();
                for (int i = 0; i < n; i++)
                {
                    for (int j = 0; j < n; j++)
                        Console.Write(matrix[i, j].ToString() + " ");
                    Console.WriteLine();
                }
            }
        }
        
        public  static  void    DistanceMatrix(string name)
        {
            GGraph graph = InternalAPI.Get(name);

            InternalAPI.Print(graph, graph.DistanceMatrix);
        }

        public  static  void    AdjacencyMatrix(string name)
        {
            GGraph graph = InternalAPI.Get(name);

            InternalAPI.Print(graph, graph.AdjacencyMatrix);
        }

        public  static  void    PathMatrix(string name)
        {
            GGraph graph = InternalAPI.Get(name);

            if (graph.Name != "empty")
            {
                for (int i = 0; i < graph.VerticesAmount; i++)
                    for (int j = 0; j < graph.VerticesAmount; j++)
                        if (i != j)
                            InternalAPI.PrintPath(graph[i], graph[j], graph.PathMatrix[i, j]);
            }
        }

        public  static  void    WeightMatrix(string name)
        {
            GGraph g = InternalAPI.Get(name);

            InternalAPI.Print(g, g.WeightMatrix);
        }

        public  static  void    Diameter(string name)
        {
            GGraph g = InternalAPI.Get(name);

            if (g.Name != "empty")
            {
                int d = g.Diameter;

                Console.WriteLine("Диаметр графа {0}: {1}", name, d);
            }            
        }

        public  static  void    Radius(string name)
        {
            GGraph g = InternalAPI.Get(name);

            if (g.Name != "empty")
            {
                int r = g.Radius;

                Console.WriteLine("Радиус графа {0}: {1}", name, r);
            }          
        }

        public  static  void    Centers(string name)
        {
            GGraph g = InternalAPI.Get(name);

            if (g.Name != "empty")
            {
                List<GVertex> centers = g.Centers;

                for (int i = 0; i < centers.Count; i++)
                {
                    Console.WriteLine("{0}-й центр графа {1}: вершина {2}", i + 1, name, centers[i].Name);
                }
            }            
        }

        public  static  void    List()
        {
            List<string> list = InternalAPI.GetGraphList();

            if (list.Count > 0)
                for (int i = 0; i < list.Count; i++)
                    Console.WriteLine((i + 1).ToString() + ". " + list[i]);
            else
                Console.WriteLine("Список введённых графов пуст");
        }

        public  static  void    Last()
        {
            Graph(InternalAPI.LastGraph);
        }

        public  static  void    AdjacencyList(string name)
        {
            GGraph graph = InternalAPI.Get(name);

            if (graph.Name != "empty")
            {
                GList list = new GList(graph);
                list.Print();
            }
        }
    }
}
