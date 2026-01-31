using NewsScope.Api.Models;

namespace NewsScope.Api.Strategies;

public class HeartRateScoreStrategy : IMeasurementScoreStrategy
{
    public MeasurementType Type => MeasurementType.HR;

    public int CalculateScore(int value)
    {
        if (value < 20 || value > 250)
            throw new ArgumentException("Heart rate out of realistic range");

        if (value <= 40) return 3;
        if (value <= 50) return 1;
        if (value <= 90) return 0;
        if (value <= 110) return 1;

        return 2;
    }
}
