using System.Drawing;
using BenchmarkDotNet.Attributes;
using CleaningRobotService.Common.Dtos.Input;
using CleaningRobotService.Common.Enums;
using CleaningRobotService.Common.Robots;

namespace CleaningRobotService.Benchmarks.ObjectBenchmarks;

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
        CalculatePointsVisited(new RobotPoints());
    }
    
    [Benchmark]
    public void RobotLines_CalculatePointsVisited()
    {
        CalculatePointsVisited(new RobotLines());
    }
    
    [Benchmark]
    public void RobotDictionaryLines_CalculatePointsVisited()
    {
        CalculatePointsVisited(new RobotDictionaryLines());
    }

    // [Benchmark]
    // public void RobotGrid_CalculatePointsVisited_10()
    // {
    //     CalculatePointsVisited(new RobotGrid(gridWidth: 10));
    // }
    //
    // [Benchmark]
    // public void RobotGrid_CalculatePointsVisited_30()
    // {
    //     CalculatePointsVisited(new RobotGrid(gridWidth: 30));
    // }
    //
    // [Benchmark]
    // public void RobotGrid_CalculatePointsVisited_100()
    // {
    //     CalculatePointsVisited(new RobotGrid(gridWidth: 100));
    // }
    //
    // [Benchmark]
    // public void RobotGrid_CalculatePointsVisited_500()
    // {
    //     CalculatePointsVisited(new RobotGrid(gridWidth: 500));
    // }
    
    // [Benchmark]
    // [Arguments(30)]
    // [Arguments(500)]
    // public void RobotGrid_CalculatePointsVisited(int gridWidth)
    // {
    //     CalculatePointsVisited(new RobotGrid(gridWidth: gridWidth));
    // }
    
    // [Benchmark]
    // public void RobotSwarm_CalculatePointsVisited_1000()
    // {
    //     CalculatePointsVisited(new RobotSwarm(chunkCommands: 1000));
    // }
    //
    // [Benchmark]
    // public void RobotSwarm_CalculatePointsVisited_500()
    // {
    //     CalculatePointsVisited(new RobotSwarm(chunkCommands: 500));
    // }
    //
    // [Benchmark]
    // public void RobotSwarm_CalculatePointsVisited_100()
    // {
    //     CalculatePointsVisited(new RobotSwarm(chunkCommands: 500));
    // }
    
    private void CalculatePointsVisited(IRobot robot)
    {
        robot.StartPoint = new Point
        {
            X = 0,
            Y = 0,
        };

        robot.Commands = _commands;

        robot.CalculatePointsVisited();

        robot.CountPointsVisited();
    }
}
