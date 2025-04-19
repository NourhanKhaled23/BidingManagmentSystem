using Application.Interfaces;
using Domain.Entities;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Services
{
    public class EvaluationService
    {
        private readonly ITenderRepository _tenderRepository;
        public EvaluationService(ITenderRepository tenderRepository)
        {
            _tenderRepository = tenderRepository;
        }

        // Example: Automatically score bids based on lowest amount
        public async Task<Bid> EvaluateTenderAsync(Guid tenderId)
        {
            var tender = await _tenderRepository.GetByIdAsync(tenderId);
            if (tender == null)
                throw new Exception("Tender not found");

            if (!tender.Bids.Any())
                throw new Exception("No bids submitted");

            // Lowest bid wins
            var winningBid = tender.Bids.OrderBy(b => b.Amount).First();

            // Change tender state to awarded
            tender.AwardTender();
            await _tenderRepository.UpdateAsync(tender);

            return winningBid;
        }
    }
}
