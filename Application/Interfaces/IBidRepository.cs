using Domain.Entities;
using System;
using System.Threading.Tasks;
namespace Application.Interfaces
{
    public interface IBidRepository
    {
        Task AddAsync(Bid bid);
        Task<IEnumerable<Bid>> GetBidsByTenderIdAsync(Guid tenderId);
    }
}