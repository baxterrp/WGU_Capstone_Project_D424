using FishingTournamentTracker.Library.Models.DataModels;
using Microsoft.AspNetCore.Components;

namespace FishingTournamentTracker.Web.Pages;

public abstract class BaseFishingTournamentView<TEntity> : ComponentBase where TEntity : IDatabaseEntity
{
    protected Dictionary<string, string>? InputValidationClasses { get; set; }
    protected bool Loading { get; set; } = false;

    protected BaseFishingTournamentView()
    {
        var entityProperties = typeof(TEntity).GetProperties().Where(property => property.Name != "Id");
        InputValidationClasses = entityProperties.ToDictionary(property => property.Name, property => string.Empty);
    }

    protected bool CheckHasFormErrors()
    {
        var hasErrors = InputValidationClasses!.Any(validation => !string.IsNullOrWhiteSpace(validation.Value));

        if (hasErrors)
        {
            Loading = false;
        }

        return hasErrors;
    }
}
