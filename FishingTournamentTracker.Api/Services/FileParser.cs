
using FishingTournamentTracker.Library.Models.DataModels;
using OfficeOpenXml;

namespace FishingTournamentTracker.Api.Services;

public class FileParser : IFileParser
{
    /// <summary>
    /// Will parse an excel sheet into an <see cref="IEnumerable{T}"/> of objects of type <see cref="IDatabaseEntity"/>
    /// Make sure the properties in the entity class are in the same order as the excel sheet columns, class props are top -> down and excel sheet is left -> right 
    /// </summary>
    /// <typeparam name="TEntity">An object of type <see cref="IDatabaseEntity"/></typeparam>
    /// <param name="fileContents">Base64 encoded string representing an excel file</param>
    /// <returns><see cref="IEnumerable{T}"/> of <see cref="IDatabaseEntity"/></returns>
    public IEnumerable<TEntity> ParseExcel<TEntity>(byte[] fileContents) where TEntity : IDatabaseEntity, new()
    {
        var entities = new List<TEntity>();
        var properties = typeof(TEntity).GetProperties().Where(prop => prop.Name != nameof(IDatabaseEntity.Id)).ToList();

        // EPPlus is free for educational purposes (Non Commercial)
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

        using var stream = new MemoryStream(fileContents);
        using var package = new ExcelPackage(stream);

        var worksheet = package.Workbook.Worksheets[0];

        // rows start at 1, first row is header
        for (var row = 2; row <= worksheet.Dimension.End.Row; row++)
        {
            var entity = new TEntity();
            var column = 1;

            foreach (var property in properties)
            {
                var propertyType = property.PropertyType;
                var cellValue = worksheet.Cells[row, column++].Text;

                object parsedValue = propertyType switch
                {
                    var dataType when dataType == typeof(DateTime?) => DateTime.Parse(cellValue),
                    var dataType when dataType == typeof(int?) => int.Parse(cellValue),

                    // defaulting to string since its already parsed as a string
                    _ => cellValue
                };

                property.SetValue(entity, parsedValue);
            }

            entities.Add(entity);
        }

        return entities;
    }
}
