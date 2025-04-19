using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace API.DTOs.Tenders
{
    public class CreateTenderRequest
    {
        [Required]
        public string Title { get; set; } = string.Empty;

        [Required]
        public string Description { get; set; } = string.Empty;

        [Required]
        public DateTime Deadline { get; set; }

        [Required]
        [Range(1, double.MaxValue, ErrorMessage = "Budget must be positive.")]
        public decimal BudgetAmount { get; set; }

        [Required]
        public string Currency { get; set; } = string.Empty;

        public IFormFile? Document { get; set; }
    }
}
