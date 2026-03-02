using Robot.Application;
using Xunit;

namespace Robot.Application.Tests;

public class MotionServiceTests
{
    private readonly MotionService _service = new();

    [Fact]
    public void ExecuteMotion_WithValidInput_ReturnsCorrectResponse()
    {
        var request = new RobotRequest(8, 8, 2, 2, "E", "MRMRR");

        var response = _service.ExecuteMotion(request);

        Assert.Equal(3, response.X);
        Assert.Equal(3, response.Y);
        Assert.Equal("N", response.Direction);
    }

    [Theory]
    [InlineData("X")]
    [InlineData("North")]
    [InlineData("")]
    [InlineData("1")]
    public void ExecuteMotion_InvalidDirection_ThrowsArgumentException(string badDir)
    {
        var request = new RobotRequest(5, 5, 2, 2, badDir, "M");

        var ex = Assert.Throws<ArgumentException>(() => _service.ExecuteMotion(request));
        Assert.Contains("Invalid starting direction", ex.Message);
    }

    [Theory]
    [InlineData("Z")]
    [InlineData("M X R")]
    [InlineData("M1R")]
    public void ExecuteMotion_InvalidCommands_ThrowsArgumentException(string badCmds)
    {
        var request = new RobotRequest(5, 5, 2, 2, "N", badCmds);

        var ex = Assert.Throws<ArgumentException>(() => _service.ExecuteMotion(request));
        Assert.Contains("Unknown command character", ex.Message);
    }

    [Fact]
    public void ExecuteMotion_OffGridMovement_ThrowsInvalidOperationException()
    {
        // Arrange - Move North from (0,0) on a 5x5 grid
        var request = new RobotRequest(5, 5, 0, 0, "N", "M");

        Assert.Throws<InvalidOperationException>(() => _service.ExecuteMotion(request));
    }
}