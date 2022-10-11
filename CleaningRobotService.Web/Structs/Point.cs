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
    
    public static bool operator ==(Point p1, Point p2) => p1.Equals(p2);
    
    public static bool operator !=(Point p1, Point p2) => !p1.Equals(p2);
}
