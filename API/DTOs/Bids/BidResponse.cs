namespace API.DTOs.Bids
{
    public class BidResponse
    {
        public Guid Id { get; set; }
        public Guid TenderId { get; set; }
        public Guid BidderId { get; set; }
        public decimal Amount { get; set; }
        public string SupportingDocuments { get; set; } = string.Empty;
    }
}