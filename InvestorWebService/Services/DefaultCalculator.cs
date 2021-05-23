using InvestorWebService.Interfaces;
using InvestorWebService.Models;
using System.Collections.Generic;

namespace InvestorWebService.Services
{
    public class DefaultCalculator : ICalculator
    {
        public InvestmentResponse CalculateInvestment(InvestmentRequest request)
        {
            var response = new InvestmentResponse();
            var lowRiskBounds = new List<InvestmentResponse.Bound>();
            var mediumRiskBounds = new List<InvestmentResponse.Bound>();
            var highRiskBounds = new List<InvestmentResponse.Bound>();

            var lowRiskWideBound1 = new InvestmentResponse.Bound { BoundType = InvestmentResponse.BoundType.Wide, InterestRate = 1, Months = CalculateMonths(1, request) };
            var lowRiskWideBound3 = new InvestmentResponse.Bound { BoundType = InvestmentResponse.BoundType.Wide, InterestRate = 3, Months = CalculateMonths(3, request) };
            var lowRiskNarrowBound1_5 = new InvestmentResponse.Bound { BoundType = InvestmentResponse.BoundType.Narrow, InterestRate = 1.5M, Months = CalculateMonths(1.5M, request) };
            var lowRiskNarrowBound2_5 = new InvestmentResponse.Bound { BoundType = InvestmentResponse.BoundType.Narrow, InterestRate = 2.5M, Months = CalculateMonths(2.5M, request) };
            lowRiskBounds.Add(lowRiskWideBound1);
            lowRiskBounds.Add(lowRiskWideBound3);
            lowRiskBounds.Add(lowRiskNarrowBound1_5);
            lowRiskBounds.Add(lowRiskNarrowBound2_5);
            var lowRisk = new InvestmentResponse.Risk { Level = InvestmentResponse.Level.Low, Bounds = lowRiskBounds };

            var mediumRiskWideBound0 = new InvestmentResponse.Bound { BoundType = InvestmentResponse.BoundType.Wide, InterestRate = 0, Months = CalculateMonths(0, request) };
            var mediumRiskWideBound5 = new InvestmentResponse.Bound { BoundType = InvestmentResponse.BoundType.Wide, InterestRate = 5, Months = CalculateMonths(5, request) };
            var mediumRiskNarrowBound1_5 = new InvestmentResponse.Bound { BoundType = InvestmentResponse.BoundType.Narrow, InterestRate = 1.5M, Months = CalculateMonths(1.5M, request) };
            var mediumRiskNarrowBound3_5 = new InvestmentResponse.Bound { BoundType = InvestmentResponse.BoundType.Narrow, InterestRate = 3.5M, Months = CalculateMonths(3.5M, request) };
            mediumRiskBounds.Add(mediumRiskWideBound0);
            mediumRiskBounds.Add(mediumRiskWideBound5);
            mediumRiskBounds.Add(mediumRiskNarrowBound1_5);
            mediumRiskBounds.Add(mediumRiskNarrowBound3_5);
            var mediumRisk = new InvestmentResponse.Risk { Level = InvestmentResponse.Level.Medium, Bounds = mediumRiskBounds };

            var highRiskWideBound_Minus1 = new InvestmentResponse.Bound { BoundType = InvestmentResponse.BoundType.Wide, InterestRate = -1, Months = CalculateMonths(-1, request) };
            var highRiskWideBound7 = new InvestmentResponse.Bound { BoundType = InvestmentResponse.BoundType.Wide, InterestRate = 7, Months = CalculateMonths(7, request) };
            var highRiskNarrowBound2 = new InvestmentResponse.Bound { BoundType = InvestmentResponse.BoundType.Narrow, InterestRate = 2, Months = CalculateMonths(2, request) };
            var highRiskNarrowBound4 = new InvestmentResponse.Bound { BoundType = InvestmentResponse.BoundType.Narrow, InterestRate = 4, Months = CalculateMonths(4, request) };

            highRiskBounds.Add(highRiskWideBound_Minus1);
            highRiskBounds.Add(highRiskWideBound7);
            highRiskBounds.Add(highRiskNarrowBound2);
            highRiskBounds.Add(highRiskNarrowBound4);
            var highRisk = new InvestmentResponse.Risk { Level = InvestmentResponse.Level.High, Bounds = highRiskBounds };

            response.Risks.Add(lowRisk);
            response.Risks.Add(mediumRisk);
            response.Risks.Add(highRisk);
            return response;
        }
        private IEnumerable<InvestmentResponse.Month> CalculateMonths(decimal interestRate, InvestmentRequest request)
        {
            List<InvestmentResponse.Month> months = new List<InvestmentResponse.Month>();
            decimal totalBalance = 0;
            decimal totalDeposits = 0;
            decimal totalInterest = 0;
            for (int m = 0; m <= request.Timescale * 12; m++)
            {
                var month = new InvestmentResponse.Month { Index = m };
                if (m == 0)
                {
                    month.Deposits = request.LumpSum;
                    month.TotalDeposits = request.LumpSum;
                    month.Interest = 0;
                    month.Balance = request.LumpSum;

                    totalBalance = request.LumpSum;
                    totalDeposits = request.LumpSum;
                }
                else
                {
                    month.Deposits = request.Monthly;
                    month.TotalDeposits = request.LumpSum + (request.Monthly * m);
                    month.Interest = (totalBalance / 100) * interestRate;
                    month.TotalInterest = month.Interest + totalInterest;
                    month.Balance = month.TotalDeposits + month.TotalInterest;

                    totalBalance += (month.Interest + month.Deposits);
                    totalInterest += month.Interest;
                    totalDeposits += request.Monthly;
                }
                months.Add(month);
            }
            return months;
        }
    }
}
