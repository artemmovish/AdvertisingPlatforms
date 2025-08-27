using AdvertisingPlatforms.Application.Common;
using AdvertisingPlatforms.Domain.Entites;
using AdvertisingPlatforms.Domain.Interfaces;
using MediatR;
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

        public SearchAdPlatformsQueryHandler(IAdPlatformsRepository repository)
        {
            _repository = repository;
        }

        public Task<Result<List<AdPlatform>>> Handle(SearchAdPlatformsQuery request, CancellationToken cancellationToken)
        {
            var platforms = _repository.SearchAdPlatformsToLocation(request.Location);

            if (platforms == null || platforms.Count == 0)
            {
                return Task.FromResult(Result<List<AdPlatform>>.Failure(
                    new Error("NotFound", $"No platforms found for location: {request.Location}")));
            }

            return Task.FromResult(Result<List<AdPlatform>>.Success(platforms));
        }
    }
}
