using NewsScope.Api.Models;

namespace NewsScope.Api.Strategies;

public class RespiratoryRateScoreStrategy : IMeasurementScoreStrategy
{
    public MeasurementType Type => MeasurementType.RR;

    public int CalculateScore(int value)
    {
        if (value < 5 || value > 60)
            throw new ArgumentException("Respiratory rate out of realistic range");

        if (value <= 8) return 3;
        if (value <= 11) return 1;
        if (value <= 20) return 0;
        if (value <= 24) return 2;

        return 3;
    }
}
