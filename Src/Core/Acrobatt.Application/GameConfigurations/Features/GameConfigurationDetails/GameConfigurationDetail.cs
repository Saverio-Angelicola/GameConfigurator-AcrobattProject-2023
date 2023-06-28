using Acrobatt.Application.Commons.Configs.Query;

namespace Acrobatt.Application.GameConfigurations.Features.GameConfigurationDetails;

public class GameConfigurationDetail : IQuery
{
    public int Id { get; set; }

    public GameConfigurationDetail(int id)
    {
        Id = id;
    }
}