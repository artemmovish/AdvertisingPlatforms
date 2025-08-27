using AdvertisingPlatforms.Domain.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdvertisingPlatforms.Domain.Interfaces
{
    public interface IAdPlatformsRepository
    {
        public void LoadAdPlatforms(List<AdPlatform> adPlatforms);

        public List<AdPlatform> SearchAdPlatformsToLocation(string location);
    }
}
