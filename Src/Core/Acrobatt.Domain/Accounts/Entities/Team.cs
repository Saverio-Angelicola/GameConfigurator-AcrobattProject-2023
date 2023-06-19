using Acrobatt.Domain.Commons;

namespace Acrobatt.Domain.Accounts.Entities;

public sealed class Team : Entity<int>
{

    public string Color { get; private set; }
    public int NbPlayer { get; private set; }
    
    public string Name { get; private set; }

    public Team(int id, string color, int nbPlayer, string name) : base(id)
    {
        Color = color;
        NbPlayer = nbPlayer;
        Name = name;
    }

    public Team(string color, int nbPlayer, string name) : base(default)
    {
        Color = color;
        NbPlayer = nbPlayer;
        Name = name;
    }
    
}