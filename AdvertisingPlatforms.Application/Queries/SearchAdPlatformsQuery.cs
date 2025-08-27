using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdvertisingPlatforms.Application.Common;
using AdvertisingPlatforms.Domain.Entites;
using MediatR;

namespace AdvertisingPlatforms.Application.Queries
{
    
    public record SearchAdPlatformsQuery(string Location) : IRequest<Result<List<AdPlatform>>>;

}
