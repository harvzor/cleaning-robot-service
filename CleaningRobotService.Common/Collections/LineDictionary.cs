using System.Drawing;
using CleaningRobotService.Common.Enums;
using CleaningRobotService.Common.Structures;

namespace CleaningRobotService.Common.Collections;

public class LineDictionary : IPointsCollections
{
    private int _count;
    
    /// <summary>
    /// Second Key is the <see cref="Line.Start"/> X or Y coordinate. Values are the index of the line in the <see cref="_lines"/>.
    /// </summary>
    /// <remarks>
    /// A Lookup might make more sense here but it isn't mutable.
    /// </remarks>
    private readonly Dictionary<(PlaneEnum, int), List<int>> _dictionary;
    private readonly List<Line> _lines;

    public LineDictionary(int numberOfExpectedCommands = 0)
    {
        // Capacity here isn't likely the be the same as the number of commands if the commands cause the robot to
        // travel the same lines.
        // Actually it would make more sense if this was a lookup as that can contain multiple values for the same key.
        _dictionary = new Dictionary<(PlaneEnum, int), List<int>>(numberOfExpectedCommands);
        _lines = new List<Line>(numberOfExpectedCommands);
    }

    public void AddLine(Line line)
    {
        PlaneEnum planeEnum = line.GetPlane();
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
        // Check to see if this Point overlaps any other pre-existing line without calculating each point of every line.
        // Overlaps are calculates by checking where the Start and End of a line is, and seeing if this Point is between
        // (or on) them.
        
        // Find all horizontal lines that are on the same y-axis at this Point.
        if (_dictionary.TryGetValue((PlaneEnum.Horizontal, point.Y), out List<int>? matchingHorizontalLineIndexes))
        {
            foreach (int index in matchingHorizontalLineIndexes)
            {
                Line line = _lines[index];
                
                // Since this Point is on the same y-axis, only check if the x value of this Point is on or between the
                // Line Start and End x points.
                if (line.Start.X < line.End.X
                        ? point.X >= line.Start.X && point.X <= line.End.X
                        : point.X <= line.Start.X && point.X >= line.End.X)
                {
                    return;
                }
            }
        }
            
        // Find all vertical lines that are on the same x-axis at this Point.
        if (_dictionary.TryGetValue((PlaneEnum.Vertical, point.X), out List<int>? matchingVerticalLineIndexes))
        {
            foreach (int index in matchingVerticalLineIndexes)
            {
                Line line = _lines[index];
                
                // Since this Point is on the same x-axis, only check if the y value of this Point is on or between the
                // Line Start and End y points.
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