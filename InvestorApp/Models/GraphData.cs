using System.Collections.Generic;

namespace InvestorApp.Models
{
    public class GraphData
    {
        public string[] Labels { get; set; }
        public IEnumerable<Dataset> Datasets { get; set; }

        public class Dataset
        {
            public string Label { get; set; }
            public string BackgroundColor { get; set; }
            public string BorderColor { get; set; }
            public IEnumerable<decimal> Data { get; set; }
        }
    }
}
