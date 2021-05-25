using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InvestorApp.Models
{
    public class InvestmentData
    {
        public IList<Risk> Risks { get; set; }
        public class Risk
        {
            public Level Level;
            public IEnumerable<Bound> Bounds { get; set; }
        }
        public class Bound
        {
            public BoundType BoundType { get; set; }
            public decimal InterestRate { get; set; }
            public IEnumerable<Year> Years { get; set; }
        }
        public class Year
        {
            public int Index { get; set; }
            public decimal Deposits { get; set; }
            public decimal Interest { get; set; }
            public decimal TotalDeposits { get; set; }
            public decimal TotalInterest { get; set; }
            public decimal Balance { get; set; }
        }

        public enum Level
        {
            Low, Medium, High
        }
        public enum BoundType
        {
            Wide, Narrow
        }
    }
}
