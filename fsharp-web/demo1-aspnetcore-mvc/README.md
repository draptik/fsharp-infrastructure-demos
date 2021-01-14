# ASP.NET Core with F#

This demo is a 1:1 port of a default C# web application project with "Basic Authentication". This template option
does not exist by default for F# projects, because EF Core doesn't support migrations.

Using "Basic Authentication" means using Microsoft's Identity plattform. For people unfamiliar with
Identity:

> The Microsoft identity platform helps you build applications your users and customers can sign in
> to using their Microsoft identities or social accounts, and provide authorized access to your own
> APIs or Microsoft APIs like Microsoft Graph.

([source](https://docs.microsoft.com/en-us/azure/active-directory/develop/v2-overview))

Yes, it's also OAuth2 compliant.

## Using EF Core with Identity

EF Core doesn't support migrations. So, to get Identity up and running, we have to use a workaround.

- create a C# web project with "Basic Authentication"
- export the C# migration into an SQL script file
- apply the SQL script to the F# project's sqlite db

PoC works...

## Using Dapper with Identity

TODO

- This looks promising: https://github.com/simonfaltum/AspNetCore.Identity.Dapper

