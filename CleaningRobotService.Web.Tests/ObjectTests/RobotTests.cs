using CleaningRobotService.Web.Objects;
using Xunit;

namespace CleaningRobotService.Web.Tests.ObjectTests;

public class RobotTests : BaseRobotTests
{
    [Fact]
    public void Robot_CalculatePointsVisited()
    {
        CalculatePointsVisitedTest(new Robot());
    }
    
    [Fact]
    public void Robot_CalculatePointsVisited_EnsureSameStepCountedOnce()
    {
        CalculatePointsVisited_EnsureSameStepCountedOnce(new Robot());
    }
}