# ASP.NET Core with F#

This demo is a 1:1 port of a default C# web application project with "Basic Authentication". This template option
does not exist by default for F# projects, because EF Core doesn't support migrations in F#.

Using "Basic Authentication" means using Microsoft's Identity plattform. For people unfamiliar with
Identity:

> The Microsoft identity platform helps you build applications your users and customers can sign in
> to using their Microsoft identities or social accounts, and provide authorized access to your own
> APIs or Microsoft APIs like Microsoft Graph.

([source](https://docs.microsoft.com/en-us/azure/active-directory/develop/v2-overview))

Yes, it's also OAuth2 compliant.

## Using EF Core with Identity

EF Core doesn't support migrations in F#. So, to get Identity up and running, we have to use a workaround.

- create a C# web project with "Basic Authentication"
    - `dotnet new mvc --auth Individual`
- export the C# migration into an SQL script file
    - `dotnet ef script migration.sql` (and remove the first 2 lines...)
- apply the SQL script to the F# project's sqlite db
    - `cp migration.sql <fsharp-folder-containing-the-Program.fs-file>`
    - `./apply-migration.sh` (Content: `cat migration.sql | sqlite3 app.db`)

This creates all database tables required by Identity.

Start F# project. It should behave the same way the C# projects behaves.

PoC works...

## Using Dapper with Identity

TODO

- This looks promising: https://github.com/simonfaltum/AspNetCore.Identity.Dapper

