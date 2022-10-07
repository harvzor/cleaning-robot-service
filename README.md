# Cleaning Robot Service

## Assumptions

- `/tibber-developer-test/enter-path` surely means to replace `/enter-path` with an appropriate resource name? I decided the resource would be called "CommandRobot". Though this API doesn't look RESTful at all.

## Todo

- make sure API is camelcase
- make sure db is stored camel case
- make sure enums are displayed in API as strings
- put timing code in a different method
- make direction code more dry (with lambda?)
- double check that Point comparison really works like a value type

## Design decisions

### Controllers and Services

Services should contain the actual business logic. Controllers are the interaction layer. This allows us to create different interaction layers such as a CLI which makes calls to the Services.

### Using EntityFramework

It's not the fastest but comes with all the features I need.

### Separating out DTOs (data transfer objects) from database models

Example: CommandRobotDto and Execution

Both of these classes are almost identical, but they're separate because the API does not need to reflect how the database is built. It's possible that the API may need to change in the future to be different to how the database is structured, so may as well create different classes now to allow this.

Mapping between the DTOs and db models is manually done. AutoMapper could be used instead, but I'm not a fan of how AutoMapper magically does this, and this can cause runtime errors rather than issues being caught at compile time.

## Could do

### Publish events to a queue

### Make the API more RESTful

CommandController - this takes in commands and stores them in the db, it returns a CommandDto, and only accepts GET and POST