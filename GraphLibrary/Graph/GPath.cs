using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphLibrary
{
    public class GPath
    {
        private bool            exist;

        private List<GEdge>     path;

        #region CONSTRUCTOR

        public GPath(GEdge e)
        {
            this.exist = true;

            path = new List<GEdge>();

            path.Add(e);
        }

        public GPath(bool exist = false)
        {
            this.exist = exist;
        }

        #endregion

        public static GPath Summ(GPath p1, GPath p2)
        {
            GPath result = new GPath(true);
            result.path = new List<GEdge>();

            for (int i = 0; i < p1.Amount; i++)
                result.path.Add(p1.path[i]);

            for (int i = 0; i < p2.Amount; i++)
                result.path.Add(p2.path[i]);

            return result;
        }

        #region INTERFACE

        public  bool        Exist
        {
            get
            {
                if (!exist)
                    return false;
                else
                    return true;
            }
        }

        public  int         Length
        {
            get
            {
                int length = 0;
                
                for (int i = 0; i < path.Count; i++)
                    length += path[i].Value;

                return length;
            }
        }

        public  int         Amount
        {
            get
            {
                return path.Count;
            }
        }

        public  GEdge       First
        {
            get
            {
                return path.First();
            }
        }

        public  GEdge       Last
        {
            get
            {
                return path.Last();
            }
        }

        public  List<GEdge> Edges
        {
            get
            {
                return path;
            }
        }

        #endregion

        #region OPERATORS

        public static GPath operator +(GPath p1, GPath p2)
        {
            //GPath result = new GPath(p1.First);

            GPath result = new GPath(true);
            result.path = new List<GEdge>();

            for (int i = 0; i < p1.Amount; i++)
                result.path.Add(p1.path[i]);

            for (int i = 0; i < p2.Amount; i++)
                result.path.Add(p2.path[i]);

            return result;
        }

        #endregion
    }
}
