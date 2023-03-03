using CleaningRobotService.Common.Factories;
using CleaningRobotService.Common.Helpers;
using CleaningRobotService.Common.Robots;
using CleaningRobotService.Messenger;
using CleaningRobotService.RobotCommander.Messages;
using CleaningRobotService.Web.Messages;

namespace CleaningRobotService.RobotCommander.MessageHandlers;

public class CommandMessageHandler : IMessageHandler<CommandMessage>
{
    public void Handle(CommandMessage message)
    {
        int? result = null;
        
        IRobot robot = new RobotFactory()
            .GetRobot(startPoint: message.StartPoint, commands: message.Commands);

        TimeSpan calculationTime = MethodTimer.Measure(() =>
        {
            robot.CalculatePointsVisited();
            result = robot.CountPointsVisited();
        });

        CommandResultMessage commandResultMessage = new()
        {
            CalculationTime = calculationTime,
            PointsVisited = result!.Value,
        };
        
        // TODO: Send result back.
    }
}
