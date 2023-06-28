namespace Acrobatt.Domain.Accounts.ValueObjects;

public record Position(float? X, float? Y)
{

    public static Position FromString(string position)
    {
        if (string.IsNullOrEmpty(position))
        {
            return new Position(null, null);
        }
        IEnumerable<float> posArray = position.Split(",").Select(Convert.ToSingle).ToArray();
        return new Position(posArray.First(), posArray.Last());
    }

    public override string ToString()
    {
        return $"{X},{Y}";
    }
};