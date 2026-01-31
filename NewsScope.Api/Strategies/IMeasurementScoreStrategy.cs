using NewsScope.Api.Models;

namespace NewsScope.Api.Strategies;

public interface IMeasurementScoreStrategy
{
    MeasurementType Type { get; }
    int CalculateScore(int value);
}
