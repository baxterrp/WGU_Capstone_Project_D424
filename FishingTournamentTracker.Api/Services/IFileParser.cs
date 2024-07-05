using FishingTournamentTracker.Library.Models.DataModels;

namespace FishingTournamentTracker.Api.Services;

public interface IFileParser
{
    IEnumerable<TEntity> ParseExcel<TEntity>(byte[] fileContents) where TEntity : IDatabaseEntity, new();
    byte[] GenerateExcel<TEntity>(IEnumerable<TEntity> entities);
}
