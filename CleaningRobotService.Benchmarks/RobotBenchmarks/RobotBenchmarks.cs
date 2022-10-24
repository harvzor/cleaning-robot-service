using System.Drawing;
using BenchmarkDotNet.Attributes;
using CleaningRobotService.Common.Dtos.Input;
using CleaningRobotService.Common.Enums;
using CleaningRobotService.Common.Robots;

namespace CleaningRobotService.Benchmarks.RobotBenchmarks;

[SimpleJob(warmupCount: 3, targetCount: 10)]
[MemoryDiagnoser(displayGenColumns: false)]
public class RobotBenchmarks
{
    private List<CommandDto> _commands = new();

    private List<CommandDto> GenerateCommands_LoopCommands()
    {
        List<CommandDto> commands = new();

        int directionInt = 0;
        for (int i = 0; i < 10000; i++)
        {
            DirectionEnum direction = (DirectionEnum)directionInt;
            directionInt++;

            if (directionInt == 4)
                directionInt = 0;

            commands.Add(new CommandDto
            {
                Direction = direction,
                Steps = 3,
            });
        }

        return commands;
    }
    
    private List<CommandDto> GenerateCommands_LoopOffset()
    {
        List<CommandDto> commands = new();

        int directionInt = 0;
        for (int i = 0; i < 10000; i++)
        {
            DirectionEnum direction = (DirectionEnum)directionInt;
            directionInt++;

            if (directionInt == 4)
                directionInt = 0;

            commands.Add(new CommandDto
            {
                Direction = direction,
                // Step one less south/west so the robot goes in circles but slightly up right each command loop.
                Steps = (uint)(direction is DirectionEnum.south or DirectionEnum.west
                    ? 2
                    : 3
                ),
            });
        }

        return commands;
    }
    
    private List<CommandDto> GenerateCommands_SpiralIn()
    {
        List<CommandDto> commands = new();

        int width = 500;
        int directionInt = 0;
        int directionOddCount = 0;
        for (int i = 0; i < width * 2; i++)
        {
            if (directionInt == 1 || directionInt == 3)
                directionOddCount++;
            
            DirectionEnum direction = (DirectionEnum)directionInt;
            directionInt++;

            if (directionInt == 4)
                directionInt = 0;

            commands.Add(new CommandDto
            {
                Direction = direction,
                Steps = i == 0
                    ? (uint)(width - 1)
                    : (uint)(width - directionOddCount),
            });
        }

        return commands;
    }
    
    [GlobalSetup]
    public void GlobalSetup()
    {
        // _commands = GenerateCommands_LoopCommands();
        _commands = GenerateCommands_LoopOffset();
        // _commands = GenerateCommands_SpiralIn();
    }

    [Benchmark(Baseline = true)]
    public void RobotPoints_CalculatePointsVisited()
    {
        CalculatePointsVisited<RobotPoints>();
    }
    
    [Benchmark]
    public void RobotLines_CalculatePointsVisited()
    {
        CalculatePointsVisited<RobotLines>();
    }
    
    [Benchmark]
    public void RobotDictionaryLines_CalculatePointsVisited()
    {
        CalculatePointsVisited<RobotDictionaryLines>();
    }

    [Benchmark]
    public void RobotGrid_CalculatePointsVisited_10()
    {
        CalculatePointsVisited<RobotGrid>(10);
    }
    
    [Benchmark]
    public void RobotGrid_CalculatePointsVisited_30()
    {
        CalculatePointsVisited<RobotGrid>(30);
    }
    
    [Benchmark]
    public void RobotGrid_CalculatePointsVisited_100()
    {
        CalculatePointsVisited<RobotGrid>(100);
    }
    
    [Benchmark]
    public void RobotGrid_CalculatePointsVisited_500()
    {
        CalculatePointsVisited<RobotGrid>(500);
    }
    
    private IRobot CreateRobot<TRobot>(Point startPoint, IEnumerable<CommandDto> commands, params object?[]? args) where TRobot : IRobot
    {
        object?[] newArgs = new object[]
        {
            startPoint,
            commands,
        };

        if (args != null)
            newArgs = newArgs.Union(args).ToArray();

        return (TRobot)Activator.CreateInstance(typeof(TRobot), newArgs)!;
    }

    private void CalculatePointsVisited<TRobot>(params object?[]? args) where TRobot : IRobot
    {
        Point startPoint = new Point
        {
            X = 0,
            Y = 0,
        };

        IRobot robot = CreateRobot<TRobot>(startPoint: startPoint, commands: _commands, args: args);

        robot.CalculatePointsVisited();

        robot.CountPointsVisited();
    }
}
