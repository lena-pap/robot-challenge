using Robot.Domain;

namespace Robot.Application;

public record RobotRequest(int GridWidth, int GridHeight, int X, int Y, string Direction, string Commands);
public record RobotResponse(int X, int Y, string Direction);

public class MotionService
{
    public RobotResponse ExecuteMotion(RobotRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Direction) || 
            char.IsDigit(request.Direction[0]) ||
            !Enum.TryParse<Direction>(request.Direction, out var initialDirection) ||
            !Enum.IsDefined(initialDirection))
        {
            throw new ArgumentException("Invalid starting direction. Use N, S, E, or W.");
        }

        var robotPosition = new Position(request.X, request.Y, initialDirection, request.GridWidth, request.GridHeight);

        foreach (char c in request.Commands)
        {
            if (!char.IsDigit(c) && Enum.TryParse<Command>(c.ToString(), out var command) && Enum.IsDefined(command))
            {
                robotPosition.Execute(command);
            }
            else
            {
                throw new ArgumentException($"Unknown command character: {c}. Use M, L or R.");
            }
        }
    
        return new RobotResponse(robotPosition.X, robotPosition.Y, robotPosition.Towards.ToString());
    }
}