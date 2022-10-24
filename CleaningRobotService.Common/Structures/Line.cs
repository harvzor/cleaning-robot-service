using System.Drawing;
using CleaningRobotService.Common.Enums;

namespace CleaningRobotService.Common.Structures;

public struct Line
{
    public Point Start;
    public Point End;

    public Line(Point start, Point end)
    {
        Start = start;
        End = end;
    }
        
    private int GetLength()
    {
        return (int)Math.Sqrt(Math.Pow((End.X - Start.X), 2) + Math.Pow((End.Y - Start.Y), 2));
    }

    public PlaneEnum GetPlane()
    {
        if (Start.X == End.X)
            return PlaneEnum.Vertical;

        return PlaneEnum.Horizontal;
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

        PlaneEnum plane = GetPlane();
        bool ascending = plane == PlaneEnum.Vertical
            ? Start.Y < End.Y
            : Start.X < End.X;

            Point currentPoint = Start;

        List<Point> points = new(GetLength() + 1) { currentPoint, };

        while (currentPoint != End)
        {
            if (plane == PlaneEnum.Vertical)
                currentPoint.Y += ascending ? 1 : -1;
            else
                currentPoint.X += ascending ? 1 : -1;

            points.Add(currentPoint);
        }

        return points;
    }
}
