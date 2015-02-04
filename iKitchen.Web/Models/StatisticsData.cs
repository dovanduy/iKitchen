using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace iKitchen.Web.Models
{
    public class StatisticsData
    {
        public string[] Series
        {
            get;
            set;
        }

        public string[] Title
        {
            get;
            set;
        }

        /// <summary>
        /// 单位
        /// </summary>
        public string Unit
        {
            get;
            set;
        }

        private List<double[]> graphs = new List<double[]>();
        public List<double[]> Graphs
        {
            get
            {
                return graphs;
            }
        }

        public StatisticsData AppendGraph(double[] graph)
        {
            graphs.Add(graph);
            return this;
        }
    }
}