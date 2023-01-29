using System.Drawing;
using BenchmarkDotNet.Attributes;
using CleaningRobotService.Common.Dtos.Input;
using CleaningRobotService.Common.Robots;
using CleaningRobotService.Tests;

namespace CleaningRobotService.Benchmarks.RobotBenchmarks;

[SimpleJob(warmupCount: 3, targetCount: 10)]
[MemoryDiagnoser(displayGenColumns: false)]
public class RobotBenchmarks
{
    private List<DirectionStep> _commands = new();
    
    [GlobalSetup]
    public void GlobalSetup()
    {
        // _commands = CommandGenerator.LoopCommands(steps: 3);
        _commands = CommandGenerator.LoopOffset();
        // _commands = CommandGenerator.SpiralIn(width: 500);
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
    
    private IRobot CreateRobot<TRobot>(Point startPoint, IEnumerable<DirectionStep> commands, params object?[]? args) where TRobot : IRobot
    {
        object?[] newArgs = {
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
