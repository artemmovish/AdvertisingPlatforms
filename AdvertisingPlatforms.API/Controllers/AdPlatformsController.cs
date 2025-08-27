using AdvertisingPlatforms.Application.Commands;
using AdvertisingPlatforms.Application.Common;
using AdvertisingPlatforms.Application.DTOs;
using AdvertisingPlatforms.Application.Queries;
using AdvertisingPlatforms.Domain.Entites;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;



namespace AdvertisingPlatforms.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdPlatformsController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<AdPlatformsController> _logger;

        public AdPlatformsController(IMediator mediator, ILogger<AdPlatformsController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpGet("search")]
        public async Task<ActionResult<ApiResponse<List<AdPlatform>>>> Search([FromQuery] string location)
        {
            _logger.LogInformation("Search endpoint called for location: {Location}", location);

            var result = await _mediator.Send(new SearchAdPlatformsQuery(location));

            if (!result.IsSuccess)
            {
                _logger.LogWarning("Search failed for location: {Location}. Error: {Error}",
                    location, result.Error.Description);
                return BadRequest(new ApiResponse<List<AdPlatform>>
                {
                    Success = false,
                    Errors = new List<string> { result.Error.Description },
                });
            }

            _logger.LogInformation("Search successful for location: {Location}. Found {Count} platforms",
                location, result.Value.Count);
            return Ok(new ApiResponse<List<AdPlatform>>
            {
                Success = true,
                Data = result.Value
            });
        }

        [HttpPost("Upload")]
        [Consumes("multipart/form-data")]
        public async Task<ActionResult<ApiResponse<FileProcessingResult>>> Upload([FromForm] UploadFileRequest request)
        {
            _logger.LogInformation("Upload endpoint called. File name: {FileName}, Size: {Size} bytes",
                request.File?.FileName, request.File?.Length);

            var result = await _mediator.Send(new UploadAdPlatformsCommand(request.File.OpenReadStream()));

            if (!result.IsSuccess)
            {
                _logger.LogError("Upload failed. Error: {Error}", result.Error.Description);
                return BadRequest(new ApiResponse<FileProcessingResult>
                {
                    Success = false,
                    Errors = new List<string> { result.Error.Description }
                });
            }

            var fileProcessingResult = result.Value;

            if (fileProcessingResult.IsSuccessOnly)
            {
                _logger.LogInformation("Upload completed successfully. Processed platforms successfully");
                return Ok(new ApiResponse<FileProcessingResult>
                {
                    Success = true,
                    Data = fileProcessingResult
                });
            }

            if (fileProcessingResult.IsPartialSuccess)
            {
                _logger.LogWarning("Upload completed with partial success. Success: {SuccessCount}, Failed: {FailedCount}",
                    fileProcessingResult.FailedLines.Count, fileProcessingResult.FailedLines.Count);
                return Ok(new ApiResponse<FileProcessingResult>
                {
                    Success = true,
                    Data = fileProcessingResult,
                    Errors = fileProcessingResult.FailedLines
                        .Take(10)
                        .Select((line, index) => $"Line error {index + 1}: {line}")
                        .ToList()
                });
            }

            _logger.LogError("Upload failed completely. Failed lines: {FailedCount}",
                fileProcessingResult.FailedLines.Count);
            return BadRequest(new ApiResponse<FileProcessingResult>
            {
                Success = false,
                Data = fileProcessingResult,
                Errors = fileProcessingResult.FailedLines
                    .Take(10)
                    .Select((line, index) => $"Line error {index + 1}: {line}")
                    .ToList(),
            });
        }
    }
}
