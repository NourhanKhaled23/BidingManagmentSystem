using Application.Interfaces;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
namespace Infrastructure.Repositories
{
    public class BidRepository : IBidRepository
    {
        private readonly AppDbContext _context;

        public BidRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Bid bid)
        {
            await _context.Bids.AddAsync(bid);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Bid>> GetBidsByTenderIdAsync(Guid tenderId)
        {
            return await _context.Bids
                .Where(b => b.TenderId == tenderId)
                .ToListAsync();
        }
    }
}
