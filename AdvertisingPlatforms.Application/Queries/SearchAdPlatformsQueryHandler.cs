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

namespace AdvertisingPlatforms.Application.Queries
{
    public class SearchAdPlatformsQueryHandler
     : IRequestHandler<SearchAdPlatformsQuery, Result<List<AdPlatform>>>
    {
        private readonly IAdPlatformsRepository _repository;
        private readonly ILogger<SearchAdPlatformsQueryHandler> _logger;

        public SearchAdPlatformsQueryHandler(
            IAdPlatformsRepository repository,
            ILogger<SearchAdPlatformsQueryHandler> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public Task<Result<List<AdPlatform>>> Handle(SearchAdPlatformsQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Searching ad platforms for location: {Location}", request.Location);

            var platforms = _repository.SearchAdPlatformsToLocation(request.Location);

            if (platforms == null || platforms.Count == 0)
            {
                _logger.LogWarning("No platforms found for location: {Location}", request.Location);
                return Task.FromResult(Result<List<AdPlatform>>.Failure(
                    new Error("NotFound", $"No platforms found for location: {request.Location}")));
            }

            _logger.LogInformation("Found {Count} platforms for location: {Location}", platforms.Count, request.Location);
            return Task.FromResult(Result<List<AdPlatform>>.Success(platforms));
        }
    }
}
