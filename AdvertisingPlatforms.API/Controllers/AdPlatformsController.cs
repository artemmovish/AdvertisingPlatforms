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

        public AdPlatformsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("search")]
        public async Task<ActionResult<ApiResponse<List<AdPlatform>>>> Search([FromQuery] string location)
        {
            var result = await _mediator.Send(new SearchAdPlatformsQuery(location));

            if (!result.IsSuccess)
            {
                return BadRequest(new ApiResponse<List<AdPlatform>>
                {
                    Success = false,
                    Errors = new List<string> { result.Error.Description },
                });
            }

            return Ok(new ApiResponse<List<AdPlatform>>
            {
                Success = true,
                Data = result.Value
            });
        }

        [HttpPost("Upload")]
        public async Task<ActionResult<ApiResponse<FileProcessingResult>> Upload([FromBody] IFormFile file)
        {
            var result = await _mediator.Send(new UploadAdPlatformsCommand(file.OpenReadStream()));

            if (!result.IsSuccess)
            {
                return BadRequest(new ApiResponse<FileProcessingResult>
                {
                    Success = false,
                    Errors = new List<string> { result.Error.Description }
                });
            }

            if (result.Value.HasSuccess)
            {
                return 
            }
        }
    }
}
