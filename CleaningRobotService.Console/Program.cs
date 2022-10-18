using System.Drawing;
using CleaningRobotService.Common.Dtos.Input;
using CleaningRobotService.Common.Enums;
using CleaningRobotService.Common.Factories;
using CleaningRobotService.Common.Interfaces;
using CleaningRobotService.Console.Enums;

// 2
// 10 22
// E 2
// N 1

int numberOfCommands = int.Parse(Console.ReadLine()!);

// 10 22
int[] xAndY = Console
    .ReadLine()!
    .Split(' ')
    .Select(int.Parse)
    .ToArray();

Point startPoint = new Point(x: xAndY[0], y: xAndY[1]);
List<CommandDto> commands = new();

for (int i = 0; i < numberOfCommands; i++)
{
    // E 2
    // N 1
    string[] directionAndSteps = Console
        .ReadLine()!
        .Split(' ')
        .ToArray();
    
    commands.Add(new CommandDto
    {
        // TODO: write a test to ensure LetterDirectionEnum is mapped correctly to DirectionEnum.
        Direction = (DirectionEnum)Enum.Parse<LetterDirectionEnum>(directionAndSteps[0]),
        Steps = uint.Parse(directionAndSteps[1])
    });
}

IRobot robot = new RobotFactory()
    .GetRobot(startPoint: startPoint, commands: commands);

robot.CalculatePointsVisited();
int uniqueStepsCounted = robot.CountPointsVisited();

Console.WriteLine("Cleaned: " + uniqueStepsCounted);
