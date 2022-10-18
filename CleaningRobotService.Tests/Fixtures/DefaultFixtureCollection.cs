using Xunit;

namespace CleaningRobotService.Tests.Fixtures;

[CollectionDefinition(nameof(DefaultFixtureCollection))]
public class DefaultFixtureCollection : ICollectionFixture<DatabaseFixture>
{
}
