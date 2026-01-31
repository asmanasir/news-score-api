using NewsScope.Api.Models;
using NewsScope.Api.Strategies;

namespace NewsScope.Api.Services;

public class NewsScoreService : INewsScoreService
{
    private readonly Dictionary<MeasurementType, IMeasurementScoreStrategy> _strategies;

    public NewsScoreService(IEnumerable<IMeasurementScoreStrategy> strategies)
    {
           _strategies = strategies.ToDictionary(s => s.Type);
    }

    public int CalculateScore(NewsRequestDto request)
    {
        Validate(request);

        return request.Measurements.Sum(m =>
            _strategies[m.Type].CalculateScore(m.Value)
        );
    }

    private void Validate(NewsRequestDto request)
    {
        if (request?.Measurements == null || !request.Measurements.Any())
        {
            throw new ArgumentException("Measurements are required.");
        }
        
        var incomingTypes = request.Measurements
            .Select(m => m.Type)
            .ToHashSet();

        if (incomingTypes.Count != request.Measurements.Count)
        {
            throw new ArgumentException("Duplicate measurement types are not allowed.");
        }

        var missingTypes = _strategies.Keys
            .Where(required => !incomingTypes.Contains(required))
            .ToList();

        if (missingTypes.Any())
        {
            var missingList = string.Join(", ", missingTypes);
            throw new ArgumentException($"Missing required measurements: {missingList}");
        }

        var unsupportedTypes = incomingTypes
            .Where(t => !_strategies.ContainsKey(t))
            .ToList();

        if (unsupportedTypes.Any())
        {
            var unsupportedList = string.Join(", ", unsupportedTypes);
            throw new ArgumentException($"Measurements contain unsupported types: {unsupportedList}");
        }
    }
}
