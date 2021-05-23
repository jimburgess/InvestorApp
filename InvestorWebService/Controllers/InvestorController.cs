using InvestorWebService.Interfaces;
using InvestorWebService.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;

namespace InvestorWebService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InvestorController : Controller
    {
        private readonly ILogger<InvestorController> _logger;
        private readonly ICalculator _calculator;

        public InvestorController(ILogger<InvestorController> logger, ICalculator calculator)
        {
            _logger = logger;
            _calculator = calculator;
        }
        [HttpPost("[Action]")]
        public InvestmentResponse Post([FromBody] InvestmentRequest request)
        {
            
            try
            {
                return _calculator.CalculateInvestment(request);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in getting investment results");
                return null;
            }
        }
    }
}
