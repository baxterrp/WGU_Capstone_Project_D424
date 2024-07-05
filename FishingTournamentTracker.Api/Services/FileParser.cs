
using FishingTournamentTracker.Library.Models.DataModels;
using OfficeOpenXml;
using System.Reflection;

namespace FishingTournamentTracker.Api.Services;

public class FileParser : IFileParser
{
    public FileParser()
    {
        // EPPlus is free for educational purposes (Non Commercial)
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <param name="entities"></param>
    /// <returns></returns>
    public byte[] GenerateExcel<TEntity>(IEnumerable<TEntity> entities)
    {
        var properties = GetPropertyInfo<TEntity>();

        using var package = new ExcelPackage();
        var worksheet = package.Workbook.Worksheets.Add(nameof(TEntity));
        var row = 1;

        // write header row
        for (int headerColumn = 1; headerColumn <= properties.Length; headerColumn++)
        {
            worksheet.Cells[row, headerColumn].Value = properties[headerColumn - 1].Name;
        }

        // move to second row for data
        row++;

        foreach (var entity in entities)
        {
            var entityColumn = 1;

            foreach (var property in properties)
            {
                worksheet.Cells[row, entityColumn++].Value = property.GetValue(entity);
            }

            // increment row for each entity
            row++;
        }

        // autofit columns to length of longest value
        worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();

        using var stream = new MemoryStream();
        package.SaveAs(stream);

        stream.Position = 0;

        return stream.ToArray();
    }

    /// <summary>
    /// Will parse an excel sheet into an <see cref="IEnumerable{T}"/> of objects of type <see cref="IDatabaseEntity"/>
    /// Make sure the properties in the entity class are in the same order as the excel sheet columns, class props are top -> down and excel sheet is left -> right 
    /// </summary>
    /// <typeparam name="TEntity">An object of type <see cref="IDatabaseEntity"/></typeparam>
    /// <param name="fileContents">Base64 encoded string representing an excel file</param>
    /// <returns><see cref="IEnumerable{T}"/> of <see cref="IDatabaseEntity"/></returns>
    public IEnumerable<TEntity> ParseExcel<TEntity>(byte[] fileContents) where TEntity : IDatabaseEntity, new()
    {
        using var stream = new MemoryStream(fileContents);
        using var package = new ExcelPackage(stream);

        var worksheet = package.Workbook.Worksheets[0];
        var entities = new List<TEntity>();

        // rows start at 1, first row is header
        for (var row = 2; row <= worksheet.Dimension.End.Row; row++)
        {
            var entity = new TEntity();
            var column = 1;

            foreach (var property in GetPropertyInfo<TEntity>())
            {
                var cellValue = worksheet.Cells[row, column++].Text;

                object parsedValue = property.PropertyType switch
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

    private static PropertyInfo[] GetPropertyInfo<TEntity>()
    {
        return typeof(TEntity)
            .GetProperties()
            .Where(prop => prop.Name != nameof(IDatabaseEntity.Id))
            .ToArray();
    }
}
