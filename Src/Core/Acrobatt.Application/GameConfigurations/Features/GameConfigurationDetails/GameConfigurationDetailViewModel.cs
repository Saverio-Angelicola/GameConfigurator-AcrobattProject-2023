using System.ComponentModel.DataAnnotations.Schema;
using Acrobatt.Domain.Accounts.Enums;
using Acrobatt.Domain.Accounts.ValueObjects;

namespace Acrobatt.Application.GameConfigurations.Features.GameConfigurationDetails;

public class GameConfigurationDetailViewModel
{
    public int Id { get; set; }
    public string GameName { get; set; }
    public int MaxPlayers { get; set; }
    public int MaxFlags { get; set; }
    public double Duration { get; set; }
    public GameMode GameMode { get; set; }
    public bool IsPrivate { get; set; }
    public int MapId { get; set; }
    
    [NotMapped]
    public Position MapCenter { get; set; }
    
    [NotMapped]
    public List<TeamViewModel> Teams { get; set; }

    public GameConfigurationDetailViewModel(int id, string gameName, int maxPlayers, int maxFlags, double duration, GameMode gameMode, bool isPrivate, int mapId)
    {
        Id = id;
        GameName = gameName;
        MaxPlayers = maxPlayers;
        MaxFlags = maxFlags;
        Duration = duration;
        GameMode = gameMode;
        IsPrivate = isPrivate;
        MapId = mapId;
        Teams = new List<TeamViewModel>();
    }
}