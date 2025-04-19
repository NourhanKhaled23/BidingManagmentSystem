using Domain.ValueObjects;

namespace Domain.Entities
{
    public enum TenderState
    {
        Draft,
        Open,
        Closed,
        Awarded
    }

    public class Tender
    {
        public Guid Id { get; private set; }
        public string Title { get; private set; }
        public string Description { get; private set; }
        public DateTime Deadline { get; private set; }
        public Money Budget { get; private set; }
        public TenderState State { get; private set; }
        public string? DocumentPath { get; private set; }

        private readonly List<Bid> _bids = new();
        public IReadOnlyCollection<Bid> Bids => _bids.AsReadOnly();

        public Tender(string title, string description, DateTime deadline, Money budget, string? documentPath)
        {
            Id = Guid.NewGuid();
            Title = title;
            Description = description;
            Deadline = deadline;
            Budget = budget;
            State = TenderState.Draft;
            DocumentPath = documentPath;
        }
        public Tender() { }

        public void UpdateDetails(string title, string description, DateTime deadline, Money budget)
        {
            if (State != TenderState.Draft)
                throw new InvalidOperationException("Only draft tenders can be updated.");

            Title = title;
            Description = description;
            Deadline = deadline;
            Budget = budget;
        }

        public void OpenTender()
        {
            if (State != TenderState.Draft)
                throw new InvalidOperationException("Only draft tenders can be opened.");

            State = TenderState.Open;
        }

        public void CloseTender()
        {
            if (State != TenderState.Open)
                throw new InvalidOperationException("Only open tenders can be closed.");

            State = TenderState.Closed;
        }

        public void AwardTender()
        {
            if (State != TenderState.Closed)
                throw new InvalidOperationException("Only closed tenders can be awarded.");

            State = TenderState.Awarded;
        }

        public void SubmitBid(Bid bid)
        {
            if (State != TenderState.Open)
                throw new InvalidOperationException("Cannot submit bids to a tender that is not open.");

            _bids.Add(bid);
        }
    }
}
