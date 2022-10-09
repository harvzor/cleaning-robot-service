# BUILD IMAGE
FROM mcr.microsoft.com/dotnet/sdk:6.0 as build
WORKDIR /build

COPY ./CleaningRobotService.Web/CleaningRobotService.Web.csproj ./CleaningRobotService.Web/
COPY ./CleaningRobotService.Web.Tests/CleaningRobotService.Web.Tests.csproj ./CleaningRobotService.Web.Tests/
COPY ./CleaningRobotService.sln ./
#COPY ./NuGet.Config ./

RUN ls
RUN dotnet restore

COPY . .
RUN dotnet build
RUN dotnet publish ./CleaningRobotService.Web/CleaningRobotService.Web.csproj -c Release -o Release

# RUNTIME IMAGE
FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS runtime
WORKDIR /app
COPY --from=build /build/Release ./
ENTRYPOINT ["dotnet", "CleaningRobotService.Web.dll"]
