module BloggingModel

open System.ComponentModel.DataAnnotations
open Microsoft.EntityFrameworkCore

[<CLIMutable>]
type Blog = {
    [<Key>] Id: int
    Url: string
}

type BloggingContext() =  
    inherit DbContext()
        
    [<DefaultValue>] val mutable blogs : DbSet<Blog>
    member __.Blogs with get() = __.blogs and set v = __.blogs <- v

    override __.OnConfiguring(options: DbContextOptionsBuilder) : unit =
        options.UseSqlite("Data Source=blogging.db") |> ignore
