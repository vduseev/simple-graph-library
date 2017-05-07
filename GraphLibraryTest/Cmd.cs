using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows;
using System.Windows.Forms;
using GraphLibrary;
using GraphLibrary.Tree;

namespace GraphLibraryTest
{
    public static class Cmd
    {
        public  static  char[]  Delimeters      = {' '};

        #region cw - Comand Words

        public  static  string  cwAdjacency     = "adjacency";
        public  static  string  cwAdd           = "add";
        public  static  string  cwAll           = "all";
        public  static  string  cwAccess        = "access";
        public  static  string  cwCenters       = "centers";
        public  static  string  cwClose         = "close"; // cw - comand word
        public  static  string  cwComponents    = "components";
        public  static  string  cwCombine       = "combine";
        public  static  string  cwConnected     = "connected";
        public  static  string  cwCount         = "count";
        public  static  string  cwDiameter      = "diameter";
        public  static  string  cwDistance      = "distance";
        public  static  string  cwFactor        = "factor";
        public  static  string  cwFiles         = "files";
        public  static  string  cwFor           = "for";
        public  static  string  cwFullScreen    = "fullscreen";
        public  static  string  cwGet           = "get";
        public  static  string  cwGraph         = "graph";
        public  static  string  cwHelp          = "help";
        public  static  string  cwIn            = "in";
        public  static  string  cwIs            = "is";
        public  static  string  cwJoin          = "join";
        public  static  string  cwList          = "list";
        public  static  string  cwLast          = "last";
        public  static  string  cwManually      = "manually";
        public  static  string  cwMatrix        = "matrix";
        public  static  string  cwMst           = "mst";
        public  static  string  cwPath          = "path";
        public  static  string  cwPrint         = "print";
        public  static  string  cwPullOff       = "pulloff";
        public  static  string  cwRadius        = "radius";
        public  static  string  cwRemove        = "remove";
        public  static  string  cwShow          = "show";
        public  static  string  cwStrongly      = "strongly";
        public  static  string  cwThrough       = "through";
        public  static  string  cwTree          = "tree";
        public  static  string  cwUndirected    = "undirected";
        public  static  string  cwWeight        = "weight";

        #endregion

