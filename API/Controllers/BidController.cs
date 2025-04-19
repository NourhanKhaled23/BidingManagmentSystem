using API.DTOs.Bids;
using Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BidController : ControllerBase
    {
        private readonly BidService _bidService;

        public BidController(BidService bidService)
        {
            _bidService = bidService;
        }

        [HttpPost("submit")]
        [Authorize(Roles = "Bidder")]
        public async Task<IActionResult> SubmitBid([FromForm] SubmitBidRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                string? documentPath = null;
                if (request.Document != null && request.Document.Length > 0)
                {
                    var uploadsDir = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "bids");
                    Directory.CreateDirectory(uploadsDir);

                    var fileName = $"{Guid.NewGuid()}_{request.Document.FileName}";
                    var filePath = Path.Combine(uploadsDir, fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await request.Document.CopyToAsync(stream);
                    }

                    documentPath = $"bids/{fileName}";
                }

                var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
                if (userId == null)
                    return Unauthorized();

                var bid = await _bidService.SubmitBidAsync(
                    Guid.Parse(userId),
                    request.TenderId,
                    request.Amount,
                    documentPath
                );

                return Ok(new BidResponse
                {
                    Id = bid.Id,
                    TenderId = bid.TenderId,
                    BidderId = bid.BidderId,
                    Amount = bid.Amount,
                    SupportingDocuments = bid.SupportingDocuments
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("tender/{tenderId}")]
        [Authorize(Roles = "ProcurementOfficer")]
        public async Task<IActionResult> GetBidsForTender(Guid tenderId)
        {
            var bids = await _bidService.GetBidsForTenderAsync(tenderId);
            var result = bids.Select(b => new BidResponse
            {
                Id = b.Id,
                TenderId = b.TenderId,
                BidderId = b.BidderId,
                Amount = b.Amount,
                SupportingDocuments = b.SupportingDocuments
            });
            return Ok(result);
        }
    }
}