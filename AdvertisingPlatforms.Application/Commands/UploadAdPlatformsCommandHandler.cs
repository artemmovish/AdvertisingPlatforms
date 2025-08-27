using AdvertisingPlatforms.Application.Common;
using AdvertisingPlatforms.Domain.Entites;
using AdvertisingPlatforms.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdvertisingPlatforms.Application.Commands
{
    public class UploadAdPlatformsCommandHandler : IRequestHandler<UploadAdPlatformsCommand, Result<FileProcessingResult>>
    {
        private readonly IFileParsingService _parsingService;
        private readonly IAdPlatformsRepository _repository;
        private readonly ILogger<UploadAdPlatformsCommandHandler> _logger;

        public UploadAdPlatformsCommandHandler(
            IFileParsingService parsingService,
            IAdPlatformsRepository repository,
            ILogger<UploadAdPlatformsCommandHandler> logger)
        {
            _parsingService = parsingService;
            _repository = repository;
            _logger = logger;
        }

        public async Task<Result<FileProcessingResult>> Handle(UploadAdPlatformsCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Starting file upload processing");

            var errorLines = new List<ErrorLine>();

            var platforms = await _parsingService.ParsePlatformsAsync(request.FileStream, cancellationToken, errorLines);

            if (!platforms.Any())
            {
                _logger.LogWarning("No valid platforms found in the uploaded file. Error lines: {ErrorCount}",
                    errorLines.Count);
                return Result<FileProcessingResult>.Success(new FileProcessingResult
                {
                    FailedLines = errorLines.Select(e => e.Data).ToList(),
                    HasSuccess = false
                });
            }

            _logger.LogInformation("Parsed {PlatformCount} platforms from file. Error lines: {ErrorCount}",
                platforms.Count, errorLines.Count);

            cancellationToken.ThrowIfCancellationRequested();

            _repository.LoadAdPlatforms(platforms);

            _logger.LogInformation("Successfully loaded {PlatformCount} platforms to repository", platforms.Count);

            return Result<FileProcessingResult>.Success(new FileProcessingResult
            {
                FailedLines = errorLines.Select(e => e.Data).ToList(),
                HasSuccess = true
            });
        }
    }
}
