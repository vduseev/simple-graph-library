using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Forms;
using System.Threading.Tasks;

namespace GraphLibrary
{
    public static class InternalAPI
    {
        private static  List<GGraph>   items  = new List<GGraph>();
        
        public  static  string  LastGraph = "empty";

        public  static  void    PrintPath(GVertex from, GVertex to, GPath path, bool printEdgeValue = false)
        {
            if (path.Exist)
            {
                Console.Write("Путь <" + from.Name + ", " + to.Name + ">, длина " + path.Amount.ToString() + ": ");

                for (int i = 0; i < path.Amount; i++)
                {
                    string str = path.Edges[i].U.Name;

                    if (printEdgeValue)
                        str +=  " - " + path.Edges[i].Value.ToString();

                    str += " → ";

                    Console.Write(str);
                }

                Console.WriteLine(path.Edges[path.Amount - 1].V.Name);
            }
            else
                Console.WriteLine("Путь <" + from.Name + ", " + to.Name + ">, не существует");
        }

        public  static  int     Amount
        {
            get
            {
                return items.Count;
            }
        }

        public  static  void    Print(GGraph g, int[,] matrix, bool rememberLast = true)
        {
            if (g.Name != "empty")
            {
                if (rememberLast)
                    LastGraph = g.Name;

                Console.Write("  ");
                for (int i = 0; i < g.VerticesAmount; i++)
                    Console.Write(g[i].Name + " ");
                Console.WriteLine();
                
                for (int i = 0; i < g.VerticesAmount; i++)
                {
                    Console.Write(g[i].Name + " ");
                    for (int j = 0; j < g.VerticesAmount; j++)
                        Console.Write(
                            (
                            (matrix[i, j] != GGraph.INFINITY) 
                            ? matrix[i, j].ToString() : "~") + " ");
                    Console.WriteLine();
                }
                Console.WriteLine();
            }
        }

        public  static  void    Draw(GGraph g)
        {
            if (g.Name != "empty")
            {
                if (g.VerticesAmount > 1)
                {
                    Window present = new Window
                    {
                        Title = "Граф " + g.Name,
                        Content = new PresentWindow(g)
                    };

                    present.ShowDialog();
                }
                else
                    Console.WriteLine("В графе {0} всего 1 вершина. Нужно 2 или более вершин для отрисовки", g.Name);
            }
        }

        public  static  GGraph  GetEmptyGraph()
        {
            return new GGraph(new string[] {""}, new int[,] {{0}}, "empty");
        }

        public  static  bool    GraphExistInList(GGraph graph)
        {
            for (int i = 0; i < Amount; i++)
            {
                if (items[i] == graph)
                {
                    return true;
                }
            }

            return false;
        }

        public  static  void    Add(GGraph graph)
        {
            if (!GraphExistInList(graph))
            {
                items.Add(graph);
                LastGraph = graph.Name;

                Console.WriteLine("Успешно добавлен граф: " + graph.Name);
            }
            else
                Console.WriteLine("Граф " + graph.Name + " уже существует в списке");
        }
        
        public  static  GGraph  Get(string name)
        {
            for (int i = 0; i < API.Amount; i++)
                if (items[i].Name == name)
                    return items[i];

            Console.WriteLine("Не существует графа с именем " + name, "Ошибка в операции Get(string name)");
            return GetEmptyGraph();
        }

        public  static  void    Remove(string name)
        {
            for (int i = 0; i < Amount; i++)
                if (items[i].Name == name)
                {
                    items.RemoveAt(i);
                    return;
                }
            Console.WriteLine("Не существует графа с таким именем");
        }

        public  static  GGraph  GetFactorGraph(string name)
        {
            GGraph graph = Get(name);

            if (graph.Name != "empty")
            {
                List<GGraph> components = graph.StronglyConnectedComponents();

                if (components.Count > 0)
                {
                    GGraph result = graph;

                    foreach (GGraph g in components)
                    {
                        result = result / g;
                        //Print(result, result.WeightMatrix);
                    }

                    return result;
                }
                else
                    Console.WriteLine("No strongly connected components in " + graph.Name);
            }

            return GetEmptyGraph();
        }

        public  static  List<string>    GetGraphList()
        {
            List<string> list = new List<string>();
            
            foreach (GGraph graph in items)
                list.Add(graph.Name);

            return list;
        }
        
    }
}
