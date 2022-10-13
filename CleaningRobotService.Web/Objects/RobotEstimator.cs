using CleaningRobotService.Web.Dtos.Input;
using CleaningRobotService.Web.Enums;
using CleaningRobotService.Web.Interfaces;
using CleaningRobotService.Web.Structs;

namespace CleaningRobotService.Web.Objects;

/// <summary>
/// Simulated robot which cleans the office.
/// </summary>
public class RobotEstimator
{
    public Point StartPoint { get; set; }
    public IEnumerable<Command> Commands { get; set; } = Enumerable.Empty<Command>();
    private Point _currentPoint;
    private readonly List<Line> _lines = new();
    
    private class Line
    {
        public Point StartPoint;
        public Point EndPoint;
        public DirectionEnum Direction;

        public IEnumerable<Point> CalculatePointsYielded()
        {
            Point currentPoint = StartPoint;

            yield return currentPoint;
            
            while (currentPoint != EndPoint)
            {
                switch (Direction)
                {
                    case DirectionEnum.north:
                        currentPoint.Y += 1;
                        break;
                    case DirectionEnum.east:
                        currentPoint.X += 1;
                        break;
                    case DirectionEnum.south:
                        currentPoint.Y -= 1;
                        break;
                    case DirectionEnum.west:
                        currentPoint.X -= 1;
                        break;
                    default:
                        throw new Exception("Command direction of {command.Direction} not covered.");
                }

                yield return currentPoint;
            }
        }
        
        public List<Point> CalculatePoints()
        {
            Point currentPoint = StartPoint;

            List<Point> points = new(Point.GetDistance(StartPoint, EndPoint) + 1) { currentPoint, };

            while (currentPoint != EndPoint)
            {
                switch (Direction)
                {
                    case DirectionEnum.north:
                        currentPoint.Y += 1;
                        break;
                    case DirectionEnum.east:
                        currentPoint.X += 1;
                        break;
                    case DirectionEnum.south:
                        currentPoint.Y -= 1;
                        break;
                    case DirectionEnum.west:
                        currentPoint.X -= 1;
                        break;
                    default:
                        throw new Exception("Command direction of {command.Direction} not covered.");
                }

                points.Add(currentPoint);
            }

            return points;
        }

        public bool IsPointOnLine(Point point)
        {
            return Point.PointOnLine(StartPoint, EndPoint, point);
        }
    }

