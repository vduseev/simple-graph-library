using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphLibrary
{
    /// <summary>
    /// REMEMBER: Graph vertices can`t exist without graph
    /// Each vertex contain address of column/row in adjacency matrix
    /// </summary>
    public class GVertices
    {
        private List<GVertex> items;

        // <> get vertices amount               
        // <> get vertex by num in collection   [int]
        // <> get vertex address by it`s name   (string)
        #region INTERFACE

        /// <summary>
        /// Get vertices amount
        /// </summary>
        public int Amount
        {
            get
            {
                return items.Count;
            }
        }

        /// <summary>
        /// Get or set vertex by it`s index
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public GVertex this[int i]
        {
            get
            {
                GVertex v = items[i];
                return v;
            }
        }
                
        /// <summary>
        /// Get vertex address by it`s name.
        /// returns -1 if not found
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public int GetAdds(string name)
        {
            for (int i = 0; i < this.Amount; i++)
            {
                if (this[i].Name == name)
                    return this[i].Adds;
            }

            return -1;
        }
        
        #endregion

        // # create member from array of names
        // # create member from array of vertices
        #region CONSTRUCTOR
        
        /// <summary>
        /// Create object from array of names.
        /// Each vertex gets address according to it`s index in array
        /// </summary>
        /// <param name="names"></param>
        public GVertices(string[] names)
        {
            items   = new List<GVertex>();

            for (int i = 0; i < names.Length; i++)
            {
                items.Add(new GVertex(names[i], i));
            }
        }

        /// <summary>
        /// Create object from array of vertices
        /// </summary>
        /// <param name="vertices"></param>
        public GVertices(List<GVertex> vertices)
        {
            this.items  = vertices;
        }

        #endregion

        // : operator ==
        // : operator !=
        // : operator <=
        // : operator >=
        #region OPERATORS

        public static bool operator ==(GVertices V1, GVertices V2)
        {
            if (V1.Amount != V2.Amount)
                return false;
            else
            {
                bool equal = true;
                for (int i = 0; i < V1.Amount; i++)
                    if (V1[i].Name != V2[i].Name)
                    {
                        equal = false;
                        break;
                    }

                if (equal)
                    return true;
                else
                    return false;
            }
        }

        public static bool operator !=(GVertices V1, GVertices V2)
        {
            if (V1.Amount != V2.Amount)
                return true;
            else
            {
                bool equal = true;
                for (int i = 0; i < V1.Amount; i++)
                    if (V1[i].Name != V2[i].Name)
                    {
                        equal = false;
                        break;
                    }

                if (equal)
                    return false;
                else
                    return true;
            }
        }

        public static bool operator <=(GVertices v, GVertices V)
        {
            for (int i = 0; i < v.Amount; i++)
            {
                if (!V.Contain(v[i]))
                    return false;
            }
            return true;
        }

        public static bool operator >=(GVertices V, GVertices v)
        {
            for (int i = 0; i < v.Amount; i++)
            {
                if (!V.Contain(v[i]))
                    return false;
            }
            return true;
        }

        #endregion
        
        // : check if this vertices contain given vertex
        public bool Contain(GVertex v)
        {
            for (int i = 0; i < this.Amount; i++)
            {
                if (this[i] == v)
                {
                    return true;
                }
            }

            return false;
        }

        // : check if this vertices contain vertex with given name
        public bool Contain(string name)
        {
            for (int i = 0; i < this.Amount; i++)
            {
                if (this[i].Name == name)
                {
                    return true;
                }
            }

            return false;
        }
        
        // : check intersection with another vertices array
        public bool Intersect(GVertices V)
        {
            for (int i = 0; i < this.Amount; i++)
            {
                if (V.Contain(this[i]))
                    return true;
            }

            return false;
        }
                        
        // : adjust addresses if vertices was substracted
        public void AdjustAdds()
        {
            for (int i = 0; i < this.Amount; i++)
            {
                this[i].Adds = i;
            }
        }        
    }
}