        public  static  void    Run()
        {
            Console.WriteLine("Консоль разработки GraphLibrary");
            Console.WriteLine("Введите help, чтобы получить список доступных команд");
            Console.Title = "Консоль разработки GraphLibrary";
            
            while (true)
            {
                Console.WriteLine();
                Console.Write(">");
                string[] c = GetCommand();
                Console.WriteLine();

                #region Handle input

                #region Last cw process
                for (int i = 0; i < c.Length; i++)
                    if (c[i].ToLower() == cwLast)
                    {
                        c[i] = API.LastGraphName;
                        break;
                    }
                #endregion

                #region Keyword: ADD
                if (c[0].ToLower() == cwAdd)
                {
                    if (c.Length > 3)
                    {
                        if (c[1].ToLower() == cwAll && c[2].ToLower() == cwIn)
                        {
                            API.AddAll(c[3]);
                        }
                        else if (c[1].ToLower() == cwFactor && c[2].ToLower() == cwGraph)
                        {
                            API.GetFactorGraph(c[3]);
                        }
                    }
                    else if (c.Length > 2)
                    {
                        if (c[1].ToLower() == cwMst)
                        {
                            API.GetMST(c[2]);
                        }
                    }
                    else if (c.Length > 1)
                    {
                        if (c[1].ToLower() == cwManually)
                        {
                            API.AddManually();
                        }
                        else
                        {
                            API.Add(c[1]);
                        }
                    }
                }
                #endregion

                #region Keyword: SHOW
                else if (c[0].ToLower() == cwShow)
                {
                    if (c.Length > 4)
                    {
                        if (c[1].ToLower() == cwAll && c[2].ToLower() == cwFiles && c[3].ToLower() == cwIn)
                        {
                            Print.AllFilesInDirectory(c[4]);
                        }
                    }
                    else if (c.Length > 3)
                    {
                        if (c[1].ToLower() == cwFiles && c[2].ToLower() == cwIn)
                        {
                            Print.FilesInDirectory(c[3]);
                        }
                    }
                }
                #endregion

                #region Keyword: PRINT
                else if (c[0].ToLower() == cwPrint)
                {
                    if (c.Length > 5)
                    {
                        #region ACCESS MATRIX THROUGH
                        if (c[1].ToLower() == cwAccess && c[2].ToLower() == cwMatrix && c[4].ToLower() == cwThrough)
                        {
                            string[] names = new string[c.Length - 5];

                            for (int i = 5; i < c.Length; i++)
                                names[i - 5] = c[i];

                            Print.WayThrough(c[3], names);
                        }
                        #endregion
                    }
                    else if (c.Length > 4)
                    {
                        #region STRONGLY CONNECTED COMPONENTS
                        if (c[1].ToLower() == cwStrongly && c[2].ToLower() == cwConnected && c[3].ToLower() == cwComponents)
                        {
                            Print.StronglyConnectedComponents(c[4]);
                        }
                        #endregion
                    }
                    else if (c.Length > 3)
                    {
                        #region PATH MATRIX
                        if (c[1].ToLower() == cwPath && c[2].ToLower() == cwMatrix)
                        {
                            Print.PathMatrix(c[3]);
                        }
                        #endregion

                        #region WEIGHT MATRIX
                        else if (c[1].ToLower() == cwWeight && c[2].ToLower() == cwMatrix)
                        {
                            Print.WeightMatrix(c[3]);
                        }
                        #endregion

                        #region ADJACENCY MATRIX
                        else if (c[1].ToLower() == cwAdjacency && c[2].ToLower() == cwMatrix)
                        {
                            Print.AdjacencyMatrix(c[3]);
                        }
                        #endregion

                        #region ADJACENCY LIST
                        else if (c[1].ToLower() == cwAdjacency && c[2].ToLower() == cwList)
                        {
                            Print.AdjacencyList(c[3]);
                        }
                        #endregion

                        #region DISTANCE MATRIX
                        else if (c[1].ToLower() == cwDistance && c[2].ToLower() == cwMatrix)
                        {
                            Print.DistanceMatrix(c[3]);
                        }
                        #endregion

                        #region K WAYS
                        else if (c[1].ToLower() == "k" && c[2].ToLower() == "ways")
                        {
                            GGraph G = InternalAPI.Get(c[3]);
                            if (G.Name != "empty")
                            {
                                Algorithms.WaysLengthK(G, 3);
                            }
                        }
                        #endregion
                    }
                    else if (c.Length > 2)
                    {
                        #region CENTERS
                        if (c[1].ToLower() ==  cwCenters)
                        {
                            Print.Centers(c[2]);
                        }
                        #endregion

                        #region DIAMETER
                        else if (c[1].ToLower() == cwDiameter)
                        {
                            Print.Diameter(c[2]);
                        }
                        #endregion

                        #region RADIUS
                        else if (c[1].ToLower() == cwRadius)
                        {
                            Print.Radius(c[2]);
                        }
                        #endregion
                    }
                    else if (c.Length > 1)
                    {
                        #region LAST
                        if (c[1].ToLower() == cwLast)
                        {
                            Print.Last();
                        }
                        #endregion

                        #region LIST
                        else if (c[1].ToLower() == cwList)
                        {
                            Print.List();
                        }
                        #endregion

                        #region COUNT
                        else if (c[1].ToLower() == cwCount)
                        {
                            Console.WriteLine(API.Amount);
                        }
                        #endregion

                        else
                        {
                            Print.Graph(c[1]);
                        }
                    }
                }
                #endregion

                #region Keyword: REMOVE
                else if (c[0].ToLower() == cwRemove)
                {
                    API.Remove(c[1]);
                }
                #endregion

                #region Keyword: COMBINE
                else if (c[0].ToLower() == cwCombine)
                {
                    if (c.Length > 2)
                    {
                        API.Combine(c[1], c[2]);
                    }
                }
                #endregion

                #region Keyword: JOIN
                else if (c[0].ToLower() == cwJoin)
                {
                    if (c.Length > 2)
                    {
                        API.Join(c[1], c[2]);
                    }
                }
                #endregion

                #region Keyword: PULLOFF
                else if (c[0].ToLower() == cwPullOff)
                {
                    if (c.Length > 2)
                    {
                        API.PullOff(c[1], c[2]);
                    }
                }
                #endregion

                #region Keyword: IS
                else if (c[0].ToLower() == cwIs)
                {
                    if (c.Length > 2)
                    {
                        if (c[1].ToLower() == cwUndirected)
                        {
                            API.IsUndirected(c[2]);
                        }
                        else if (c[1].ToLower() == cwConnected)
                        {
                            API.IsConnected(c[2]);
                        }
                    }

                }
                #endregion

                #region Keyword: CLOSE
                else if (c[0].ToLower() == cwClose)
                    break;
                #endregion
                    
                #region Keyword: HELP
                else if (c[0].ToLower() == cwHelp)
                {
                    Console.WriteLine("Список команд и их описание:");
                    PrintComandHelp("- add all in 'directory'", "прочитать все файлы с раширением *g в указанной директории и добавить графы из них в список ");
                    PrintComandHelp("- add 'path'", " :: добавить граф из файла с расширением *g, находящегося по указанному адресу");
                    PrintComandHelp("- add manually", " ::открыть окно редактора, чтобы ввести граф вручную через матрицу весов");
                    PrintComandHelp("- show all files in 'directory'", " ::вывести список файлов в директории и её поддиректориях");
                    PrintComandHelp("- show files in 'directory path'", " ::вывести список файлов в директории");
                    PrintComandHelp("- print adjacency list 'name'", " ::вывести списки смежности для указанного графа ");
                    PrintComandHelp("- print acess matrix for 'name' through 'v1' 'v2' ", " ::вывести матрицу достижимости по алгоритму Уоршала");
                    PrintComandHelp("- print distance matrix 'name'", " ::вывести матрицу расстояний для указанного графа");
                    PrintComandHelp("- print adjacency matrix 'name'", " ::вывести матрицу смежности для указанного графа");
                    Console.WriteLine("- Алгоритм Флойда-Уоршала:");
                    PrintComandHelp("- print path matrix 'name'", " ::напечатать все пути в указанном графе от одной вершины к другой");
                    PrintComandHelp("- print diameter 'name'", " ::напечатать диаметр указанного графа");
                    PrintComandHelp("- print radius 'name'", " ::напечатать радиус указанного графа");
                    PrintComandHelp("- print centers 'name'", " ::напечатать центральные вершины указанного графа");
                    PrintComandHelp("- print 'name'", " ::отрисовать указанный граф");
                    PrintComandHelp("- print list", " ::вывести список добавленных графов");
                    PrintComandHelp("- print last", " ::отрисовать последний использованный граф");
                    Console.WriteLine("- Алгоритм Уоршала:");
                    PrintComandHelp("- print strongly connected components 'name'", " ::напечатать все сильно связные компоненты указанного графа ");
                    PrintComandHelp("- add factor graph 'name'", " ::добавить в список фактор граф указанного графа ");
                    Console.WriteLine("- Алгоритм Краскала:");
                    PrintComandHelp("- add mst 'name'", " ::добавить в список минимальное остовное дерево указанного графа ");
                    PrintComandHelp("- remove 'name'", " ::удалить из списка указанный граф");
                    PrintComandHelp("- combine 'name1' 'name2'", " ::соединить указанные графы и добавить полученный граф в список");
                    PrintComandHelp("- join 'name1' 'name2'", " ::объединить указанные графы и добавить полученный граф в список");
                    PrintComandHelp("- pulloff 'граф' 'подграф'", " ::стянуть подграф графа в вершину и добавить полученный граф в список");
                    PrintComandHelp("- is undirected 'name'", " ::проверить, является ли указанный граф ненаправленным");
                    PrintComandHelp("- is connected 'name'", " ::проверить, является ли указанный граф сильно связным");
                    PrintComandHelp("- close", " ::закрыть программу");
                }
                #endregion

                #region Keyword: FULLSCREEN
                else if (c[0].ToLower() == cwFullScreen)
                {
                    Console.SetWindowPosition(0, 0);
                    Console.WindowHeight = Console.LargestWindowHeight - 2;
                }
                #endregion

                #region Keyword: TREE
                else if (c[0].ToLower() == cwTree)
                {
                    BinaryTree<string> tree = new BinaryTree<string>();
                    tree.Add("v");
                    tree.Add("a");
                    tree.Add("g");
                    tree.Add("i");
                    tree.Add("z");
                    tree.Add("d");
                    tree.Add("u");
                    tree.Add("s");
                    tree.Add("e");
                    tree.Add("r");
                    tree.Add("f");
                    //tree.InorderTraversal(tree.Root);
                    
                    Console.WriteLine("Root - {0}", tree.Root.Value);
                    Queue<BinaryTreeNode<string>> vertexQueue = new Queue<BinaryTreeNode<string>>();
                    vertexQueue.Enqueue(tree.Root);
                    while (vertexQueue.Count > 0)
                    {
                        BinaryTreeNode<string> current = vertexQueue.Dequeue();
                        if (current.Left != null)
                        {
                            Console.WriteLine("Left branch of {0} is {1}", current.Value, current.Left.Value);
                            vertexQueue.Enqueue(current.Left);
                        }
                        if (current.Right != null)
                        {
                            Console.WriteLine("Right branch of {0} is {1}", current.Value, current.Right.Value);
                            vertexQueue.Enqueue(current.Right);
                        }
                    }

                    tree.Remove("g");

                    Console.WriteLine("Root - {0}", tree.Root.Value);
                    vertexQueue = new Queue<BinaryTreeNode<string>>();
                    vertexQueue.Enqueue(tree.Root);
                    while (vertexQueue.Count > 0)
                    {
                        BinaryTreeNode<string> current = vertexQueue.Dequeue();
                        if (current.Left != null)
                        {
                            Console.WriteLine("Left branch of {0} is {1}", current.Value, current.Left.Value);
                            vertexQueue.Enqueue(current.Left);
                        }
                        if (current.Right != null)
                        {
                            Console.WriteLine("Right branch of {0} is {1}", current.Value, current.Right.Value);
                            vertexQueue.Enqueue(current.Right);
                        }
                    }

                    GGraph treeGraph = Algorithms.TreeToGraph(tree);
                    //InternalAPI.Print(treeGraph, treeGraph.WeightMatrix, false);
                    
                    InternalAPI.Draw(treeGraph);
                }
                #endregion

                #region Unknown comand
                else
                {
                    Console.WriteLine("Unknown comand");
                }
                #endregion

                #endregion
            } 
        }

        private static void PrintComandHelp(string comand, string description)
        {
            Console.BackgroundColor = ConsoleColor.Cyan;
            Console.ForegroundColor = ConsoleColor.Black;
            Console.Write(comand);
            Console.ResetColor();
            Console.WriteLine(description);
            Console.WriteLine();
        }
        
        private static  string[] GetCommand()
        {   
            Console.ForegroundColor = ConsoleColor.Gray;

            string line = Console.ReadLine();

            Console.ResetColor();

            return line.Split(Cmd.Delimeters);
        }
    }
}
