namespace Domain.Entities
{
    public class Bid
    {
        public Guid Id { get; private set; }
        public Guid TenderId { get; private set; }
        public Guid BidderId { get; private set; }
        public decimal Amount { get; private set; }
        public string SupportingDocuments { get; private set; } = string.Empty;

        public Tender Tender { get; set; } = null!; // Navigation to tender
        public User Bidder { get; set; } = null!;   // Navigation to user

        public Bid(Guid tenderId, Guid bidderId, decimal amount, string supportingDocuments)
        {
            Id = Guid.NewGuid();
            TenderId = tenderId;
            BidderId = bidderId;
            Amount = amount;
            SupportingDocuments = supportingDocuments;
        }

        public void UpdateAmount(decimal newAmount)
        {
            Amount = newAmount;
        }

        public void AddDocuments(string filePath)
        {
            SupportingDocuments = filePath;
        }
    }
}
