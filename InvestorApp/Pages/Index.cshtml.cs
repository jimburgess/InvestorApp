using InvestorApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace InvestorApp.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        [Display(Name = "Lump Sum Investment (£)")]
        [Required]
        [BindProperty]
        public decimal LumpSum { get; set; }

        [Display(Name = "Monthly Investment (£)")]
        [Required]
        [BindProperty]
        public decimal Monthly { get; set; }

        [Display(Name = "Target Value (£)")]
        [Required]
        [BindProperty]
        public decimal Target { get; set; }

        [Display(Name = "Timescale (years)")]
        [Required]
        [BindProperty]
        public int Timescale { get; set; }

        [Display(Name = "Risk Level")]
        [Required]
        [BindProperty]
        public Level RiskLevel { get; set; }

        public enum Level
        {
            Low, Medium, High
        }

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {

        }
        public async Task OnPostAsync()
        {
            var request = new Request {LumpSum = LumpSum, Monthly=Monthly,RiskLevel=RiskLevel.ToString(),Target=Target, Timescale= Timescale
            };
            var json = JsonSerializer.Serialize(request);
            var data = new StringContent(json, Encoding.UTF8, "application/json");
            using (var httpClient = new HttpClient())
            {
                var result = await httpClient.PostAsync("http://localhost:5000/api/Investor/Post",data);
            }
        }
    }
}
