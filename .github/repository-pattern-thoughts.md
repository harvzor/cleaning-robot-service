# Repository Pattern Thoughts

Pros:

- encourages code re-use
- facade could be used for any database/ORM being used underneath
- facade could use faster ORM in some places depending on the use
  - for example, a method in the facade could say it's returning a readonly object (in EF, change tracking off), the underlying tech could be changed to Dapper to speed it up

Cons:

- new developers coming to a project need to learn how to use your specific implementation
- basically just rewriting features of EF into my repository pattern
- makes it harder to write special code for edge cases such as high performance cases where change tracking should be turned off
- eager loading strange to setup
  - if the underlying tech was not EF, but something like ElasticSearch, eager loading isn't really a thing?
- are you really ever going to change the database or ORM being used underneath?
- you have to implement UoW pattern because EF is already a bunch of repositories with the UoW pattern (calling SaveChanges saves everything)

