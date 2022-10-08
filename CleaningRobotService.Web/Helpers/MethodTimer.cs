using System.Diagnostics;

namespace CleaningRobotService.Web.Helpers;

public class MethodTimer
{
    /// <summary>
    /// Measure how long the <param name="action"></param> takes to complete (in seconds).
    /// </summary>
    /// <param name="action"></param>
    /// <returns></returns>
    public static float Measure(Action action)
    {
        Stopwatch stopWatch = new();
        stopWatch.Start();
        
        action();
        
        return stopWatch.ElapsedMilliseconds * 1000;
    }
}