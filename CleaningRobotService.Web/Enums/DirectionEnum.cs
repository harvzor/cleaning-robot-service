namespace CleaningRobotService.Web.Enums;

/// <summary>
/// Cardinal directions the robot could take.
/// </summary>
public enum DirectionEnum
{
    // TODO: figure out how to have casing as per .NET standard but in the API have it lowercase.
    // https://stackoverflow.com/a/59912419/
    north = 0,
    east = 1,
    south = 2,
    west = 3,
}
