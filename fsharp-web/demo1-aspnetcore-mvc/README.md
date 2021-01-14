# ASP.NET Core with F#

## Using EF Core with Identity

EF Core doesn't support migrations. So, to get Identity up and running, we have to use a workaround.

- create a C# web project with "Basic Authentication"
- export the C# migration into an SQL script file
- apply the SQL script to the F# project's sqlite db

PoC works...

## Using Dapper with Identity

TODO

- This looks promising: https://github.com/simonfaltum/AspNetCore.Identity.Dapper

