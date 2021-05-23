using System.Collections.Generic;

namespace InvestorWebService.Models
{
    public class InvestmentResponse
    {
        public InvestmentResponse()
        {
            Risks = new List<Risk>();
        }
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
            public IEnumerable<Month> Months { get; set; }
        }
        public class Month
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
