using InvestorWebService.Interfaces;
using InvestorWebService.Models;
using System.Collections.Generic;

namespace InvestorWebService.Services
{
    public class DefaultCalculator : ICalculator
    {
        public InvestmentResponse CalculateInvestment(InvestmentRequest request)
        {
            return CalculateRiskBounds(request);
            
        }
        private InvestmentResponse CalculateRiskBounds(InvestmentRequest request)
        {
            var response = new InvestmentResponse();
            var risk = new InvestmentResponse.Risk();
            var riskBounds = new List<InvestmentResponse.Bound>();
            var wideRiskBound1 = new InvestmentResponse.Bound {BoundType= InvestmentResponse.BoundType.Wide };
            var wideRiskBound2 = new InvestmentResponse.Bound { BoundType = InvestmentResponse.BoundType.Wide };
            var narrowRiskBound1 = new InvestmentResponse.Bound { BoundType = InvestmentResponse.BoundType.Narrow };
            var narrowRiskBound2 = new InvestmentResponse.Bound { BoundType = InvestmentResponse.BoundType.Narrow };
            switch (request.RiskLevel.ToLower().Substring(0,1))
            {
                case "h":
                    {
                        wideRiskBound1.InterestRate = -1;
                        wideRiskBound1.Months = CalculateMonths(-1, request);
                        wideRiskBound1.Years = CalculateYears(wideRiskBound1.Months);
                        wideRiskBound2.InterestRate = 7;
                        wideRiskBound2.Months = CalculateMonths(7, request);
                        wideRiskBound2.Years = CalculateYears(wideRiskBound2.Months);
                        narrowRiskBound1.InterestRate = 2;
                        narrowRiskBound1.Months = CalculateMonths(2, request);
                        narrowRiskBound1.Years = CalculateYears(narrowRiskBound1.Months);
                        narrowRiskBound2.InterestRate = 4;
                        narrowRiskBound2.Months = CalculateMonths(4, request);
                        narrowRiskBound2.Years = CalculateYears(narrowRiskBound2.Months);
                        break;
                    }
                case "l":
                    {
                        wideRiskBound1.InterestRate = 1;
                        wideRiskBound1.Months = CalculateMonths(1, request);
                        wideRiskBound1.Years = CalculateYears(wideRiskBound1.Months);
                        wideRiskBound2.InterestRate = 3;
                        wideRiskBound2.Months = CalculateMonths(3, request);
                        wideRiskBound2.Years = CalculateYears(wideRiskBound2.Months);
                        narrowRiskBound1.InterestRate = 1.5M;
                        narrowRiskBound1.Months = CalculateMonths(1.5M, request);
                        narrowRiskBound1.Years = CalculateYears(narrowRiskBound1.Months);
                        narrowRiskBound2.InterestRate = 2.5M;
                        narrowRiskBound2.Months = CalculateMonths(2.5M, request);
                        narrowRiskBound2.Years = CalculateYears(narrowRiskBound2.Months);
                        break;
                    }
                default:
                    {
                        wideRiskBound1.InterestRate = 0;
                        wideRiskBound1.Months = CalculateMonths(0, request);
                        wideRiskBound1.Years = CalculateYears(wideRiskBound1.Months);
                        wideRiskBound2.InterestRate = 5;
                        wideRiskBound2.Months = CalculateMonths(5, request);
                        wideRiskBound2.Years = CalculateYears(wideRiskBound2.Months);
                        narrowRiskBound1.InterestRate = 1.5M;
                        narrowRiskBound1.Months = CalculateMonths(1.5M, request);
                        narrowRiskBound1.Years = CalculateYears(narrowRiskBound1.Months);
                        narrowRiskBound2.InterestRate = 3.5M;
                        narrowRiskBound2.Months = CalculateMonths(3.5M, request);
                        narrowRiskBound2.Years = CalculateYears(narrowRiskBound2.Months);
                        break;
                    }
            }
            riskBounds.Add(wideRiskBound1);
            riskBounds.Add(wideRiskBound2);
            riskBounds.Add(narrowRiskBound1);
            riskBounds.Add(narrowRiskBound2);
            risk.Bounds = riskBounds;
            response.Risks.Add(risk);
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
                    month.Interest = decimal.Round(((totalBalance / 100) * interestRate),2);
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

        private IEnumerable<InvestmentResponse.Year> CalculateYears(IEnumerable<InvestmentResponse.Month> months)
        {
            List<InvestmentResponse.Year> years = new List<InvestmentResponse.Year>();
            decimal interest = 0;
            int index = 0;
            foreach (var month in months)
            {
                if (month.Index == 0)
                {
                    years.Add(
                        new InvestmentResponse.Year
                        {
                            Index = 0,
                            Balance = month.Balance,
                            Deposits = month.Deposits,
                            Interest = 0,
                            TotalDeposits = month.TotalDeposits,
                            TotalInterest = 0
                        });
                }
                else
                {
                    interest += month.Interest;
                    if (month.Index % 12 == 0)
                    {
                        index++;
                        years.Add(
                            new InvestmentResponse.Year 
                            { 
                                Index = index, 
                                Balance = month.Balance, 
                                TotalInterest=month.TotalInterest,
                                TotalDeposits = month.TotalDeposits,
                                Interest = interest
                            });
                        interest = 0;
                    }
                }
            }
            
            return years;
        }
    }
}
