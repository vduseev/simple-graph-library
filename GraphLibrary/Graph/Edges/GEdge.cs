using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphLibrary
{
    public class GEdge
    {
        private GVertex u;
        private GVertex v;
        private int     value;
        public  string  Name;

        private bool    exist;

        public  bool    Exist
        {
            get
            {
                return exist;
            }
        }

        // # create member from two vertices (GVertex, GVertex)
        #region CONSTRUCTOR
        
        /// <summary>
        /// Create edge from 'u' vertex to 'v' vertex
        /// </summary>
        /// <param name="name"></param>
        /// <param name="address"></param> 
        public GEdge(GVertex u, GVertex v, int value)
        {
            this.u  = u;
            this.v  = v;
            this.value = value;
            this.exist = true;
        }

        public GEdge(GVertex u, GVertex v, bool exist = false)
        {
            this.u = u;
            this.v = v;
            this.exist = exist;
            this.value = GGraph.INFINITY;
        }

        #endregion

        #region INTERFACE

        /// <summary>
        /// Get or set edge weight
        /// </summary>
        public int Value
        {
            get
            {
                return value;
            }
            set
            {
                this.value = value;
            }
        }

        /// <summary>
        /// Get starting vertex
        /// </summary>
        public GVertex U
        {
            get
            {
                return u;
            }
        }

        /// <summary>
        /// Get Destination vertex
        /// </summary>
        public GVertex V
        {
            get
            {
                return v;
            }
        }

        #endregion

        #region OPERATORS

        // : == operator
        public static bool operator ==(GEdge e1, GEdge e2)
        {
            // edge 1 and edge2 exists
            if (e1.Exist && e2.Exist)
            {
                if (e1.U == e2.U && e1.V == e2.V && e1.Value == e2.Value)
                    return true;
                else
                    return false;
            }
            else if (!e1.Exist && !e2.Exist)
            {
                if (e1.U == e2.U && e1.V == e2.V)
                    return true;
                else
                    return false;
            }
            else
                return false;
        }

        // : != operator
        public static bool operator !=(GEdge e1, GEdge e2)
        {
            // edge 1 and edge2 exists
            if (e1.Exist && e2.Exist)
            {
                if (e1.U == e2.U && e1.V == e2.V && e1.Value == e2.Value)
                    return false;
                else
                    return true;
            }
            else if (!e1.Exist && !e2.Exist)
            {
                if (e1.U == e2.U && e1.V == e2.V)
                    return false;
                else
                    return true;
            }
            else
                return true;
        }

        #endregion
    }
}
