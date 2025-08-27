using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdvertisingPlatforms.Domain.Entites
{
    public class AdPlatform
    {
        public string Name { get; set; }
        public List<string> Locations { get; set; }
    }
}
