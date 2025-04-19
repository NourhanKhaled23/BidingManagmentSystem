using API.DTOs.Tenders;
using Application.Services;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TenderController : ControllerBase
    {
        private readonly TenderService _tenderService;
        private readonly ILogger<TenderController> _logger;

        public TenderController(TenderService tenderService, ILogger<TenderController> logger)
        {
            _tenderService = tenderService;
            _logger = logger;
        }

        /// <summary>
        /// Get all tenders, with optional status filtering and pagination.
        /// </summary>
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllTenders(
            [FromQuery] string? status = null,
            [FromQuery] string? keyword = null,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10)
        {
            var tenders = await _tenderService.GetAllAsync();

            // Filter by status
            if (!string.IsNullOrEmpty(status) && Enum.TryParse(status, true, out TenderState parsedStatus))
                tenders = tenders.Where(t => t.State == parsedStatus).ToList();

            // Filter by keyword
            if (!string.IsNullOrWhiteSpace(keyword))
            {
                tenders = tenders.Where(t =>
                    t.Title.Contains(keyword, StringComparison.OrdinalIgnoreCase) ||
                    t.Description.Contains(keyword, StringComparison.OrdinalIgnoreCase)).ToList();
            }

            // Apply pagination
            var paged = tenders
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(t => new TenderResponse
                {
                    Id = t.Id,
                    Title = t.Title,
                    Description = t.Description,
                    Deadline = t.Deadline,
                    BudgetAmount = t.Budget.Amount,
                    Currency = t.Budget.Currency,
                    DocumentPath = t.DocumentPath,
                    State = t.State.ToString()
                });

            return Ok(paged);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetTender(Guid id)
        {
            var tender = await _tenderService.GetByIdAsync(id);
            if (tender == null)
                return NotFound(new { message = "Tender not found." });

            return Ok(new TenderResponse
            {
                Id = tender.Id,
                Title = tender.Title,
                Description = tender.Description,
                Deadline = tender.Deadline,
                BudgetAmount = tender.Budget.Amount,
                Currency = tender.Budget.Currency,
                DocumentPath = tender.DocumentPath,
                State = tender.State.ToString()
            });
        }

        [HttpPost]
        [Authorize(Roles = "ProcurementOfficer")]
        public async Task<IActionResult> CreateTender([FromForm] CreateTenderRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                string? documentPath = null;
                if (request.Document is { Length: > 0 })
                {
                    var uploadsDir = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
                    Directory.CreateDirectory(uploadsDir);

                    var fileName = $"{Guid.NewGuid()}_{request.Document.FileName}";
                    var filePath = Path.Combine(uploadsDir, fileName);

                    await using var stream = new FileStream(filePath, FileMode.Create);
                    await request.Document.CopyToAsync(stream);

                    documentPath = $"uploads/{fileName}";
                }

                var tender = await _tenderService.CreateTenderAsync(
                    request.Title,
                    request.Description,
                    request.Deadline,
                    request.BudgetAmount,
                    request.Currency,
                    documentPath
                );

                return CreatedAtAction(nameof(GetTender), new { id = tender.Id }, new TenderResponse
                {
                    Id = tender.Id,
                    Title = tender.Title,
                    Description = tender.Description,
                    Deadline = tender.Deadline,
                    BudgetAmount = tender.Budget.Amount,
                    Currency = tender.Budget.Currency,
                    DocumentPath = tender.DocumentPath,
                    State = tender.State.ToString()
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while creating tender.");
                return StatusCode(500, new { message = "An error occurred while creating the tender.", error = ex.Message });
            }
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "ProcurementOfficer")]
        public async Task<IActionResult> UpdateTender(Guid id, [FromBody] UpdateTenderRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var updatedTender = await _tenderService.UpdateTenderAsync(
                    id,
                    request.Title,
                    request.Description,
                    request.Deadline,
                    request.BudgetAmount,
                    request.Currency
                );

                return Ok(new TenderResponse
                {
                    Id = updatedTender.Id,
                    Title = updatedTender.Title,
                    Description = updatedTender.Description,
                    Deadline = updatedTender.Deadline,
                    BudgetAmount = updatedTender.Budget.Amount,
                    Currency = updatedTender.Budget.Currency,
                    DocumentPath = updatedTender.DocumentPath,
                    State = updatedTender.State.ToString()
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to update tender.");
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("{id}/open")]
        [Authorize(Roles = "ProcurementOfficer")]
        public async Task<IActionResult> OpenTender(Guid id)
        {
            try
            {
                await _tenderService.OpenTenderAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to open tender.");
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("{id}/close")]
        [Authorize(Roles = "ProcurementOfficer")]
        public async Task<IActionResult> CloseTender(Guid id)
        {
            try
            {
                await _tenderService.CloseTenderAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to close tender.");
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
