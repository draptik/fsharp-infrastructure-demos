namespace EFWebApi.Person

open Microsoft.EntityFrameworkCore

[<CLIMutable>]
type Person =
    { PersonId : int
      FirstName : string
      LastName : string
      Address : string
      City : string }

type PersonDataContext public(options) =
    inherit DbContext(options)

    [<DefaultValue>]
    val mutable persons : DbSet<Person>

    member public this.Persons with get() = this.persons
                               and set p = this.persons <- p