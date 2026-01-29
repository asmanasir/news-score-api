using NewsScope.Api.Models;

namespace NewsScope.Api.Services;

public class NewsScoreService : INewsScoreService
{
    public int CalculateScore(NewsRequestDto request)
    {
        ValidateRequest(request);

        int totalScore = 0;

        foreach (var measurement in request.Measurements)
        {
            totalScore += CalculateMeasurementScore(measurement);
        }

        return totalScore;
    }

    private static void ValidateRequest(NewsRequestDto request)
    {
        if (request.Measurements == null || request.Measurements.Count == 0)
            throw new ArgumentException("Measurements are required.");

        var requiredTypes = new[]
        {
            MeasurementType.TEMP,
            MeasurementType.HR,
            MeasurementType.RR
        };

        foreach (var type in requiredTypes)
        {
            if (!request.Measurements.Any(m => m.Type == type))
                throw new ArgumentException($"Missing measurement: {type}");
        }

        var duplicateTypes = request.Measurements
            .GroupBy(m => m.Type)
            .Where(g => g.Count() > 1);

        if (duplicateTypes.Any())
            throw new ArgumentException("Duplicate measurement types are not allowed.");
    }

    private static readonly List<ScoreRange> TempRules = new()
    {
        new(31, 35, 3),
        new(35, 36, 1),
        new(36, 38, 0),
        new(38, 39, 1),
        new(39, 42, 2),
    };

    private static readonly List<ScoreRange> HrRules = new()
    {
        new(25, 40, 3),
        new(40, 50, 1),
        new(50, 90, 0),
        new(90, 110, 1),
        new(110, 130, 2),
        new(130, 220, 3),
    };

    private static readonly List<ScoreRange> RrRules = new()
    {
        new(3, 8, 3),
        new(8, 11, 1),
        new(11, 20, 0),
        new(20, 24, 2),
        new(24, 60, 3),
    };

    private static int CalculateMeasurementScore(MeasurementDto measurement)
    {
        var rules = measurement.Type switch
        {
            MeasurementType.TEMP => TempRules,
            MeasurementType.HR => HrRules,
            MeasurementType.RR => RrRules,
            _ => throw new ArgumentOutOfRangeException()
        };

        var rule = rules.FirstOrDefault(r =>
            measurement.Value > r.MinExclusive &&
            measurement.Value <= r.MaxInclusive);

        if (rule == null)
            throw new ArgumentException(
                $"Invalid value {measurement.Value} for {measurement.Type}");

        return rule.Score;
    }
}
