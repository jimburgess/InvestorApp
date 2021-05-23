using InvestorWebService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InvestorWebService.Interfaces
{
    public interface ICalculator
    {
        InvestmentResponse CalculateInvestment(InvestmentRequest request);
    }
}
