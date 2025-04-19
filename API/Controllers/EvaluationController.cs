using Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

[ApiController]
[Route("api/[controller]")]
public class EvaluationController : ControllerBase
{
    private readonly EvaluationService _evaluationService;

    public EvaluationController(EvaluationService evaluationService)
    {
        _evaluationService = evaluationService;
    }

    [HttpPost("{tenderId}/evaluate")]
    [Authorize(Roles = "Evaluator")]
    public async Task<IActionResult> EvaluateTender(Guid tenderId)
    {
        try
        {
            var winningBid = await _evaluationService.EvaluateTenderAsync(tenderId);
            return Ok(new { message = "Tender evaluated successfully.", winningBidId = winningBid.Id });
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
}
