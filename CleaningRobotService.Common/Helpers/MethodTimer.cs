using System.Diagnostics;

namespace CleaningRobotService.Common.Helpers;

public class MethodTimer
{
    /// <summary>
    /// Measure how long the <param name="action"></param> takes to complete (in seconds).
    /// </summary>
    /// <param name="action"></param>
    /// <returns></returns>
    public static double Measure(Action action)
    {
        Stopwatch stopWatch = new();
        stopWatch.Start();
        
        action();

        stopWatch.Stop();
        
        return stopWatch.Elapsed.TotalSeconds;
    }
}