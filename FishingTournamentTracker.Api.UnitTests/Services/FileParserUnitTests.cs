using FishingTournamentTracker.Api.Services;
using FishingTournamentTracker.Library.Models.DataModels;
using FluentAssertions;

namespace FishingTournamentTracker.Api.UnitTests.Services;

public class FileParserUnitTests
{
    private readonly IFileParser _fileParser;

    public FileParserUnitTests()
    {
        _fileParser = new FileParser();
    }

    [Fact]
    public void GenerateExcel_WithValidInput_ReturnsNonEmptyByteArray()
    {
        // Arrange
        var entities = GetTestEntities();

        // Act
        var result = _fileParser.GenerateExcel(entities);

        // Assert
        result.Should().NotBeNullOrEmpty();
    }

    [Fact]
    public void ParseExcel_WithValidInput_ReturnsListOfEntities()
    {
        // Arrange
        var expectedEntities = GetTestEntities();
        var entityBytes = _fileParser.GenerateExcel(expectedEntities);

        // Act
        var result = _fileParser.ParseExcel<TestEntity>(entityBytes);

        // Assert
        // Id does not get written on file upload
        result.Should().BeEquivalentTo(expectedEntities, options => options.Excluding(entity => entity.Id));
    }

    private static List<TestEntity> GetTestEntities()
    {
        return
        [
            new TestEntity
            {
                Id = Guid.NewGuid().ToString(),
                Name = Guid.NewGuid().ToString()
            },
            new TestEntity
            {
                Id = Guid.NewGuid().ToString(),
                Name = Guid.NewGuid().ToString()
            },
            new TestEntity
            {
                Id = Guid.NewGuid().ToString(),
                Name = Guid.NewGuid().ToString()
            },
            new TestEntity
            {
                Id = Guid.NewGuid().ToString(),
                Name = Guid.NewGuid().ToString()
            }
        ];
    }

    internal class TestEntity : IDatabaseEntity
    {
        public string? Id { get; set; }
        public string? Name { get; set; }
    }
}
