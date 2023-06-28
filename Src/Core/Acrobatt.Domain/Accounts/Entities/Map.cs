using Acrobatt.Domain.Accounts.ValueObjects;
using Acrobatt.Domain.Commons;

namespace Acrobatt.Domain.Accounts.Entities;

public sealed class Map : Entity<int>
{
    public string MapName { get; private set; }
    public bool IsPublic { get; private set; }
    
    public Position MapCenter { get; private set; }


    public Map(string mapName, bool isPublic, Position mapCenter) : base(default)
    {
        MapName = mapName;
        IsPublic = isPublic;
        MapCenter = mapCenter;
    }
    
}