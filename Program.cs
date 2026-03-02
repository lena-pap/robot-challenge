var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.MapPost("/move", (int gridWidth, int gridHeight, int x, int y, string direction, string commands) =>
{
    if (IsOffGrid(x, y, gridWidth, gridHeight))
    {
        return Results.BadRequest(new { error = "Off grid!" });
    }

    Position initialPosition = new(x, y, direction);
    var (nextX, nextY) = (x, y); // North decreases the value of y, since (0,0) is at the top-left corner
    string nextDirection = direction;

    try
    {
        foreach(char motion in commands)
        {
            if(motion == 'M')
            {
                (nextX, nextY) = nextDirection switch
                {
                    "N" => (nextX, nextY - 1),
                    "S" => (nextX, nextY + 1),
                    "E" => (nextX + 1, nextY),
                    "W" => (nextX - 1, nextY),
                    _ => throw new BadRequestException("Uknown direction!")
                };

                if (IsOffGrid(nextX, nextY, gridWidth, gridHeight))
                {
                    return Results.BadRequest(new { error = "Off grid!" });
                }
            }
            else if(motion == 'R' || motion == 'L')
            {
                nextDirection = (nextDirection, motion) switch
                {
                    ("N", 'R') => "E", ("E", 'R') => "S", ("S", 'R') => "W", ("W", 'R') => "N",
                    ("N", 'L') => "W", ("W", 'L') => "S", ("S", 'L') => "E", ("E", 'L') => "N",
                    _ => throw new BadRequestException("Uknown command!")
                };
            }
            else
            {
                return Results.BadRequest(new { error = "Unkown command!" });
            }
        }

        return Results.Ok(new Position(nextX, nextY, nextDirection));
    }
    catch (BadRequestException ex)
    {
        return Results.BadRequest(new { error = ex.Message });
    }
})
.WithName("Move");

// Static helper method to validate the boundary logic
static bool IsOffGrid(int x, int y, int width, int height)
{
    if (x < 0 || x >= width || y < 0 || y >= height)
    {
        return true;
    }
    return false;
}

app.Run();

record Position(int X, int Y, string Direction);

class BadRequestException(string message) : Exception(message);