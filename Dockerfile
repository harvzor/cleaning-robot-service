# BUILD IMAGE
FROM mcr.microsoft.com/dotnet/sdk:6.0 as build
WORKDIR /build

# Currently no way to use a glob pattern to get all csproj files:
# https://stackoverflow.com/questions/51372791/is-there-a-more-elegant-way-to-copy-specific-files-using-docker-copy-to-the-work
COPY ./CleaningRobotService.Common/CleaningRobotService.Common.csproj ./CleaningRobotService.Common/
COPY ./CleaningRobotService.Console/CleaningRobotService.Console.csproj ./CleaningRobotService.Console/
COPY ./CleaningRobotService.Tests/CleaningRobotService.Tests.csproj ./CleaningRobotService.Tests/
COPY ./CleaningRobotService.Web/CleaningRobotService.Web.csproj ./CleaningRobotService.Web/
COPY ./CleaningRobotService.Benchmarks/CleaningRobotService.Benchmarks.csproj ./CleaningRobotService.Benchmarks/
COPY ./CleaningRobotService.sln ./
#COPY ./NuGet.Config ./

RUN dotnet restore

COPY . .
RUN dotnet build
RUN dotnet publish ./CleaningRobotService.Web/CleaningRobotService.Web.csproj -c Release -o Web
RUN dotnet publish ./CleaningRobotService.Console/CleaningRobotService.Console.csproj -c Release -o Console

# RUNTIME IMAGE
FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS web
WORKDIR /app
COPY --from=build /build/Web ./
ENTRYPOINT ["dotnet", "CleaningRobotService.Web.dll"]

# CONSOLE IMAGE
FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS console
WORKDIR /app
COPY --from=build /build/Console ./
ENTRYPOINT ["dotnet", "CleaningRobotService.Console.dll"]
