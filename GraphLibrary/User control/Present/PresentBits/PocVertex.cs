using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace GraphLibrary
{
    /// <summary>
    /// A simple identifiable vertex.
    /// </summary>
    [DebuggerDisplay("{ID}")]
    public class PocVertex
    {
        public string   Name { get; private set; }

        public PocVertex(string name)
        {
            Name = name;
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
