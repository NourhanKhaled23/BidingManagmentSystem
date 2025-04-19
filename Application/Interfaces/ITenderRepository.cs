using Domain.Entities;
using System;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface ITenderRepository
    {
        Task<Tender?> GetByIdAsync(Guid id);      
        Task AddAsync(Tender tender);
        Task UpdateAsync(Tender tender);
        Task<IEnumerable<Tender>> GetAllAsync();
        IQueryable<Tender> GetQueryable();

    }
}
