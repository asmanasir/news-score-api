namespace NewsScope.Api.Services;

public record ScoreRange(
    int MinExclusive,
    int MaxInclusive,
    int Score
);