    private void CalculateLines()
    {
        _currentPoint = StartPoint;

        foreach (var command in Commands.Where(command => command.Steps != 0))
        {
            Point start = _currentPoint;
            
            switch (command.Direction)
            {
                case DirectionEnum.north:
                    _currentPoint.Y += (int)command.Steps;
                    break;
                case DirectionEnum.east:
                    _currentPoint.X += (int)command.Steps;
                    break;
                case DirectionEnum.south:
                    _currentPoint.Y -= (int)command.Steps;
                    break;
                case DirectionEnum.west:
                    _currentPoint.X -= (int)command.Steps;
                    break;
                default:
                    throw new ArgumentException(
                        message: $"Command direction of {command.Direction} not covered.",
                        paramName: nameof(Commands)
                    );
            }

            Point end = _currentPoint;

            bool shouldBeSwapped = command.Direction is DirectionEnum.east or DirectionEnum.west
                ? Point.IsMoreEast(start, end)
                : Point.IsMoreNorth(start, end);
            
            // Normalise new StartEnd so the EndPoint is more positive than the StartPoint.
            Line line = new()
            {
                StartPoint = shouldBeSwapped ? end : start,
                // I think the direction should only be north or east given that it's normalised.
                Direction = command.Direction is DirectionEnum.north or DirectionEnum.south
                    ? DirectionEnum.north
                    : DirectionEnum.east,
                EndPoint = shouldBeSwapped ? start : end,
            };

            bool replaced = false;
            
            // Check for any _startEnds where this new startEnd starts or ends between.
            // TODO: must be a way to make code in statement into method (DRY it up).
            if (command.Direction is DirectionEnum.east or DirectionEnum.west)
            {
                Line? matchingStartEndIfStartOrEndIsOnLine = _lines
                    .Where(x => x.Direction is DirectionEnum.east or DirectionEnum.west)
                    // Where the Y coordinate is the same.
                    .Where(x => x.StartPoint.Y == line.StartPoint.Y)
                    // And the new startEnd starts or ends along any other in _startEnds.
                    .Where(x
                        => Point.PointOnLine(pt1: x.StartPoint, pt2: x.EndPoint, pt: line.StartPoint)
                        || Point.PointOnLine(pt1: x.StartPoint, pt2: x.EndPoint, pt: line.EndPoint)
                    )
                    .FirstOrDefault();

                if (matchingStartEndIfStartOrEndIsOnLine != null)
                {
                    replaced = true;
                    // If the new StartPoint is more west than the old StartPoint, swap them.
                    if (Point.IsMoreWest(p1: line.StartPoint, p2: matchingStartEndIfStartOrEndIsOnLine.StartPoint))
                        matchingStartEndIfStartOrEndIsOnLine.StartPoint = line.StartPoint;
                    
                    // If the new EndPoint is more east than the old EndPoint, swap them.
                    if (Point.IsMoreEast(p1: line.EndPoint, p2: matchingStartEndIfStartOrEndIsOnLine.EndPoint))
                        matchingStartEndIfStartOrEndIsOnLine.EndPoint = line.EndPoint;
                }
            }
            else
            {
                Line? matchingStartEndIfStartOrEndIsOnLine = _lines
                    .Where(x => x.Direction is DirectionEnum.north or DirectionEnum.south)
                    // Where the Y coordinate is the same.
                    .Where(x => x.StartPoint.X == line.StartPoint.X)
                    // And the new startEnd starts or ends along any other in _startEnds.
                    .Where(x
                        => Point.PointOnLine(pt1: x.StartPoint, pt2: x.EndPoint, pt: line.StartPoint)
                        || Point.PointOnLine(pt1: x.StartPoint, pt2: x.EndPoint, pt: line.EndPoint)
                    )
                    .FirstOrDefault();

                if (matchingStartEndIfStartOrEndIsOnLine != null)
                {
                    replaced = true;
                    // If the new StartPoint is more north than the old StartPoint, swap them.
                    if (Point.IsMoreNorth(p1: matchingStartEndIfStartOrEndIsOnLine.StartPoint, p2: line.StartPoint))
                        matchingStartEndIfStartOrEndIsOnLine.StartPoint = line.StartPoint;
                    
                    // If the new EndPoint is more south than the old EndPoint, swap them.
                    if (Point.IsMoreSouth(p1: matchingStartEndIfStartOrEndIsOnLine.EndPoint, p2: line.EndPoint))
                        matchingStartEndIfStartOrEndIsOnLine.EndPoint = line.EndPoint;
                }
            }
            
            // If the new startEnd didn't overlap any other startEnds, then just add it.
            if (!replaced)
                _lines.Add(line);
        }
    }

    private void ShiftEastLines()
    {
        List<Line> northLines = _lines
            .Where(x => x.Direction is DirectionEnum.north or DirectionEnum.south)
            .ToList();
        
        List<Line> eastLines = _lines
            .Where(x => x.Direction is DirectionEnum.east or DirectionEnum.west)
            .ToList();

        foreach (Line eastLine in eastLines)
        {
            // If the start/end is moved, then we know it's on an unused point.
            bool startMoved = false;
            bool endMoved = false;
            
            foreach (Point point in eastLine.CalculatePointsYielded())
            {
                // Move the start point to an used 
                if (!northLines.Any(northLine => point == northLine.StartPoint || point == northLine.EndPoint))
                {
                    eastLine.StartPoint = point;
                    startMoved = true;
                }
                
                if (!northLines.Any(northLine => point == northLine.EndPoint || point == northLine.StartPoint))
                {
                    eastLine.EndPoint = point;
                    startMoved = true;
                }
            }

            if (!startMoved && !endMoved)
            {
                _lines.Remove(eastLine);
            }
            
            // while (northLines.Any(northLine
            //     => eastLine.StartPoint == northLine.StartPoint
            //     || eastLine.StartPoint == northLine.EndPoint)
            // )
            // {
            //     if (eastLine.StartPoint == eastLine.EndPoint)
            //     {
            //         _lines.Remove(eastLine);
            //         break;
            //     }
            //     
            //     eastLine.StartPoint.X += 1;
            // }
            //
            // while (northLines.Any(northLine
            //     => eastLine.EndPoint == northLine.EndPoint
            //     || eastLine.EndPoint == northLine.StartPoint)
            // )
            // {
            //     if (eastLine.StartPoint == eastLine.EndPoint)
            //     {
            //         _lines.Remove(eastLine);
            //         break;
            //     }
            //     
            //     eastLine.EndPoint.X -= 1;
            // }
        }
    }

