using System.Drawing;
using BenchmarkDotNet.Attributes;
using CleaningRobotService.Web.Dtos.Input;
using CleaningRobotService.Web.Enums;
using CleaningRobotService.Web.Interfaces;
using CleaningRobotService.Web.Objects;

namespace CleaningRobotService.Web.Benchmarks.ObjectBenchmarks;

[SimpleJob(warmupCount: 3, targetCount: 10)]
[MemoryDiagnoser(displayGenColumns: false)]
public class RobotBenchmarks
{
    private List<Command> _commands = new();
    
    [GlobalSetup]
    public void GlobalSetup()
    {
        List<Command> commands = new();

        int directionInt = 0;
        for (int i = 0; i < 10000; i++)
        {
            DirectionEnum direction = (DirectionEnum)directionInt;
            directionInt++;

            if (directionInt == 4)
                directionInt = 0;

            commands.Add(new Command
            {
                Direction = direction,
                Steps = 3,
            });
        }

        _commands = commands;
    }
    
    [Benchmark(Baseline = true)]
    public void RobotPoints_CalculatePointsVisited()
    {
        CalculatePointsVisited(new RobotPoints());
    }
    
    [Benchmark]
    // [Arguments(500)]
    // [Arguments(10000)]
    public void RobotGrid_CalculatePointsVisited(/* int gridWidth */)
    {
        // CalculatePointsVisited(new RobotGrid(gridWidth: gridWidth));
        CalculatePointsVisited(new RobotGrid());
    }
    
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
