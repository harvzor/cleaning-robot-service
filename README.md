# Cleaning Robot Service

## About

> This service simulates a robot moving around a space.

Given this input to the web API:

```json
{
  "start": {
    "x": 10,
    "y": 22
  },
  "commands": [
    {
      "direction": "east",
      "steps": 2
    },
    {
      "direction": "north",
      "steps": 1
    }
  ]
}
```

The robot should move like this:

![Visual example path](.github/visual-example-path.png)

The robot covers 4 coordinates on its route.

API output is:

```json
{
  "id": 19,
  "timestamp": "2022-10-18T13:05:48.8732354+00:00",
  "commands": 2,
  "result": 4,
  "duration": 0.0000258
}
```

## Requirements

- DotNet 6.0
- Postgres 10.8 (for running the API)
- Docker Engine 17.09.0+ (if you want to build from the `docker-compose.yml`)

## Running

Either run natively if you have dotnet installed or use Docker:

### Console application

```
dotnet run --configuration Release --project CleaningRobotService.Console 
```

or:

```
docker compose run --rm console
```

### Web application

This will also run any dependencies such as Postgres if you haven't already separately ran that service. You can comment out the `depends_on` section in the compose file if need be.

```
dotnet run --configuration Release --project CleaningRobotService.Web 
```

or

```
docker compose up web
```

#### Web env vars

- `App__DatabaseConnectionString` - configure the Postgres connection string

### Run dependencies

These will be run automatically if you run any services that depend on them.

```
docker compose up -d postgres
```

### Testing

```
dotnet test
```

or

```
docker compose run --rm test
```

### Benchmarking

```
dotnet run --configuration Release --project CleaningRobotService.Benchmarks
```

or

```
docker compose run --rm benchmark
```

## Robot algorithms

Methods like `RobotGrid_CalculatePointsVisited_10` indicate that the grid size is 10. Different grid sizes will impact memory usage.

### Robot algorithms benchmarks

**GenerateCommands_SpiralIn**

> generates a perfect spiral which hits every point in a 500 width/height grid exactly once

```
|                               Method |      Mean |     Error |   StdDev | Ratio | RatioSD |   Allocated | Alloc Ratio |
|------------------------------------- |----------:|----------:|---------:|------:|--------:|------------:|------------:|
|   RobotPoints_CalculatePointsVisited |  47.82 ms |  2.970 ms | 1.965 ms |  1.00 |    0.00 | 12233.83 KB |       1.000 |
|    RobotLines_CalculatePointsVisited | 648.68 ms | 12.627 ms | 7.514 ms | 13.55 |    0.62 |    20.53 KB |       0.002 |
|  RobotGrid_CalculatePointsVisited_10 |  37.35 ms |  1.534 ms | 1.015 ms |  0.78 |    0.04 |  2398.87 KB |       0.196 |
|  RobotGrid_CalculatePointsVisited_30 |  30.08 ms |  0.702 ms | 0.464 ms |  0.63 |    0.03 |   847.29 KB |       0.069 |
| RobotGrid_CalculatePointsVisited_100 |  29.34 ms |  0.436 ms | 0.288 ms |  0.61 |    0.03 |   414.74 KB |       0.034 |
| RobotGrid_CalculatePointsVisited_500 |  28.32 ms |  0.225 ms | 0.118 ms |  0.59 |    0.03 |   277.86 KB |       0.023 |
```

The `RobotGrid_CalculatePointsVisited_500` uses very little memory because it has a Grid size of 500, so the entire Grid is used.

**GenerateCommands_LoopOffset**

> moves in a loop which is offset by 1 point (north 3, east 3, south 2, west 2) x 10,000

