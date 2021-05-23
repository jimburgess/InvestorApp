using InvestorWebService.Models;

namespace InvestorWebService.Interfaces
{
    public interface ICalculator
    {
        InvestmentResponse CalculateInvestment(InvestmentRequest request);
    }
}
