using System;
using NewsScope.Api.Strategies;
using Xunit;

public class RespiratoryRateScoreStrategyTests
{
    private readonly RespiratoryRateScoreStrategy _strategy = new();

    [Theory]
    [InlineData(8, 3)]
    [InlineData(10, 1)]
    [InlineData(16, 0)]
    [InlineData(22, 2)]
    [InlineData(30, 3)]
    public void CalculateScore_ValidValues_ReturnExpectedScore(int value, int expected)
    {
        var score = _strategy.CalculateScore(value);
        Assert.Equal(expected, score);
    }

    [Theory]
    [InlineData(2)]
    [InlineData(80)]
    public void CalculateScore_InvalidValues_ThrowsException(int value)
    {
        Assert.Throws<ArgumentException>(() => _strategy.CalculateScore(value));
    }
}
