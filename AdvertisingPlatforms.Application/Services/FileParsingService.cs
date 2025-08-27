using AdvertisingPlatforms.Domain.Entites;
using AdvertisingPlatforms.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdvertisingPlatforms.Application.Services
{
    public class FileParsingService : IFileParsingService
    {
        public async Task<List<AdPlatform>> ParsePlatformsAsync(Stream stream, CancellationToken cancellationToken, List<ErrorLine> errorLines)
        {
            var root = new LocationNode { Name = "" };
            var platforms = new List<AdPlatform>();

            using var reader = new StreamReader(stream);
            int lineNumber = 0;

            while (!reader.EndOfStream)
            {
                cancellationToken.ThrowIfCancellationRequested();

                lineNumber++;
                var line = await reader.ReadLineAsync(); // асинхронное чтение
                if (string.IsNullOrWhiteSpace(line)) continue;

                try
                {
                    var parts = line.Split(':', 2);
                    if (parts.Length != 2) throw new FormatException("Нет ':' разделителя");

                    var platformName = parts[0].Trim();
                    if (string.IsNullOrEmpty(platformName)) throw new FormatException("Пустое имя площадки");

                    var locList = parts[1].Split(',', StringSplitOptions.RemoveEmptyEntries)
                                           .Select(l => l.Trim())
                                           .Where(l => !string.IsNullOrEmpty(l))
                                           .ToList();
                    if (locList.Count == 0) throw new FormatException("Нет локаций");

                    var platform = new AdPlatform
                    {
                        Name = platformName,
                        Locations = locList
                    };
                    platforms.Add(platform);

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
                catch
                {
                    errorLines.Add(new ErrorLine(lineNumber, line));
                }
            }

            return platforms;
        }

    }

}
