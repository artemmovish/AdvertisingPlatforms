using AdvertisingPlatforms.Application.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdvertisingPlatforms.Application.DTOs
{
    public static class ErrorsDTO
    {
        public static Error PartialReadError => new("FileRead.Partial", "Partial file read error");
        public static Error FullReadError => new("FileRead.Full", "Complete file read error");
        public static Error InvalidFormat => new("FileRead.InvalidFormat", "Invalid file format");
    }
}
