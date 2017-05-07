using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphLibrary
{    
    public class GVertex
    {
        public string   Name;
        public int      Adds;

        // # create member from name and address  (string, int)
        #region CONSTRUCTOR
        
        /// <summary>
        /// Create object from name and address
        /// </summary>
        /// <param name="name"></param>
        /// <param name="address"></param> 
        public GVertex(string name, int address)
        {
            this.Name       = name;
            this.Adds    = address;
        }

        #endregion

        #region OPERATORS

        // : == operator
        public static bool operator ==(GVertex v1, GVertex v2)
        {
            if (v1.Name == v2.Name)
                return true;
            else
                return false;
        }

        // : != operator
        public static bool operator !=(GVertex v1, GVertex v2)
        {
            if (v1.Name != v2.Name)
                return true;
            else
                return false;
        }

        // : < operator
        public static bool operator <(GVertex v1, GVertex v2)
        {
            if (Misc.StringIsLess(v1.Name, v2.Name))
                return true;
            else
                return false;
        }

        // : > operator
        public static bool operator >(GVertex v1, GVertex v2)
        {
            if (Misc.StringIsLess(v2.Name, v1.Name))
                return true;
            else
                return false;
        }

        #endregion
    }
}