    public int CalculateNumberOfPointsVisited()
    {
        HashSet<Point> points = new();
        
        CalculateLines();

        // And brute force the rest.
        // {
        //     foreach (Line line in _lines)
        //     {
        //         List<Point> newPoints = line.CalculatePoints();
        //
        //         points.UnionWith(newPoints);
        //     }
        //
        //     return points.Count;
        // }

        // Brute force but don't store every point.
        // Since the east lines might overlap the north lines, count the points on the north lines,
        // then find the none overlapping east points and count them too.
        // {
        //     List<Line> northLines = _lines
        //         .Where(x => x.Direction is DirectionEnum.north or DirectionEnum.south)
        //         .ToList();
        //
        //     List<Line> eastLines = _lines
        //         .Where(x => x.Direction is DirectionEnum.east or DirectionEnum.west)
        //         .ToList();
        //
        //     int northCounts = northLines
        //         .Sum(line
        //             => Point.GetDistance(line.StartPoint, line.EndPoint)
        //                // Add one because the distance between 2 points doesn't include the initial point.
        //                + 1
        //         );
        //
        //     int eastsCount = 0;
        //     if (northLines.Any())
        //     {
        //         foreach (Line eastLine in eastLines)
        //         {
        //             List<Point> newPoints = eastLine.CalculatePoints();
        //
        //             // Using a line intersect method to figure out where two lines cross may be faster.
        //             foreach (Point point in newPoints)
        //             {
        //                 if (!northLines.Any(northLine => northLine.IsPointOnLine(point)))
        //                 {
        //                     eastsCount++;
        //                 }
        //             }
        //         }
        //     }
        //     else
        //     {
        //         eastsCount = eastLines
        //             .Sum(line
        //                 => Point.GetDistance(line.StartPoint, line.EndPoint)
        //                    // Add one because the distance between 2 points doesn't include the initial point.
        //                    + 1
        //             );
        //     }
        //
        //     return northCounts + eastsCount;
        // }

        // Normal count.
        // After ensuring no east line starts where north line ends.
        {
            ShiftEastLines();
        
            int northCounts = _lines
                .Where(x => x.Direction is DirectionEnum.north or DirectionEnum.south)
                .Sum(line
                    => Point.GetDistance(line.StartPoint, line.EndPoint)
                       // Add one because the distance between 2 points doesn't include the initial point.
                       + 1
                );
            
            int eastsCount =_lines
                .Where(x => x.Direction is DirectionEnum.east or DirectionEnum.west)
                .Sum(line
                    => Point.GetDistance(line.StartPoint, line.EndPoint)
                       // Add one because the distance between 2 points doesn't include the initial point.
                       + 1
                );
        
            return northCounts + eastsCount;
        }

        // return _startEnds
        //     .Where(x => x.Direction is DirectionEnum.north or DirectionEnum.south)
        //     .Sum(startEnd
        //         => Point.GetDistance(startEnd.StartPoint, startEnd.EndPoint)
        //         // Add one because the distance between 2 points doesn't include the initial point.
        //         + 1
        // );

        // int distances = _startEnds
        //     .Sum(startEnd
        //         => Point.GetDistance(startEnd.StartPoint, startEnd.EndPoint)
        //         // Add one because the distance between 2 points doesn't include the initial point.
        //         // + (startEnd.Direction is DirectionEnum.west or DirectionEnum.east ? 1 : 0)
        //     );
        //
        // int numberOfMatchingStartPoints= _startEnds
        //     .GroupBy(x => x.StartPoint)
        //     .Where(group => group.Count() > 1)
        //     .Count();
        //
        // numberOfMatchingStartPoints = 0;
        //
        // return distances + numberOfMatchingStartPoints;
    }
}