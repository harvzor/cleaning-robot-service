namespace CleaningRobotService.Web.Structs;

/// <summary>
/// A single location on a 2D plane the robot could be at.
/// </summary>
public struct Point
{
    public int X { get; set; }
    public int Y { get; set; }
    
    public Point(int x, int y)
    {
        X = x;
        Y = y;
    }

    public Point Combine(Point otherPoint)
    {
        return new Point()
        {
            X = X + otherPoint.X,
            Y = Y + otherPoint.Y,
        };
    }
    
    public static int GetDistance(Point p1, Point p2)
    {
        return (int)Math.Sqrt(Math.Pow((p2.X - p1.X), 2) + Math.Pow((p2.Y - p1.Y), 2));
    }
    
    /// <summary>Taken from https://stackoverflow.com/a/25689069/</summary>
    /// <remarks>Could probably simplify to not need <param name="epsilon"></param> given that this is always used on a straight line.</remarks>
    public static bool PointOnLine(Point pt1, Point pt2, Point pt, double epsilon = 0.001)
    {
        if (pt.X - Math.Max(pt1.X, pt2.X) > epsilon || 
            Math.Min(pt1.X, pt2.X) - pt.X > epsilon || 
            pt.Y - Math.Max(pt1.Y, pt2.Y) > epsilon || 
            Math.Min(pt1.Y, pt2.Y) - pt.Y > epsilon)
            return false;

        if (Math.Abs(pt2.X - pt1.X) < epsilon)
            return Math.Abs(pt1.X - pt.X) < epsilon || Math.Abs(pt2.X - pt.X) < epsilon;
        if (Math.Abs(pt2.Y - pt1.Y) < epsilon)
            return Math.Abs(pt1.Y - pt.Y) < epsilon || Math.Abs(pt2.Y - pt.Y) < epsilon;

        double x = pt1.X + (pt.Y - pt1.Y) * (pt2.X - pt1.X) / (pt2.Y - pt1.Y);
        double y = pt1.Y + (pt.X - pt1.X) * (pt2.Y - pt1.Y) / (pt2.X - pt1.X);

        return Math.Abs(pt.X - x) < epsilon || Math.Abs(pt.Y - y) < epsilon;
    }

    public static bool IsMoreNorth(Point p1, Point p2) => p1.Y > p2.Y;
    public static bool IsMoreEast(Point p1, Point p2) => p1.X > p2.X;
    public static bool IsMoreSouth(Point p1, Point p2) => p1.Y < p2.Y;
    public static bool IsMoreWest(Point p1, Point p2) => p1.X < p2.X;
    
    public static bool operator ==(Point p1, Point p2) => p1.Equals(p2);
    
    public static bool operator !=(Point p1, Point p2) => !p1.Equals(p2);
}
