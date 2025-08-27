using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdvertisingPlatforms.Application.Common
{
    public record Error(string Code, string Description)
    {
        public static Error None => new(string.Empty, string.Empty);
    }
}
