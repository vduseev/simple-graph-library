using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using GraphSharp.Controls;

namespace GraphLibrary
{
    public class ControlViewModel : INotifyPropertyChanged
    {
        #region Data

        private string layoutAlgorithmType;
        private PocGraph graph;
        private List<String> layoutAlgorithmTypes = new List<string>();
        #endregion
        
        #region Ctor
        public ControlViewModel(GGraph G)
        {            
            Graph = new PocGraph(true);
            
            List<GEdge> ggedgeList = G.GetEdgeList();

            List<PocVertex> existingVertices = new List<PocVertex>();
            for (int i = 0; i < G.VerticesAmount; i++)
                existingVertices.Add(new PocVertex(G[i].Name));

            foreach (PocVertex vertex in existingVertices)
                Graph.AddVertex(vertex);
            
            //add some edges to the graph
            if (ggedgeList.Count > 0)
            {
                for (int i = 0; i < ggedgeList.Count; i++)
                {
                    GEdge edge = ggedgeList[i];
                    int vertexIndex1 = edge.U.Adds;
                    int vertexIndex2 = edge.V.Adds;

                    AddNewGraphEdge(existingVertices[vertexIndex1], existingVertices[vertexIndex2]);
                }
            }

            //Add Layout Algorithm Types
            layoutAlgorithmTypes.Add("BoundedFR");
            layoutAlgorithmTypes.Add("Circular");
            layoutAlgorithmTypes.Add("CompoundFDP");
            layoutAlgorithmTypes.Add("EfficientSugiyama");
            layoutAlgorithmTypes.Add("FR");
            layoutAlgorithmTypes.Add("ISOM");
            layoutAlgorithmTypes.Add("KK");
            layoutAlgorithmTypes.Add("LinLog");
            layoutAlgorithmTypes.Add("Tree");

            //Pick a default Layout Algorithm Type
            LayoutAlgorithmType = "EfficientSugiyama";

        }
        #endregion
        
        public void ReLayoutGraph()
        {
            //graph = new PocGraph(true);
            //count++;

            //List<PocVertex> existingVertices = new List<PocVertex>();
            
            //foreach (PocVertex vertex in existingVertices)
            //    Graph.AddVertex(vertex);
            
            ////add some edges to the graph
            //AddNewGraphEdge(existingVertices[0], existingVertices[1]);
            //AddNewGraphEdge(existingVertices[0], existingVertices[2]);
            
            //NotifyPropertyChanged("Graph");
        }
        
        #region Private Methods
        private PocEdge AddNewGraphEdge(PocVertex from, PocVertex to)
        {
            string edgeString = string.Format("{0}-{1} Connected", from.Name, to.Name);

            PocEdge newEdge = new PocEdge(edgeString, from, to);
            Graph.AddEdge(newEdge);
            return newEdge;
        }
        #endregion

        #region Public Properties

        public List<String> LayoutAlgorithmTypes
        {
            get { return layoutAlgorithmTypes; }
        }


        public string LayoutAlgorithmType
        {
            get { return layoutAlgorithmType; }
            set
            {
                layoutAlgorithmType = value;
                NotifyPropertyChanged("LayoutAlgorithmType");
            }
        }
        

        public PocGraph Graph
        {
            get { return graph; }
            set
            {
                graph = value;
                NotifyPropertyChanged("Graph");
            }
        }
        #endregion

        #region INotifyPropertyChanged Implementation

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(String info)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(info));
            }
        }

        #endregion
    }
}
