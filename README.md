# Robot Motion API
A nice and straightforward API for commanding a robot to move around! 🤖

The API takes grid dimensions and initial position as arguments, along with a set of commands as to where to move to.
Then it executes given commands and returns the final position.

## Assumptions
 - the grid's starting point is at the top-left, resembling a matrix
 - commanding the robot to move off grid is wrong*

## Thought process
Started with the simple implementation of the algorithm as you can see in `Program.cs` file.
Then structured the program to a company-grade API with `src` and `tests` folders, creating more separation of concerns and object-oriented implementation.

## Errors
..are thrown in case:
 - initial position is off grid
 - any subsequent position is off grid*
 - commands are not in the range M, R, L
 - direction is not in the range N, E, S, W

## Stack
Keep in mind that this project is in:
```
> dotnet --version
> 10.0.103
```
## Instructions
In order to run the API:
```
dotnet run --project src/Robot.Api
```

Feel free to try:
```
> curl -X POST http://localhost:xxxx/api/robot/move -H "Content-Type: application/json" -d '{
  "gridWidth": 8,
  "gridHeight": 8,
  "x": 2,
  "y": 2,
  "direction": "E",
  "commands": "MRMRR"
}'
> {"x":3,"y":3,"direction":"N"}
```
where xxxx is the port assigned when the server starts.

Run the tests by:
```
> Robot/tests/Robot.Application.Tests$ dotnet test
```
## Docker
The project is also dockerized for your convenience, so you can just run:
```
> docker build -t robot-api .
> docker run -d -p 5000:8080 --name robot-sim robot-api
```
When the build takes place, the tests also run :)
