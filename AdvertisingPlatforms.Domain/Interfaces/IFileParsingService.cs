using AdvertisingPlatforms.Domain.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdvertisingPlatforms.Domain.Interfaces
{
    public interface IFileParsingService
    {
        public Task<List<AdPlatform>> ParsePlatformsAsync(Stream stream, CancellationToken cancellationToken, List<ErrorLine> errorLines);
    }
}
