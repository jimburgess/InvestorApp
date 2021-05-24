using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InvestorApp.Models
{
    public class GraphData
    {
        public string[] Labels { get; set; }
        public IEnumerable<Dataset> Datasets { get; set; }

        public class Dataset
        {
            public string Label { get; set; }
            public string BackgroundColour { get; set; }
            public string BorderColour { get; set; }
            public IEnumerable<decimal> Data { get; set; }
        }
    }
}
