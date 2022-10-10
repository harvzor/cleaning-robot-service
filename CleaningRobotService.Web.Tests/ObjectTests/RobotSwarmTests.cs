using CleaningRobotService.Web.Objects;
using Xunit;

namespace CleaningRobotService.Web.Tests.ObjectTests;

public class RobotSwarmTests : BaseRobotTests
{
    [Fact]
    public void RobotSwarm_CalculatePointsVisited()
    {
        base.CalculatePointsVisitedTest(new RobotSwarm(chunkCommands: 1));
    }
    
    [Fact]
    public void RobotSwarm_CalculatePointsVisited_EnsureSameStepCountedOnce()
    {
        base.CalculatePointsVisited_EnsureSameStepCountedOnce(new RobotSwarm(chunkCommands: 1));
    }
}