﻿namespace InvestorWebService.Models
{
    public class InvestmentRequest
    {
        public decimal LumpSum { get; set; }

        public decimal Monthly { get; set; }

        public decimal Target { get; set; }

        public int Timescale { get; set; }

        public string RiskLevel { get; set; }
    }
}
