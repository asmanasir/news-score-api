using System;
using NewsScope.Api.Strategies;
using Xunit;

public class TemperatureScoreStrategyTests
{
    private readonly TemperatureScoreStrategy _strategy = new();

    [Theory]
    [InlineData(34, 3)]
    [InlineData(36, 1)]
    [InlineData(37, 0)]
    [InlineData(39, 2)]
    public void CalculateScore_ValidValues_ReturnExpectedScore(int value, int expected)
    {
        var score = _strategy.CalculateScore(value);
        Assert.Equal(expected, score);
    }

    [Theory]
    [InlineData(20)]
    [InlineData(50)]
    public void CalculateScore_InvalidValues_ThrowsException(int value)
    {
        Assert.Throws<ArgumentException>(() => _strategy.CalculateScore(value));
    }
}
