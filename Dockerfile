# Stage 1: Build
FROM mcr.microsoft.com/dotnet/sdk:10.0-preview AS build
WORKDIR /app

# Copy csproj files and restore dependencies
COPY src/Robot.Domain/*.csproj ./src/Robot.Domain/
COPY src/Robot.Application/*.csproj ./src/Robot.Application/
COPY src/Robot.Api/*.csproj ./src/Robot.Api/
RUN dotnet restore src/Robot.Api/Robot.Api.csproj

# Copy everything else and build
COPY . .
RUN dotnet publish src/Robot.Api/Robot.Api.csproj -c Release -o /out

# Run tests: if these fail, the build fails
RUN dotnet test tests/Robot.Application.Tests/Robot.Application.Tests.csproj

# Stage 2: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:10.0-preview AS runtime
WORKDIR /app
COPY --from=build /out .

# Expose the port the app runs on
EXPOSE 8080
ENTRYPOINT ["dotnet", "Robot.Api.dll"]