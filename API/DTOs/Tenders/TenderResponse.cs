namespace API.DTOs.Tenders
{
    public class TenderResponse
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime Deadline { get; set; }
        public decimal BudgetAmount { get; set; }
        public string Currency { get; set; } = string.Empty;
        public string? DocumentPath { get; set; }
        public string State { get; set; } = string.Empty;
    }
}
