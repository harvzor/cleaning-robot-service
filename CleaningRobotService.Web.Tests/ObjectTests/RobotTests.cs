using CleaningRobotService.Web.Objects;
using Xunit;

namespace CleaningRobotService.Web.Tests.ObjectTests;

public class RobotTests : BaseRobotTests
{
    [Fact]
    public void Robot_CalculatePointsVisited()
    {
        base.CalculatePointsVisitedTest(new Robot());
    }
    
    [Fact]
    public void Robot_CalculatePointsVisited_EnsureSameStepCountedOnce()
    {
        base.CalculatePointsVisited_EnsureSameStepCountedOnce(new Robot());
    }
}