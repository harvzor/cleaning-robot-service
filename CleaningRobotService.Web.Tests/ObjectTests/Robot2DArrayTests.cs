using CleaningRobotService.Web.Objects;
using Xunit;

namespace CleaningRobotService.Web.Tests.ObjectTests;

public class Robot2DArrayTests : BaseRobotTests
{
    [Fact]
    public void Robot2DArray_CalculatePointsVisited()
    {
        CalculatePointsVisitedTest(new Robot2DArray());
    }
    
    [Fact]
    public void Robot2DArray_SupportsNegativePoints()
    {
        CalculatePointsVisited_SupportsNegativePoints(new Robot2DArray());
    }
    
    [Fact]
    public void Robot2DArray_EnsureSameStepCountedOnce()
    {
        CalculatePointsVisited_EnsureSameStepCountedOnce(new Robot2DArray());
    }
}