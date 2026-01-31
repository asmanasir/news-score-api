using System;
using System.Collections.Generic;
using NewsScope.Api.Models;
using NewsScope.Api.Services;
using NewsScope.Api.Strategies;
using Xunit;

public class NewsScoreServiceTests
{
    private readonly INewsScoreService _service;

    public NewsScoreServiceTests()
    {
        var strategies = new List<IMeasurementScoreStrategy>
        {
            new TemperatureScoreStrategy(),
            new HeartRateScoreStrategy(),
            new RespiratoryRateScoreStrategy()
        };

        _service = new NewsScoreService(strategies);
    }

    [Fact]
    public void CalculateScore_ValidMeasurements_ReturnsExpectedScore()
    {
        var request = new NewsRequestDto
        {
            Measurements = new List<MeasurementDto>
            {
                new() { Type = MeasurementType.TEMP, Value = 37 }, 
                new() { Type = MeasurementType.HR, Value = 60 },   
                new() { Type = MeasurementType.RR, Value = 5 }     
            }
        };

        var score = _service.CalculateScore(request);

        Assert.Equal(3, score);
    }

    [Fact]
    public void CalculateScore_MissingMeasurement_ThrowsException()
    {
        var request = new NewsRequestDto
        {
            Measurements = new List<MeasurementDto>
            {
                new() { Type = MeasurementType.TEMP, Value = 37 },
                new() { Type = MeasurementType.HR, Value = 60 }
            }
        };

        var ex = Assert.Throws<ArgumentException>(() => _service.CalculateScore(request));
        Assert.Contains("Missing required measurements", ex.Message);
    }

    [Fact]
    public void CalculateScore_DuplicateMeasurement_ThrowsException()
    {
        var request = new NewsRequestDto
        {
            Measurements = new List<MeasurementDto>
            {
                new() { Type = MeasurementType.TEMP, Value = 37 },
                new() { Type = MeasurementType.TEMP, Value = 38 }, // Duplicate
                new() { Type = MeasurementType.RR, Value = 10 }
            }
        };

        Assert.Throws<ArgumentException>(() => _service.CalculateScore(request));
    }

    [Fact]
    public void CalculateScore_InvalidTemperature_ThrowsException()
    {
        var request = new NewsRequestDto
        {
            Measurements = new List<MeasurementDto>
            {
                new() { Type = MeasurementType.TEMP, Value = 100 }, 
                new() { Type = MeasurementType.HR, Value = 60 },
                new() { Type = MeasurementType.RR, Value = 10 }
            }
        };

        Assert.Throws<ArgumentException>(() => _service.CalculateScore(request));
    }

        [Fact]
    public void CalculateScore_UnsupportedMeasurement_ThrowsException()
    {
        var request = new NewsRequestDto
        {
            Measurements = new List<MeasurementDto>
            {
                new() { Type = MeasurementType.TEMP, Value = 37 },
                new() { Type = MeasurementType.HR, Value = 60 },
                new() { Type = MeasurementType.RR, Value = 12 },
                new() { Type = (MeasurementType)99, Value = 95 } 
            }
        };

        var ex = Assert.Throws<ArgumentException>(() => _service.CalculateScore(request));

           Assert.Contains("unsupported types", ex.Message);
    }
}
