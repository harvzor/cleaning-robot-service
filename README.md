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

## Could do

### Publish events to a queue