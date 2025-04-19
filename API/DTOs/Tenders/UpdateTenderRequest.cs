using System;
using System.ComponentModel.DataAnnotations;

namespace API.DTOs.Tenders
{
    public class UpdateTenderRequest
    {
        [Required]
        public string Title { get; set; } = string.Empty;

        [Required]
        public string Description { get; set; } = string.Empty;

        [Required]
        public DateTime Deadline { get; set; }

        [Required]
        public decimal BudgetAmount { get; set; }

        [Required]
        public string Currency { get; set; } = string.Empty;
    }
}

