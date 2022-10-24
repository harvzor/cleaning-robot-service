using System.Drawing;
using CleaningRobotService.Common.Enums;
using CleaningRobotService.Common.Structures;

namespace CleaningRobotService.Common.Collections;

public class LineDictionary : IPointsCollections
{
    private int _count = 0;
    
    /// <summary>
    /// Second Key is the x or y coordinate. Values are the index of the line in the <see cref="_lines"/>.
    /// </summary>
    /// <remarks>
    /// A <see cref="Lookup"/> might make more sense here but it isn't mutable.
    /// </remarks>
    private readonly Dictionary<(PlaneEnum, int), List<Line>> _dictionary;
    private readonly List<Line> _lines;

    public LineDictionary(int numberOfExpectedCommands = 0)
    {
        // Capacity here isn't likely the be the same as the number of commands if the commands cause the robot to
        // travel the same lines.
        _dictionary = new Dictionary<(PlaneEnum, int), List<Line>>(numberOfExpectedCommands);
        _lines = new List<Line>(numberOfExpectedCommands);
    }

    public void AddLine(Line line)
    {
        PlaneEnum planeEnum = line.GetPlane();
        int key = planeEnum == PlaneEnum.Horizontal
            ? line.Start.Y
            : line.Start.X;
        
        _lines.Add(line);

        if (_dictionary.TryGetValue((planeEnum, key), out List<Line>? lineIndexes))
        {
            lineIndexes.Add(line);
        }
        else
        {
            _dictionary
                .Add((planeEnum, key), new List<Line>
                {
                    line,
                });
        }
    }
    
    public void AddPoint(Point point)
    {
        // Check to see if this Point overlaps any other pre-existing line without calculating each point of every line.
        // Overlaps are calculates by checking where the Start and End of a line is, and seeing if this Point is between
        // (or on) them.
        
        // Find all horizontal lines that are on the same y-axis at this Point.
        if (_dictionary.TryGetValue((PlaneEnum.Horizontal, point.Y), out List<Line>? matchingHorizontalLines))
        {
            foreach (Line line in matchingHorizontalLines)
            {
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
        if (_dictionary.TryGetValue((PlaneEnum.Vertical, point.X), out List<Line>? matchingVerticalLines))
        {
            foreach (Line line in matchingVerticalLines)
            {
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