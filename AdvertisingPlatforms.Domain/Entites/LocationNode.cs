using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdvertisingPlatforms.Domain.Entites
{
    public class LocationNode
    {
        public string Name { get; set; }
        public List<AdPlatform> Platforms { get; set; } = new();
        public Dictionary<string, LocationNode> Children { get; set; } = new();
    }
}