```
|                               Method |       Mean |      Error |     StdDev |  Ratio | RatioSD |  Allocated | Alloc Ratio |
|------------------------------------- |-----------:|-----------:|-----------:|-------:|--------:|-----------:|------------:|
|   RobotPoints_CalculatePointsVisited |   3.028 ms |  0.1850 ms |  0.1224 ms |   1.00 |    0.00 |  657.47 KB |        1.00 |
|    RobotLines_CalculatePointsVisited | 828.450 ms | 34.3731 ms | 22.7357 ms | 273.84 |    9.52 |  198.12 KB |        0.30 |
|  RobotGrid_CalculatePointsVisited_10 |   4.306 ms |  0.2630 ms |  0.1740 ms |   1.43 |    0.11 |  580.72 KB |        0.88 |
|  RobotGrid_CalculatePointsVisited_30 |   4.227 ms |  0.1495 ms |  0.0989 ms |   1.40 |    0.06 |  554.47 KB |        0.84 |
| RobotGrid_CalculatePointsVisited_100 |   4.779 ms |  0.2059 ms |  0.1362 ms |   1.58 |    0.07 |  864.62 KB |        1.32 |
| RobotGrid_CalculatePointsVisited_500 |   8.909 ms |  0.4659 ms |  0.3081 ms |   2.95 |    0.16 | 2809.36 KB |        4.27 |
```

### Different algorithm strategies

I tried many different algorithms for calculating where the robot has been:

#### RobotPoints

Simple algorithm which stores each Point the robot has been at and stores it in a `HashSet<Point>`.

Pros:

- fast and simple with small datasets

Cons:

- memory usage is too high
  - each point uses 16 bytes of memory (2 ints, 8 bytes each)
  - if every possible point was visited and stored: 200,000² possible points * 16 bytes in a point = 640GB
  - HashSet requires an extra 12 bytes for each item in the collection so the actual memory usage would be even higher

#### RobotLines

Rather than storing each Point, each path is stored as a Line which has a Start and End Point. Each Point the Robot then visits can be checked to see if it's on any other Line.

Pros:

- uses very little memory
  - each Line requires 2 Points which requires 32 bytes of memory
  - if there are 10,000 commands, there should be 10,000 Lines, which would use 0.32MB of memory (not including collection overhead)

Cons:

- very slow speed
  - each Point has to be compared against each Line

Improvements?

- maybe `List<Line>` is not the best data structure for comparisons

#### RobotDictionaryLines

Similar to `RobotLines` but the search time of finding if a Point is on a pre-existing Line is reduced by putting the Lines into a Dictionary and only searching through Lines that are on the same x-axis or y-axis as this Point.

Pros:

- uses very little memory (same as RobotLines)

Cons:

- reasonably fast speed
  - each Point only has to be checked against a subset of Lines

#### RobotGrid

Rather than storing where the Robot has been using a Point, create a 2D array called a `Grid` which stores booleans and uses the index of the columns/rows to know the X and Y coordinates.

This Grid has a width of 3:

```
   0  1  2
0 [ ][ ][ ]
1 [ ][ ][ ]
2 [ ][ ][ ]
```

If the Robot starts at [0,0], goes to [2,0], then [2,2], the data structure would look like this:

```
   0  1  2
0 [x][x][x]
1 [ ][ ][x]
2 [ ][ ][x]
```

However, objects in C# cannot have a size larger than ~2GB. To have a Grid which could store every Point on it, I would need at least 5GB of memory to store the booleans alone (200,000² bits).

Therefore, I can use multiple Grids, lined up in rows and columns. These Grids can be added as needed to avoid unnecessary memory usage.

If the Robot starts at [0,0], goes to [5,0], and my Grid size is 3, 2 Grids will be used:

```
   0  1  2      0  1  2
0 [x][x][x]  0 [x][x][ ]
1 [ ][ ][ ]  1 [ ][ ][ ]
2 [ ][ ][ ]  2 [ ][ ][ ]
```

Each Grid can then store its offset.

Pros:

- tends to use less memory with large datasets
- reasonably fast

Cons:

- data structure unable to store the order of the points visited
- a good Grid width needs to be used to reduce memory usage
  - in my tests, I found a width of 30 to fit well
- still wastes memory for storing unvisited points

## Migrations

Migrations are handled by EntityFramework.

### Creating new migrations

