using AdvertisingPlatforms.Application.Common;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdvertisingPlatforms.Application.Commands
{
    public record UploadAdPlatformsCommand(Stream FileStream) : IRequest<Result<FileProcessingResult>>;
}
