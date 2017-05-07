using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GraphLibrary;

namespace GraphLibrary
{
    public static class API
    {
        public  static  int     Amount
        {
            get
            {
                return InternalAPI.Amount;
            }
        }

        public  static  string  LastGraphName
        {
            get
            {
                return InternalAPI.LastGraph;
            }
        }

        ///////////////////////////////////////////////////////////////////////////////

        public  static  string[]    GetFilesInDirectory(string dirPath)
        {
            try
            {
                return System.IO.Directory.GetFiles(dirPath);
            }
            catch
            {
                Console.WriteLine("Error file reading");
                return new string[] {""};
            }
        }

        public  static  string[]    GetAllFilesInDirectory(string dirPath)
        {
            try
            {
                return System.IO.Directory.GetFiles(dirPath, "*.*", System.IO.SearchOption.AllDirectories);
            }
            catch
            {
                Console.WriteLine("Error file reading");
                return new string[] {""};
            }
        }

        public  static  void    AddAll(string dirPath)
        {
            string[] names = GetAllFilesInDirectory(dirPath);

            foreach (string path in names)
            {
                Add(@path);
            }
        }

        public  static  void    Add(string path)
        {
            GGraph graph = new GGraph(path);

            if (graph.Name != "empty")
            {
                if (graph.Name == "" || graph.VerticesAmount < 1)
                    Console.WriteLine("Неверный файл");
                else
                {
                    InternalAPI.Add(graph);
                }
            }
        }

        public  static  void    AddManually()
        {
            AddGraphForm form = new AddGraphForm();
            form.ShowDialog();
        }

        public  static  void    Remove(string name)
        {
            InternalAPI.Remove(name);
        }

        public  static  void    IsUndirected(string name)
        {
            GGraph graph = InternalAPI.Get(name);

            if (graph.Name != "empty")
            {
                if (graph.IsUndirected)
                    Console.WriteLine(name + " is undirected");
                else
                    Console.WriteLine(name + " is directed");
            }
        }

        public  static  void    IsConnected(string name)
        {
            GGraph graph = InternalAPI.Get(name);

            if (graph.Name != "empty")
            {
                if (graph.IsConnected)
                    Console.WriteLine(name + " is connected");
                else
                    Console.WriteLine(name + " is not connected");
            }
        }

        public  static  void    GetFactorGraph(string name)
        {
            GGraph graph = InternalAPI.GetFactorGraph(name);

            if (graph.Name != "empty")
            {
                graph.Name = "FG_" + name;
                InternalAPI.Add(graph);
            }
        }

        public  static  void    GetMST(string name)
        {
            GGraph graph = InternalAPI.Get(name);

            if (graph.Name != "empty")
            {
                if (graph.IsConnected)
                {
                    GGraph mst = Algorithms.KruskalMST(graph);
                    InternalAPI.Add(mst);
                }
                else
                    Console.WriteLine(graph.Name + " is not connected. You can`t get MST of this graph");
            }
        }

        public  static  void    Combine(string name1, string name2)
        {
            GGraph graph1 = InternalAPI.Get(name1);
            GGraph graph2 = InternalAPI.Get(name2);

            if (graph1.Name != "empty" && graph2.Name != "empty")
            {
                GGraph combined = graph1 + graph2;
                Console.WriteLine("Операция объединения завершена");
                InternalAPI.Add(combined);
            }
        }

        public  static  void    Join(string name1, string name2)
        {
            GGraph graph1 = InternalAPI.Get(name1);
            GGraph graph2 = InternalAPI.Get(name2);

            if (graph1.Name != "empty" && graph2.Name != "empty")
            {
                GGraph joined = graph1 * graph2;
                Console.WriteLine("Операция соединения завершена");
                InternalAPI.Add(joined);
            }
        }

        public  static  void    PullOff(string name1, string name2)
        {
            GGraph graph1 = InternalAPI.Get(name1);
            GGraph graph2 = InternalAPI.Get(name2);

            if (graph1.Name != "empty" && graph2.Name != "empty")
            {
                if (graph2 <= graph1)
                {
                    GGraph pulledoff = graph1 / graph2;
                    Console.WriteLine("Операция стягивания подграфа " + name2 + " в вершину завершена");
                    InternalAPI.Add(pulledoff);
                }
                else
                    Console.WriteLine("Граф " + graph2.Name + " не является подграфом " + graph1.Name);
            }
        }
    }
}
