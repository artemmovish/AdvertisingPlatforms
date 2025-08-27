using AdvertisingPlatforms.Domain.Entites;
using AdvertisingPlatforms.Domain.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdvertisingPlatforms.Infrastructure.Repositories
{
    public class AdPlatformsRepository : IAdPlatformsRepository
    {
        private Dictionary<string, LocationNode> _locations;
        private readonly ReaderWriterLockSlim _rwLock = new ReaderWriterLockSlim();
        private readonly ILogger<AdPlatformsRepository> _logger;

        public AdPlatformsRepository(ILogger<AdPlatformsRepository> logger)
        {
            _logger = logger;
            _locations = new Dictionary<string, LocationNode>();
            _locations[""] = new LocationNode { Name = "" };
        }

        public void LoadAdPlatforms(List<AdPlatform> adPlatforms)
        {
            _logger.LogInformation("Loading {PlatformCount} ad platforms to repository", adPlatforms.Count);

            _rwLock.EnterWriteLock();
            try
            {
                _locations = new Dictionary<string, LocationNode>();
                var root = new LocationNode { Name = "" };
                _locations[""] = root;
                int totalLocations = 0;

                foreach (var platform in adPlatforms)
                {
                    foreach (var location in platform.Locations)
                    {
                        totalLocations++;
                        var segments = location.Split('/', StringSplitOptions.RemoveEmptyEntries);
                        var currentNode = root;

                        foreach (var segment in segments)
                        {
                            if (!currentNode.Children.ContainsKey(segment))
                            {
                                currentNode.Children[segment] = new LocationNode { Name = segment };
                            }
                            currentNode = currentNode.Children[segment];
                        }

                        currentNode.Platforms.Add(platform);
                    }
                }

                _logger.LogInformation("Repository loaded successfully. Platforms: {PlatformCount}, Total locations: {LocationCount}",
                    adPlatforms.Count, totalLocations);
            }
            finally
            {
                _rwLock.ExitWriteLock();
            }
        }

        public List<AdPlatform> SearchAdPlatformsToLocation(string location)
        {
            _logger.LogDebug("Searching platforms for location: {Location}", location);

            _rwLock.EnterReadLock();
            try
            {
                if (_locations == null || !_locations.ContainsKey(""))
                {
                    _logger.LogWarning("Repository not initialized or empty");
                    return new List<AdPlatform>();
                }

                var root = _locations[""];
                var segments = location.Split('/', StringSplitOptions.RemoveEmptyEntries);
                var currentNode = root;
                var result = new HashSet<AdPlatform>();

                foreach (var segment in segments)
                {
                    foreach (var p in currentNode.Platforms)
                    {
                        result.Add(p);
                    }

                    if (!currentNode.Children.ContainsKey(segment))
                    {
                        _logger.LogDebug("Location segment '{Segment}' not found in hierarchy", segment);
                        break;
                    }

                    currentNode = currentNode.Children[segment];
                }

                foreach (var p in currentNode.Platforms)
                {
                    result.Add(p);
                }

                _logger.LogDebug("Found {PlatformCount} platforms for location: {Location}", result.Count, location);
                return result.ToList();
            }
            finally
            {
                _rwLock.ExitReadLock();
            }
        }
    }
}
