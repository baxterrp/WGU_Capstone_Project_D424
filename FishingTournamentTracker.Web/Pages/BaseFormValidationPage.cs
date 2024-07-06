using FishingTournamentTracker.Library.Models.DataModels;
using Microsoft.AspNetCore.Components;

namespace FishingTournamentTracker.Web.Pages;

public abstract class BaseFormValidationPage<TEntity> : ComponentBase where TEntity : IDatabaseEntity
{
    protected Dictionary<string, string>? InputValidationClasses { get; set; }

    protected BaseFormValidationPage()
    {
        var entityProperties = typeof(TEntity).GetProperties().Where(property => property.Name != "Id");
        InputValidationClasses = entityProperties.ToDictionary(property => property.Name, property => string.Empty);
    }

    protected bool CheckHasFormErrors()
    {
        return InputValidationClasses!.Any(validation => !string.IsNullOrWhiteSpace(validation.Value));
    }
}
