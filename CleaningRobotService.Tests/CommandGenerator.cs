using CleaningRobotService.Common.Dtos.Input;
using CleaningRobotService.Common.Enums;

namespace CleaningRobotService.Tests;

public static class CommandGenerator
{
    public static List<CommandDto> LoopCommands(uint steps)
    {
        List<CommandDto> commands = new();

        int directionInt = 0;
        for (int i = 0; i < 10000; i++)
        {
            DirectionEnum direction = (DirectionEnum)directionInt;
            directionInt++;

            if (directionInt == 4)
                directionInt = 0;

            commands.Add(new CommandDto
            {
                Direction = direction,
                Steps = steps,
            });
        }

        return commands;
    }
    
    public static List<CommandDto> LoopOffset()
    {
        List<CommandDto> commands = new();

        int directionInt = 0;
        for (int i = 0; i < 10000; i++)
        {
            DirectionEnum direction = (DirectionEnum)directionInt;
            directionInt++;

            if (directionInt == 4)
                directionInt = 0;

            commands.Add(new CommandDto
            {
                Direction = direction,
                // Step one less south/west so the robot goes in circles but slightly up right each command loop.
                Steps = (uint)(direction is DirectionEnum.South or DirectionEnum.West
                    ? 2
                    : 3
                ),
            });
        }

        return commands;
    }
    
    public static List<CommandDto> SpiralIn(uint width)
    {
        List<CommandDto> commands = new();

        int directionInt = 0;
        int directionOddCount = 0;
        for (int i = 0; i < width * 2; i++)
        {
            if (directionInt is 1 or 3)
                directionOddCount++;
            
            DirectionEnum direction = (DirectionEnum)directionInt;
            directionInt++;

            if (directionInt == 4)
                directionInt = 0;

            commands.Add(new CommandDto
            {
                Direction = direction,
                Steps = i == 0
                    ? width - 1
                    : (uint)(width - directionOddCount),
            });
        }

        return commands;
    }
}