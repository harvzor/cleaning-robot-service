using System.Drawing;
using System.Numerics;
using CleaningRobotService.Common.Enums;
using CleaningRobotService.Common.Structures;

namespace CleaningRobotService.Common.Collections;

public class LineDictionary : IPointsCollections
{
    private int _count = 0;
    
    /// <summary>
    /// Second Key is the x or y coordinate. Values are the index of the line in the <see cref="_lines"/>.
    /// </summary>
    private readonly Dictionary<(PlaneEnum, int), List<int>> _dictionary;
    private readonly List<Line> _lines;

    public LineDictionary(int numberOfExpectedCommands = 0)
    {
        // Capacity here isn't likely the be the same as the number of commands if the commands cause the robot to
        // travel the same lines.
        _dictionary = new Dictionary<(PlaneEnum, int), List<int>>(numberOfExpectedCommands);
        _lines = new List<Line>(numberOfExpectedCommands);
    }

    public void AddLine(Line line, DirectionEnum direction)
    {
        // TODO: Can just calculate the plane based on the start/end.
        PlaneEnum planeEnum = direction is DirectionEnum.west or DirectionEnum.east
            ? PlaneEnum.Horizontal
            : PlaneEnum.Vertical;
        int key = planeEnum == PlaneEnum.Horizontal
            ? line.Start.Y
            : line.Start.X;

        _lines.Add(line);

        if (_dictionary.TryGetValue((planeEnum, key), out List<int>? lineIndexes))
        {
            lineIndexes.Add(_lines.Count - 1);
        }
        else
        {
            _dictionary
                .Add((planeEnum, key), new List<int>
                {
                    _lines.Count - 1,
                });
        }
    }
    
    public void AddPoint(Point point)
    {
        if (_dictionary.TryGetValue((PlaneEnum.Horizontal, point.Y), out List<int>? matchingHorizontalLineIndexes))
        {
            foreach (int index in matchingHorizontalLineIndexes)
            {
                Line line = _lines[index];
                    
                if (line.Start.X < line.End.X
                        ? point.X >= line.Start.X && point.X <= line.End.X
                        : point.X <= line.Start.X && point.X >= line.End.X)
                {
                    return;
                }
            }
        }
            
        if (_dictionary.TryGetValue((PlaneEnum.Vertical, point.X), out List<int>? matchingVerticalLineIndexes))
        {
            foreach (int index in matchingVerticalLineIndexes)
            {
                Line line = _lines[index];
                    
                if (line.Start.Y < line.End.Y
                        ? point.Y >= line.Start.Y && point.Y <= line.End.Y
                        : point.Y <= line.Start.Y && point.Y >= line.End.Y)
                {
                    return;
                }
            }
        }
        
        _count++;
    }

    public IEnumerable<Point> GetPoints()
    {
        return _lines
            .SelectMany(line => line.CalculatePoints())
            .Distinct();
    }

    public int Count() => _count;
}