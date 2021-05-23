using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace InvestorApp.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        [Display(Name = "Lump Sum Investment (£)")]
        [Required]
        public decimal LumpSum { get; set; }

        [Display(Name = "Monthly Investment (£)")]
        [Required]
        public decimal Monthly { get; set; }

        [Display(Name = "Target Value (£)")]
        [Required]
        public decimal Target { get; set; }

        [Display(Name = "Timescale (years)")]
        [Required]
        public int Timescale { get; set; }

        [Display(Name = "Risk Level")]
        [Required]
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
    }
}
