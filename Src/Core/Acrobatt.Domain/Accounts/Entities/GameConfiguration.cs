using Acrobatt.Domain.Accounts.Enums;
using Acrobatt.Domain.Accounts.Exceptions;
using Acrobatt.Domain.Commons;

namespace Acrobatt.Domain.Accounts.Entities;

public sealed class GameConfiguration : Entity<int>
{
    public string GameName { get; private set; }
    public int MaxPlayers { get; private set; }
    
    public int MaxFlags { get; private set; }

    public double Duration { get; private set; }
    
    public GameMode GameMode { get; private set; }

    public bool IsPrivate { get; private set; }

    public Map? Map { get; private set; }
    
    public List<Team> Teams { get; private set; }
    
    public GameConfiguration(int id, string gameName, int maxPlayers, int maxFlags, double duration, GameMode gameMode, bool isPrivate) : base(id)
    {
        GameName = gameName;
        MaxPlayers = maxPlayers;
        MaxFlags = maxFlags;
        Duration = duration;
        GameMode = gameMode;
        IsPrivate = isPrivate;
        Teams = new List<Team>();
    }

    public GameConfiguration(int id, string gameName, int maxPlayers, int maxFlags, double duration, GameMode gameMode, bool isPrivate,Map map, List<Team> teams) : base(id)
    {
        GameName = gameName;
        MaxPlayers = maxPlayers;
        MaxFlags = maxFlags;
        Duration = duration;
        GameMode = gameMode;
        IsPrivate = isPrivate;
        Teams = teams;
        Map = map;
    }

    public static GameConfiguration Create(string gameName, int maxPlayers, int maxFLags, double duration, GameMode gameMode, bool isPrivate, Map map, List<Team> teams)
    {
        if (teams.Count < 2)
        {
            throw new NotEnoughTeamsException();
        }
        
        return new GameConfiguration(default, gameName, maxPlayers, maxFLags, duration, gameMode, isPrivate, map, new List<Team>())
        {
            Teams = teams
        };
    }
}