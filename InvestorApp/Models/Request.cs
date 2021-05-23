using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InvestorApp.Models
{
    public class Request
    {
        public decimal LumpSum { get; set; }

        public decimal Monthly { get; set; }

        public decimal Target { get; set; }

        public int Timescale { get; set; }

        public string RiskLevel { get; set; }
    }
}
