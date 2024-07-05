using Dapper;
using FishingTournamentTracker.Api.Configuration;
using FishingTournamentTracker.Library.Models.DataModels;
using Microsoft.Extensions.Options;
using System.Data.SqlClient;
using System.Reflection;
using System.Text;

namespace FishingTournamentTracker.Api.Repositories;

public abstract class BaseRepository(IOptions<DatabaseConfiguration> databaseConfiguration)
{
    protected async Task<TEntity> Insert<TEntity>(TEntity entity) where TEntity : IDatabaseEntity
    {
        entity.Id = Guid.NewGuid().ToString();
        using var connection = new SqlConnection(databaseConfiguration.Value.ConnectionString);
        return await connection.QuerySingleAsync<TEntity>(BuildAddCommand(entity), entity);
    }

    protected async Task<TEntity> Update<TEntity>(TEntity entity) where TEntity : IDatabaseEntity
    {
        using var connection = new SqlConnection(databaseConfiguration.Value.ConnectionString);
        return await connection.QuerySingleAsync<TEntity>(BuildUpdateCommand(entity), entity);
    }

    protected async Task<IEnumerable<TEntity>> Search<TEntity>(DynamicParameters dynamicParameters) where TEntity : IDatabaseEntity
    {
        using var connection = new SqlConnection(databaseConfiguration.Value.ConnectionString);
        return await connection.QueryAsync<TEntity>(BuildSearchQuery<TEntity>(dynamicParameters), dynamicParameters);
    }

    public async Task<TEntity> FindById<TEntity>(string entityId)
    {
        using var connection = new SqlConnection(databaseConfiguration.Value.ConnectionString);
        return await connection.QuerySingleAsync<TEntity>(BuildFindByIdQuery<TEntity>(), new { Id = entityId });
    }
    private static string BuildSearchQuery<TEntity>(DynamicParameters dynamicParameters)
    {
        var stringBuilder = new StringBuilder();
        stringBuilder.Append("SELECT * FROM ");
        stringBuilder.Append($"[dbo].[{typeof(TEntity).Name}]");
        
        if (dynamicParameters.ParameterNames.Any())
        {
            stringBuilder.Append(" WHERE ");
            stringBuilder.Append(string.Join(" AND ", dynamicParameters.ParameterNames.Select(parameter => $"[{parameter}] = @{parameter}")));
        }

        return stringBuilder.ToString();
    }

    private static string BuildFindByIdQuery<TEntity>()
    {
        var stringBuilder = new StringBuilder();
        stringBuilder.Append("SELECT * FROM ");
        stringBuilder.Append($"[dbo].[{typeof(TEntity).Name}]");
        stringBuilder.Append(" WHERE [Id] = @Id");

        return stringBuilder.ToString();
    }

    private static string BuildUpdateCommand<TEntity>(TEntity entity)
    {
        var properties = GetPropertyInfo(entity);
        var output = string.Join(",", properties.Select(x => $"INSERTED.{x.Name}"));
        var sets = string.Join(",", properties.Select(x => $"{x.Name} = @{x.Name}"));

        var stringBuilder = new StringBuilder();
        stringBuilder.Append("UPDATE ");
        stringBuilder.Append($"[dbo].[{typeof(TEntity).Name}]");
        stringBuilder.Append(" SET ");
        stringBuilder.Append(sets);
        stringBuilder.Append(" OUTPUT ");
        stringBuilder.Append(output);
        stringBuilder.Append(" WHERE [Id] = @Id");

        return stringBuilder.ToString();
    }

    private static string BuildAddCommand<TEntity>(TEntity entity)
    {
        var properties = GetPropertyInfo(entity);
        var columns = string.Join(",", properties.Select(x => x.Name));
        var output = string.Join(",", properties.Select(x => $"INSERTED.{x.Name}"));
        var values = string.Join(",", properties.Select(x => $"@{x.Name}"));

        var stringBuilder = new StringBuilder();
        stringBuilder.Append("INSERT INTO ");
        stringBuilder.Append($"[dbo].[{typeof(TEntity).Name}]");
        stringBuilder.Append(" (");
        stringBuilder.Append(columns);
        stringBuilder.Append(") OUTPUT ");
        stringBuilder.Append(output);
        stringBuilder.Append(" VALUES (");
        stringBuilder.Append(values);
        stringBuilder.Append(')');

        return stringBuilder.ToString();
    }

    private static PropertyInfo[] GetPropertyInfo<TEntity>(TEntity entity) => entity!.GetType().GetProperties();
}
