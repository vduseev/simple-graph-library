using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphLibrary
{
    internal static class Misc
    {
        /// <summary>
        /// Compare two string
        /// </summary>
        /// <param name="s1"></param>
        /// <param name="s2"></param>
        /// <returns>true if s1 less then s2</returns>
        public static bool StringIsLess(string s1, string s2)
        {
            if (s1.Length < s2.Length)
            {
                return true;
            }
            else if (s1.Length == s2.Length)
            {
                for (int i = 0; i < s1.Length; i++)
                {
                    if (s1[i] < s2[i])
                        return true;

                    else if (s1[i] == s2[i])
                        continue;

                    else
                        return false;
                }
            }

            return false;
        }

        //public static GList DFS()
        //{

        //}

        /// <summary>
        /// Combine two arrays of vertices and sort their names in ascending order
        /// Returns array of combined names
        /// </summary>
        /// <param name="V1"></param>
        /// <param name="V2"></param>
        /// <returns>string[]</returns>
        public static string[] Combine(GVertices V1, GVertices V2)
        {
            string[] combined = new string[V1.Amount + V2.Amount];
            for (int i = 0; i < V1.Amount; i++)
                combined[i] = V1[i].Name;

            int k = V1.Amount;
            for (int i = 0; i < V2.Amount; i++)
            {
                bool found = false;
                for (int j = 0; j < V1.Amount; j++)
                {
                    if (V1[j].Name == V2[i].Name)
                    {
                        found = true;
                        break;
                    }
                }

                if (!found)
                    combined[k++] = V2[i].Name;
            }

            string[] names = new string[k];
            for (int i = 0; i < k; i++)
            {
                names[i] = combined[i];
            }   
            
            return Misc.SortStrings(names);
        }

        /// <summary>
        /// Sort string[] in ascending order
        /// </summary>
        /// <param name="lines"></param>
        /// <returns></returns>
        public static string[] SortStrings(string[] lines)
        {
            string[] result = new string[lines.Length];
            for (int i = 0; i < lines.Length; i++)
                result[i] = lines[i];

            // selection sort
            for (int i = 0; i < result.Length - 1; i++)
            {
                int min = i;
                for (int j = i + 1; j < result.Length; j++)
                    if (StringIsLess(result[j], result[min]))
                        min = j;

                string tmp  = result[i];
                result[i]    = result[min];
                result[min]  = tmp;
            }

            return result;
        }

        /// <summary>
        /// Q Sort algorithm
        /// </summary>
        /// <param name="A"></param>
        /// <param name="low"></param>
        /// <param name="high"></param>
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
    }
}
