using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdvertisingPlatforms.Application.Common
{
    public class FileProcessingResult
    {
        public IReadOnlyList<string> FailedLines { get; init; } = Array.Empty<string>();

        public bool HasSuccess { get; init; }

        public bool IsPartialSuccess => HasSuccess && FailedLines.Any();

        public bool IsSuccessOnly => HasSuccess && !FailedLines.Any();
    }
}
