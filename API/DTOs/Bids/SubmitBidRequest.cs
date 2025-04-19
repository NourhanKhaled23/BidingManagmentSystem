using System.ComponentModel.DataAnnotations;
namespace API.DTOs.Bids
{
    public class SubmitBidRequest
    {
        [Required]
        public Guid TenderId { get; set; }

        [Required]
        public decimal Amount { get; set; }

        public IFormFile? Document { get; set; }
    }
}
