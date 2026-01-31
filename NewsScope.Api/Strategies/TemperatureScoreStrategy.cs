using NewsScope.Api.Models;

namespace NewsScope.Api.Strategies;

public class TemperatureScoreStrategy : IMeasurementScoreStrategy
{
    public MeasurementType Type => MeasurementType.TEMP;

    public int CalculateScore(int value)
    {
        
        if (value < 30 || value > 45)
            throw new ArgumentException("Temperature out of realistic range");

        if (value < 35) return 3;
        if (value <= 36) return 1;
        if (value <= 38) return 0;

        return 2; 
    }
}