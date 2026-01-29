namespace NewsScope.Api.Models;

public class NewsRequestDto
{
    public List<MeasurementDto> Measurements { get; set; } = new();
}
