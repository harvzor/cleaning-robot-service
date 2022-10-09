# Cleaning Robot Service

## Assumptions

- `/tibber-developer-test/enter-path` surely means to replace `/enter-path` with an appropriate resource name? I decided the resource would be called "CommandRobot". Though this API doesn't look RESTful at all.

## Env vars

- App__DatabaseConnectionString

## Docker

### Running

```
docker compose up runtime 
```

### Testing

```
docker compose up test 
```

## Migrations

Migrations are handled by EntityFramework.

### Creating new migrations

First install the tool (the config for local tools is stored in `.config/dotnet-tools.json`:

```
dotnet tool restore
```

To add a new migration:

```
dotnet ef migrations add MigrationName --project CleaningRobotService.Web --context ServiceDbContext
```

### Running migrations

Migrations are run by this service as it first starts up. This means when you release a code change which also requires a database change, these will be released at the same time.

If you have multiple instances of this service running at the same time, and they're released one by one, older versions of code may run into errors if the database is updated by another instance. When you're releasing, you need to be careful to either:

- ensure that migrations don't take too long to run (which would stop so many errors in production from occurring)
- or your database changes should not break existing code (this may require multiple releases of migrations)

## Todo

- ~~make sure API is camelcase~~
- ~~make sure db is stored camel case~~
- ~~make sure enums are displayed in API as strings~~
  - technically done but my solution isn't global for all enums, it can easily be done by using Newtonsoft
    - https://github.com/domaindrivendev/Swashbuckle.AspNetCore/issues/2293
    - https://stackoverflow.com/questions/72034017/net-6-addjsonoptions-with-camelcase-not-working
- ~~put timing code in a different method~~
- ~~make direction code more dry (with lambda?)~~
- ~~double check that Point comparison really works like a value type~~
- ~~make sure all paths begin with tibber-developer-test~~
- improve docs
- ~~test db~~
- ~~create dockerfile for building service~~
- test performance with larger dataset
- change CommandRobotService to only deal with the database and create an object which is the actual robot

## Design decisions

### Controllers and Services

Services should contain the actual business logic. Controllers are the interaction layer. This allows us to create different interaction layers such as a CLI which makes calls to the Services.

### Using EntityFramework

It's not the fastest but comes with all the features I need.

### Separating out DTOs (data transfer objects) from database models

Example: CommandRobotDto and Execution

Both of these classes are almost identical, but they're separate because the API does not need to reflect how the database is built. It's possible that the API may need to change in the future to be different to how the database is structured, so may as well create different classes now to allow this.

Mapping between the DTOs and db models is manually done. AutoMapper could be used instead, but I'm not a fan of how AutoMapper magically does this, and this can cause runtime errors rather than issues being caught at compile time.

## If I had more time I would...

### Publish events to a queue

### Make the API more RESTful

CommandController - this takes in commands and stores them in the db, it returns a CommandDto, and only accepts GET and POST

### Fix global enum in API support

Internally enums should be used to reduce coding/spelling mistakes. Externally, we want strings to be sent to the API.

### Implement auth

Right now, anyone can command the robot.

### Run commands in the background

Commands would need to be stored.

## Make use of transactions in db tests to ensure db remains clean between different tests?

## Fix preciseness issue between Postgres and .NET

https://stackoverflow.com/questions/51103606/storing-datetime-in-postgresql-without-loosing-precision

## Decorate Swagger API with docs from XML

https://learn.microsoft.com/en-us/aspnet/core/tutorials/getting-started-with-swashbuckle?view=aspnetcore-6.0&tabs=visual-studio#xml-comments

## Ensure env vars are required

Running the application without properly setting up env vars can cause issues. This can happen quite often when a developer forgets to add new env vars to the production environment after testing a feature on a staging env and releasing. The application should fail to start if env vars are incorrectly configured. Systems like Kubernetes can also be configured to only take down older instances of services once the new instance has correctly started. This can avoid bad releases.
