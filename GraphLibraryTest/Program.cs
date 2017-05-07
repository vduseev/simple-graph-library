using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GraphLibrary;

namespace GraphLibraryTest
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            //GGraph graph = new GGraph("dz/dz4/TG1.g");
            ////InternalAPI.Add(graph);
            //foreach (GVertex c in graph.Centers)
            //    Console.WriteLine(c.Name);
            //Console.WriteLine("External: " + graph[graph.GetExtMedian()].Name);
            //Console.WriteLine("Internal: " + graph[graph.GetIntMedian()].Name);
            //Console.Read();

            Cmd.Run();
        }
    }
}
