namespace WebApplication

open Microsoft.AspNetCore.Builder
open Microsoft.AspNetCore.Hosting
open Microsoft.Extensions.Configuration
open Microsoft.Extensions.DependencyInjection
open Microsoft.Extensions.Hosting

// https://github.com/simonfaltum/AspNetCore.Identity.Dapper
// nuget package names:
// - VeryGood.AspNetCore.Identity
// - VeryGood.AspNetCore.Identity.DatabaseScripts.DbUp
open AspNetCore.Identity.Dapper
open AspNetCore.Identity.Dapper.Models
open AspNetCore.Identity.DatabaseScripts.DbUp

type Startup(configuration: IConfiguration) =
    member _.Configuration = configuration

    // This method gets called by the runtime. Use this method to add services to the container.
    member _.ConfigureServices(services: IServiceCollection) =
        // Add framework services.
        
        let myDbFilename = "app.sqlite"
        let myConnectionString = sprintf "Data Source=%s;Version=3;" myDbFilename
        let myDbSchema = "foo"
        
        services
            .AddIdentityCore<ApplicationUser>(fun options ->
                options.User.AllowedUserNameCharacters <- "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+"
                options.User.RequireUniqueEmail <- true
                options.Password.RequireDigit <- false
                )
            .AddRoles<ApplicationRole>()
            .AddDapperStores(fun options ->
                options.ConnectionString <- myConnectionString
                options.DbSchema <- myDbSchema) |> ignore
        
        services
            .AddIdentityDbUpDatabaseScripts(fun options ->
                options.ConnectionString <- myConnectionString
                options.DbSchema <- myDbSchema) |> ignore
        
        services.AddControllers() |> ignore

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    member _.Configure(app: IApplicationBuilder, env: IWebHostEnvironment) =
        if (env.IsDevelopment()) then
            app.UseDeveloperExceptionPage() |> ignore
        app.UseHttpsRedirection()
           .UseRouting()
           .UseAuthorization()
           .UseEndpoints(fun endpoints ->
                endpoints.MapControllers() |> ignore
            ) |> ignore
