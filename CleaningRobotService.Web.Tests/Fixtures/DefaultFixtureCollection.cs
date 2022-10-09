using Xunit;

namespace CleaningRobotService.Web.Tests.Fixtures;

[CollectionDefinition(nameof(DefaultFixtureCollection))]
public class DefaultFixtureCollection : ICollectionFixture<DatabaseFixture>
{
}
