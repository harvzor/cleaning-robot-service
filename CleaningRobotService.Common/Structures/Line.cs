using System.Drawing;
using CleaningRobotService.Common.Enums;

namespace CleaningRobotService.Common.Structures;

public struct Line
{
    public Point Start;
    public Point End;
        
    private int GetLength()
    {
        return (int)Math.Sqrt(Math.Pow((End.X - Start.X), 2) + Math.Pow((End.Y - Start.Y), 2));
    }
        
    public List<Point> CalculatePoints()
    {
        if (Start == End)
        {
            return new List<Point>(1)
            {
                Start,
            };
        }

        bool isVertical = Start.X == End.X;
        bool ascending = isVertical
            ? Start.Y < End.Y
            : Start.X < End.X;

            Point currentPoint = Start;

        List<Point> points = new(GetLength() + 1) { currentPoint, };

        while (currentPoint != End)
        {
            if (isVertical)
                currentPoint.Y += ascending ? 1 : -1;
            else
                currentPoint.X += ascending ? 1 : -1;

            points.Add(currentPoint);
        }

        return points;
    }
}
