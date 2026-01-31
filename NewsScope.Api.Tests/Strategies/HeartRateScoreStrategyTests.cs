using System;
using NewsScope.Api.Strategies;
using Xunit;

public class HeartRateScoreStrategyTests
{
    private readonly HeartRateScoreStrategy _strategy = new();

    [Theory]
   
    [InlineData(40, 3)]  
    [InlineData(41, 1)]  
    [InlineData(50, 1)]  
    
  
    [InlineData(51, 0)]  
    [InlineData(90, 0)]  
    
   
    [InlineData(91, 1)]  
    [InlineData(110, 1)] 
    [InlineData(111, 2)] 
    [InlineData(200, 2)] 
    public void CalculateScore_BoundaryValues_ReturnExpectedScore(int value, int expected)
    {
        var score = _strategy.CalculateScore(value);
        Assert.Equal(expected, score);
    }

    [Theory]
    [InlineData(19)]  
    [InlineData(251)] 
    public void CalculateScore_PhysiologicalBoundaries_ThrowsException(int value)
    {
        Assert.Throws<ArgumentException>(() => _strategy.CalculateScore(value));
    }
}
