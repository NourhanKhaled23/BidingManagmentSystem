using Application.Interfaces;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class TenderRepository : ITenderRepository
    {
        private readonly AppDbContext _context;

        public TenderRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Tender?> GetByIdAsync(Guid id)
        {
            return await _context.Tenders
                .Include(t => t.Bids)
                .FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task AddAsync(Tender tender)
        {
            await _context.Tenders.AddAsync(tender);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Tender tender)
        {
            _context.Tenders.Update(tender);
            await _context.SaveChangesAsync();
        }
        public async Task<IEnumerable<Tender>> GetAllAsync()
        {
            return await _context.Tenders.Include(t => t.Bids).ToListAsync();
        }
        public IQueryable<Tender> GetQueryable()
        {
            return _context.Tenders.AsQueryable();
        }


    }
}