First install the tool (the config for local tools is stored in `.config/dotnet-tools.json`:

```
dotnet tool restore
```

To add a new migration:

```
dotnet ef migrations add MigrationName --project CleaningRobotService.DataPersistence --context ServiceDbContext
```

### Running migrations

Migrations are run by this service as it first starts up. This means when you release a code change which also requires a database change, these will be released at the same time.

If you have multiple instances of this service running at the same time, and they're released one by one, older versions of code may run into errors if the database is updated by another instance. When you're releasing, you need to be careful to either:

- ensure that migrations don't take too long to run (which would stop so many errors in production from occurring)
- or your database changes should not break existing code (this may require multiple releases of migrations)

## Design decisions

### Patterns

Patterns used:

- Repository pattern - repositories are exposed from the DataPersistence project so the rest of the application doesn't know which ORM is being used underneath (this is a facade)
- Service pattern - for business logic
- Dependency injection - easier developing and testing (if mocking is needed)

Patterns not used:

- Unit of Work - could consider using to allow for transactional updates to the db

### Using EntityFramework

This ORM isn't the fastest ([being about twice as slow as Dapper](https://github.com/DapperLib/Dapper#performance)) but has some nice features:

- handles migrations
- no need to write SQL (and I love LINQ to SQL), so if your models update, the generated SQL updates too
- code is fairly agnostic about which db is really being used

Downsides:

- special features in specific databases can sometimes be hard to use

### Tests use a real instance of Postgres

While running database queries against a real instance of Postgres will somewhat slow down the tests, this is the best way to simulate real conditions in a unit test.

You can use an in memory database, but it doesn't act the same way as a real database.

https://learn.microsoft.com/en-us/ef/core/testing/#involving-the-database-or-not

> The in-memory provider will not behave like your real database in many important ways. Some features cannot be tested with it at all (e.g. transactions, raw SQL..), while other features may behave differently than your production database (e.g. case-sensitivity in queries). While in-memory can work for simple, constrained query scenarios, it is highly limited and we discourage its use.

### Separating out DTOs (data transfer objects) from database models

Example: CommandRobotDto and Execution

Both of these classes are almost identical, but they're separate because the API does not need to reflect how the database is built. It's possible that the API may need to change in the future to be different to how the database is structured, so may as well create different classes now to allow this.

Mapping between the DTOs and db models is manually done. AutoMapper could be used instead, but I'm not a fan of how AutoMapper magically does this, and this can cause runtime errors rather than issues being caught at compile time.

## If I had more time I would...

### Make the API more RESTful

I would make the CommandController store the Commands directly in the database. I would then have another controller where the Executions could be retrieved from.

Benefits:

- Commands can be replayed and Executions could be fixed (currently if there's a bug, there's no way to know which Commands were actually run).
- Executions could be run asynchronously. In the real world, the robot would move quite slowly so the task of it moving around could take a long time.

### Publish events to a queue

Since this service would likely be deployed to system of microservices, it could be that other services need to be aware of Executions, so I would publish them to a queue to other services could asynchronously consume any events.

I'd also add a Uuid column to the `executions` table which would be published instead of the integer ID.

### Implement auth

Right now, anyone can command the robot.

### Make use of transactions in db tests to ensure db remains clean between different tests?

Currently, each test which interacts with the database actually makes changes. This means if some tests ran in parallel, they could run into each other.

### Fix preciseness issue between Postgres and .NET

https://stackoverflow.com/questions/51103606/storing-datetime-in-postgresql-without-loosing-precision

### Decorate Swagger API with docs from XML

https://learn.microsoft.com/en-us/aspnet/core/tutorials/getting-started-with-swashbuckle?view=aspnetcore-6.0&tabs=visual-studio#xml-comments

### Ensure env vars are required

Running the application without properly setting up env vars can cause issues. This can happen quite often when a developer forgets to add new env vars to the production environment after testing a feature on a staging env and releasing. The application should fail to start if env vars are incorrectly configured. Systems like Kubernetes can also be configured to only take down older instances of services once the new instance has correctly started. This can avoid bad releases.

### Use Filters to create a consistent and useful error message when exceptions occur

https://learn.microsoft.com/en-us/aspnet/core/mvc/controllers/filters?view=aspnetcore-6.0

### Add some CI to build images and deploy to a container library

Personally I like Drone CI but Travis CI is okay too.

### More TODOs

- create some kind of system for putting benchmarks in the readme
- add large dataset for testing to repo
- add enforced styleguide
- better logging
- set CreateAt/ModifiedAt before save
- implement deletion of models and handle it in repo
- use async/await for db stuff
- fixing crazy mappings between all types
