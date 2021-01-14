https://github.com/efcore/EFCore.FSharp/blob/master/GETTING_STARTED.md

Trying to creating migrations...

```sh
dotnet ef migrations add Initial
```

Fails with the following error:

```sh
Build started...
Build succeeded.
System.MissingMethodException: Method not found: 'System.String Microsoft.EntityFrameworkCore.Design.ICSharpHelper.Lambda(System.Collections.Generic.IReadOnlyList`1<System.String>)'.
   at EntityFrameworkCore.FSharp.EFCoreFSharpServices.Microsoft.EntityFrameworkCore.Design.IDesignTimeServices.ConfigureDesignTimeServices(IServiceCollection services)
   at DesignTimeServices.DesignTimeServices.Microsoft-EntityFrameworkCore-Design-IDesignTimeServices-ConfigureDesignTimeServices(IServiceCollection serviceCollection) in /home/patrick/projects/fsharp-ef/demo1/FsEfTest/DesignTimeServices.fs:line 12
   at Microsoft.EntityFrameworkCore.Design.Internal.DesignTimeServicesBuilder.ConfigureDesignTimeServices(Type designTimeServicesType, IServiceCollection services)
   at Microsoft.EntityFrameworkCore.Design.Internal.DesignTimeServicesBuilder.ConfigureUserServices(IServiceCollection services)
   at Microsoft.EntityFrameworkCore.Design.Internal.DesignTimeServicesBuilder.Build(DbContext context)
   at Microsoft.EntityFrameworkCore.Design.Internal.MigrationsOperations.AddMigration(String name, String outputDir, String contextType, String namespace)
   at Microsoft.EntityFrameworkCore.Design.OperationExecutor.AddMigrationImpl(String name, String outputDir, String contextType, String namespace)
   at Microsoft.EntityFrameworkCore.Design.OperationExecutor.AddMigration.<>c__DisplayClass0_0.<.ctor>b__0()
   at Microsoft.EntityFrameworkCore.Design.OperationExecutor.OperationBase.<>c__DisplayClass3_0`1.<Execute>b__0()
   at Microsoft.EntityFrameworkCore.Design.OperationExecutor.OperationBase.Execute(Action action)
Method not found: 'System.String Microsoft.EntityFrameworkCore.Design.ICSharpHelper.Lambda(System.Collections.Generic.IReadOnlyList`1<System.String>)'.
```

It doesn't matter which target framework is used, the error is always the same (tried 3.1.404 and
5.0.100).

