using System.Drawing;
using CleaningRobotService.Common.Dtos;

namespace CleaningRobotService.BusinessLogic.Mappers;

public static class PointMapper
{
    public static PointDto ToDto(this Point point) => new()
    {
        X = point.X,
        Y = point.Y,
    };
    
    public static Point ToModel(this PointDto pointDto) => new()
    {
        X = pointDto.X,
        Y = pointDto.Y,
    };
}
