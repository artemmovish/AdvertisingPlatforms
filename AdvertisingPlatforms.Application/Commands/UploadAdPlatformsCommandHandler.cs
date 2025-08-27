using AdvertisingPlatforms.Application.Common;
using AdvertisingPlatforms.Domain.Entites;
using AdvertisingPlatforms.Domain.Interfaces;
using MediatR;
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

        public UploadAdPlatformsCommandHandler(IFileParsingService parsingService, IAdPlatformsRepository repository)
        {
            _parsingService = parsingService;
            _repository = repository;
        }

        public async Task<Result<FileProcessingResult>> Handle(UploadAdPlatformsCommand request, CancellationToken cancellationToken)
        {
            var errorLines = new List<ErrorLine>();

            var platforms = await _parsingService.ParsePlatformsAsync(request.FileStream, cancellationToken, errorLines);

            if (!platforms.Any())
            {
                return Result<FileProcessingResult>.Success(new FileProcessingResult
                {
                    FailedLines = errorLines.Select(e => e.Data).ToList(),
                    HasSuccess = false
                });
            }

            cancellationToken.ThrowIfCancellationRequested();

            _repository.LoadAdPlatforms(platforms);

            return Result<FileProcessingResult>.Success(new FileProcessingResult
            {
                FailedLines = errorLines.Select(e => e.Data).ToList(),
                HasSuccess = true
            });
        }

    }
}
