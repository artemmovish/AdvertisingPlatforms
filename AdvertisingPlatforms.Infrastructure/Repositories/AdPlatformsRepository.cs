using AdvertisingPlatforms.Domain.Entites;
using AdvertisingPlatforms.Domain.Interfaces;
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

        public void LoadAdPlatforms(List<AdPlatform> adPlatforms)
        {

            _rwLock.EnterWriteLock();
            try
            {
                _locations = new Dictionary<string, LocationNode>();

                var root = new LocationNode { Name = "" };
                _locations[""] = root;

                foreach (var platform in adPlatforms)
                {
                    foreach (var location in platform.Locations)
                    {
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
            }
            finally
            {
                _rwLock.ExitWriteLock();
            }
        }

        public List<AdPlatform> SearchAdPlatformsToLocation(string location)
        {
            _rwLock.EnterReadLock();
            try
            {
                if (_locations == null || !_locations.ContainsKey("")) return new List<AdPlatform>();

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
                        break;
                    }

                    currentNode = currentNode.Children[segment];
                }

                foreach (var p in currentNode.Platforms)
                {
                    result.Add(p);
                }

                return result.ToList();
            }
            finally
            {
                _rwLock.ExitReadLock();
            }
            
        }
    }
}
