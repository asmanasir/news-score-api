using NewsScope.Api.Models;

namespace NewsScope.Api.Services;

public interface INewsScoreService
{
	int CalculateScore(NewsRequestDto request);
}