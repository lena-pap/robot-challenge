namespace Robot.Domain;

public class Position
{
    public int X { get; private set; }
    public int Y { get; private set; }
    
    public Direction Towards { get; private set; }
    private readonly int _gridWidth;
    private readonly int _gridHeight;

    public Position(int x, int y, Direction towards, int width, int height)
    {
        _gridWidth = width;
        _gridHeight = height;

        if (IsOffGrid(x, y))
        {
            throw new ArgumentException("Initial position is off grid!");
        }

        X = x;
        Y = y;
        Towards = towards;
    }

    public void Execute(Command command)
    {
        if (command == Command.M) Move();
        else Rotate(command);
    }

    private void Move()
    {
        // North decreases the value of y, since (0,0) is at the top-left corner
        var (nextX, nextY) = Towards switch
        {
            Direction.N => (X, Y - 1),
            Direction.S => (X, Y + 1),
            Direction.E => (X + 1, Y),
            Direction.W => (X - 1, Y),
            _ => (X, Y)
        };

        if (IsOffGrid(nextX, nextY))
        {
            throw new InvalidOperationException("Off grid!");
        }

        X = nextX;
        Y = nextY;
    }

    private void Rotate(Command command)
    {
        Towards = (Towards, command) switch
        {
            (Direction.N, Command.R) => Direction.E, (Direction.N, Command.L) => Direction.W,
            (Direction.E, Command.R) => Direction.S, (Direction.E, Command.L) => Direction.N,
            (Direction.S, Command.R) => Direction.W, (Direction.S, Command.L) => Direction.E,
            (Direction.W, Command.R) => Direction.N, (Direction.W, Command.L) => Direction.S,
            _ => Towards
        };
    }

    private bool IsOffGrid(int nx, int ny) 
    {
        return nx < 0 || nx >= _gridWidth || ny < 0 || ny >= _gridHeight;
    }
}