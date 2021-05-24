﻿using InvestorApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
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
        //[BindProperty (SupportsGet =true)]
        //public string [] XLabels { get; set; }
        //[BindProperty(SupportsGet = true)]
        //public IEnumerable<InvestmentData.Bound> Bounds { get; set; }
        [BindProperty (SupportsGet =true)]
        public string GraphData { get; set; }
        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
            
        }
        public async Task<IActionResult> OnPostAsync()
        {
            var request = new Request {LumpSum = LumpSum, Monthly=Monthly,RiskLevel=RiskLevel.ToString(),Target=Target, Timescale= Timescale
            };
            var jsonRequest = JsonSerializer.Serialize(request);
            var dataRequest = new StringContent(jsonRequest, Encoding.UTF8, "application/json");
            using (var httpClient = new HttpClient())
            {
                var result = await httpClient.PostAsync("http://localhost:5000/api/Investor/Post",dataRequest);
                if (result.IsSuccessStatusCode)
                {
                    var jsonResponse = await result.Content.ReadAsStringAsync();
                    if (!string.IsNullOrWhiteSpace(jsonResponse))
                    {
                        InvestmentData data = JsonSerializer.Deserialize<InvestmentData>(jsonResponse,new JsonSerializerOptions {PropertyNameCaseInsensitive=true });
                        if (data != null)
                        {
                            //XLabels = data.Risks.FirstOrDefault().Bounds.FirstOrDefault().Months.Select(x => x.Index.ToString()).ToArray<string>();
                            var bounds = data.Risks.FirstOrDefault().Bounds.ToList();
                            var graphData = new GraphData();
                            graphData.Labels = bounds.FirstOrDefault().Months.Select(x => x.Index.ToString()).ToArray<string>();
                            var datasets = new List<GraphData.Dataset>();
                            foreach (var bound in bounds)
                            {
                                datasets.Add(
                                    new GraphData.Dataset { 
                                        BackgroundColour = "rgb(255, 99, 132)", 
                                        BorderColour = "rgb(255, 99, 132)", 
                                        Data = bound.Months.Select(x => x.Balance), Label = bound.BoundType.ToString() });
                            }
                            graphData.Datasets = datasets;
                            GraphData = JsonSerializer.Serialize(graphData, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
                                
                        }
                        
                    }
                }
                
            }
            return Page();
        }
    }
}
