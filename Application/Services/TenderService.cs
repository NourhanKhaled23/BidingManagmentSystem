using Application.Interfaces;
using Domain.Entities;
using Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Services
{
    public class TenderService
    {
        private readonly ITenderRepository _tenderRepository;

        public TenderService(ITenderRepository tenderRepository)
        {
            _tenderRepository = tenderRepository;
        }

        public async Task<Tender> CreateTenderAsync(string title, string description, DateTime deadline, decimal budgetAmount, string currency, string? documentPath)
        {
            var tender = new Tender(title, description, deadline, new Money(budgetAmount, currency), documentPath);
            await _tenderRepository.AddAsync(tender);
            return tender;
        }

        public async Task<Tender> OpenTenderAsync(Guid tenderId)
        {
            var tender = await _tenderRepository.GetByIdAsync(tenderId);
            if (tender == null)
                throw new Exception("Tender not found");

            tender.OpenTender();
            await _tenderRepository.UpdateAsync(tender);
            return tender;
        }

        public async Task<Tender> CloseTenderAsync(Guid tenderId)
        {
            var tender = await _tenderRepository.GetByIdAsync(tenderId);
            if (tender == null)
                throw new Exception("Tender not found");

            tender.CloseTender();
            await _tenderRepository.UpdateAsync(tender);
            return tender;
        }

        public async Task<Tender?> GetByIdAsync(Guid tenderId)
        {
            return await _tenderRepository.GetByIdAsync(tenderId);
        }

        public async Task<IEnumerable<Tender>> GetAllAsync()
        {
            return await _tenderRepository.GetAllAsync();
        }

        public async Task<Tender> UpdateTenderAsync(Guid id, string title, string description, DateTime deadline, decimal budgetAmount, string currency)
        {
            var tender = await _tenderRepository.GetByIdAsync(id);
            if (tender == null)
                throw new Exception("Tender not found");

            if (tender.State != TenderState.Draft)
                throw new Exception("Only draft tenders can be updated");

            tender.UpdateDetails(title, description, deadline, new Money(budgetAmount, currency));
            await _tenderRepository.UpdateAsync(tender);
            return tender;
        }
        public async Task<Tender> AwardTenderAsync(Guid tenderId)
        {
            var tender = await _tenderRepository.GetByIdAsync(tenderId);
            if (tender == null)
                throw new Exception("Tender not found.");

            tender.AwardTender();
            await _tenderRepository.UpdateAsync(tender);

            return tender;
        }

    }
}
