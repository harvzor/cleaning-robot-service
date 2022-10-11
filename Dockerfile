# BUILD IMAGE
FROM mcr.microsoft.com/dotnet/sdk:6.0 as build
WORKDIR /build

# Currently no way to use a glob pattern to get all csproj files:
# https://stackoverflow.com/questions/51372791/is-there-a-more-elegant-way-to-copy-specific-files-using-docker-copy-to-the-work
COPY ./CleaningRobotService.Web/CleaningRobotService.Web.csproj ./CleaningRobotService.Web/
COPY ./CleaningRobotService.Web.Benchmarks/CleaningRobotService.Web.Benchmarks.csproj ./CleaningRobotService.Web.Benchmarks/
COPY ./CleaningRobotService.Web.Tests/CleaningRobotService.Web.Tests.csproj ./CleaningRobotService.Web.Tests/
COPY ./CleaningRobotService.sln ./
#COPY ./NuGet.Config ./

RUN dotnet restore

COPY . .
RUN dotnet build
RUN dotnet publish ./CleaningRobotService.Web/CleaningRobotService.Web.csproj -c Release -o Release

# RUNTIME IMAGE
FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS runtime
WORKDIR /app
COPY --from=build /build/Release ./
ENTRYPOINT ["dotnet", "CleaningRobotService.Web.dll"]
