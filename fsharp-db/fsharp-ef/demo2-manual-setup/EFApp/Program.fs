open System
open Microsoft.EntityFrameworkCore

(*
NOTE: We have to create the SQLite database file and tables manually.

1. Create database file in same folder as `Program.cs`: `sqlite3 app.db`
2. Within Sqlite3, add table: `create table Persons (PersonId int PRIMARY KEY, FirstName string, LastName string, Address string, City string) without rowid;`
3. Add `app.db` file to project: Rider: Add existing file
4. Make sure the property is set to `Always copy` (otherwise to db can't be found and you'll get an error "table not found [...]"

Alternative to step 4: use an absolut path instead of `optionsBuilder.UseSqlite("DataSource=app.db")`
 
*)

[<CLIMutable>]
type Person = {
    PersonId : int
    FirstName : string
    LastName : string
    Address : string
    City : string
}

type PersonDataContext() =
    inherit DbContext()
    
    [<DefaultValue>]
    val mutable persons : DbSet<Person>
    
    member public this.Persons with get() = this.persons
                               and set p = this.persons <- p
    
    override __.OnConfiguring(optionsBuilder : DbContextOptionsBuilder) =
        optionsBuilder.UseSqlite("DataSource=app.db")
        |> ignore

[<EntryPoint>]
let main argv =
    let ctx = new PersonDataContext()
    
    ctx.Persons.Add({
        PersonId = (Random()).Next(99999)
        FirstName = "Name"
        LastName = "Surname"
        Address = "Address"
        City = "City"
    }) |> ignore
    
    ctx.SaveChanges() |> ignore
    
    let getPersons(ctx : PersonDataContext) =
        async {
            return! ctx.Persons.ToArrayAsync()
                    |> Async.AwaitTask
        }
        
    let persons = getPersons ctx |> Async.RunSynchronously
    
    persons
    |> Seq.iter Console.WriteLine
    
    0