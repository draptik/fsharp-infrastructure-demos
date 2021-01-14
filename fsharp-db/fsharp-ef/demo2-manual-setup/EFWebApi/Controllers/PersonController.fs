namespace EFWebApi.Controllers

open System.Linq
open Microsoft.AspNetCore.Mvc
open Microsoft.Extensions.Logging
open EFWebApi.Person

[<ApiController>]
[<Route("[controller]")>]
type PersonController (logger : ILogger<PersonController>, ctx : PersonDataContext) =
    inherit ControllerBase()

    [<HttpGet>]
    [<Route("{id}")>]
    member this.Get(id : int) =
        let person = ctx.Persons.FirstOrDefault(fun p -> p.PersonId = id)
        if (box person = null)
        then this.NotFound() :> IActionResult
        else this.Ok person :> IActionResult

    [<HttpGet>]
    member this.All() =
        let persons = ctx.Persons.Cast<Person>().ToList()
        persons
    
    [<HttpPost>]
    member this.Post(person : Person) : IActionResult =
        
        let createPerson(person : Person) : Person =
            ctx.Persons.Add(person) |> ignore
            ctx.SaveChanges() |> ignore
            ctx.Persons.First(fun p -> p = person)
            
        match person.PersonId with
        | 0 -> this.BadRequest("PersonId is required") :> IActionResult
        | _ ->
            match box(ctx.Persons.FirstOrDefault(fun p -> p.PersonId = person.PersonId)) with
            | null -> createPerson person |> this.Ok :> IActionResult
            | _ ->
                ctx.Persons.First(fun p -> p.PersonId = person.PersonId)
                |> this.Conflict :> IActionResult
                