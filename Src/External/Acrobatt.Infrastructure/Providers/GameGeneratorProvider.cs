using System.IO.Compression;
using System.Text.Json;
using Acrobatt.Application.Commons.Contracts.Providers;
using Acrobatt.Domain.Accounts.Entities;
namespace Acrobatt.Infrastructure.Providers;

public class GameGeneratorProvider : IGameGeneratorProvider
{
    private readonly IStorageProvider _storageProvider;

    public GameGeneratorProvider(IStorageProvider storageProvider)
    {
        _storageProvider = storageProvider;
    }

    public async Task<Stream> GenerateGameArchive(GameConfiguration? gameConfiguration, CancellationToken cancellationToken)
    {
        if (File.Exists("../game-server.zip"))
        {
            File.Delete("../game-server.zip");
        }
        
        Stream geoJsonFile = await _storageProvider.GetFileAsync(gameConfiguration.Map.MapName, cancellationToken);
        using MemoryStream geojsonFileMemoryStream = new MemoryStream();
        await geoJsonFile.CopyToAsync(geojsonFileMemoryStream, cancellationToken);
        await using Stream gameConfigurationJsonFile = File.Create("../game-server/game_config.json");

        await File.WriteAllBytesAsync("../game-server/map_config.json", geojsonFileMemoryStream.ToArray(), cancellationToken);
        await JsonSerializer.SerializeAsync(gameConfigurationJsonFile, gameConfiguration, cancellationToken: cancellationToken);
        
        geoJsonFile.Close();
        gameConfigurationJsonFile.Close();

        ZipFile.CreateFromDirectory("../game-server", "../game-server.zip");
        FileStream zipFile = File.Open("../game-server.zip", FileMode.Open);
        return zipFile;
    }
}