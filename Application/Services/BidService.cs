using Application.Interfaces;
using Domain.Entities;
namespace Application.Services
{
    public class BidService
    {
        private readonly IBidRepository _bidRepository;
        private readonly ITenderRepository _tenderRepository;

        public BidService(IBidRepository bidRepository, ITenderRepository tenderRepository)
        {
            _bidRepository = bidRepository;
            _tenderRepository = tenderRepository;
        }

        public async Task<Bid> SubmitBidAsync(Guid bidderId, Guid tenderId, decimal amount, string? documentPath)
        {
            var tender = await _tenderRepository.GetByIdAsync(tenderId);
            if (tender == null || tender.State != TenderState.Open)
                throw new Exception("Tender is not available for bidding.");

            var bid = new Bid(tenderId, bidderId, amount, documentPath ?? string.Empty);
            tender.SubmitBid(bid);

            await _bidRepository.AddAsync(bid);
            await _tenderRepository.UpdateAsync(tender);

            return bid;
        }

        public async Task<IEnumerable<Bid>> GetBidsForTenderAsync(Guid tenderId)
        {
            return await _bidRepository.GetBidsByTenderIdAsync(tenderId);
        }
    }
}
