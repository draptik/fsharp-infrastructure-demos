open System
open System.Collections.Generic
open System.Data.SQLite
open Dapper

type TradeData = { 
    Symbol : string; 
    Timestamp : DateTime; 
    Price : float;
    TradeSize : float }

// Sample Data
let trades = [
    { Symbol = "BTC/USD"; Timestamp = DateTime(2017, 07, 28, 10, 44, 33); Price = 2751.20; TradeSize = 0.01000000 }
    { Symbol = "BTC/USD"; Timestamp = DateTime(2017, 07, 28, 10, 44, 21); Price = 2750.20; TradeSize = 0.01000000 }
    { Symbol = "BTC/USD"; Timestamp = DateTime(2017, 07, 28, 10, 44, 21); Price = 2750.01; TradeSize = 0.40000000 }
    { Symbol = "BTC/USD"; Timestamp = DateTime(2017, 07, 28, 10, 44, 21); Price = 2750.01; TradeSize = 0.55898959 }
    { Symbol = "BTC/USD"; Timestamp = DateTime(2017, 07, 28, 10, 44, 03); Price = 2750.00; TradeSize = 0.86260000 }
    { Symbol = "BTC/USD"; Timestamp = DateTime(2017, 07, 28, 10, 44, 03); Price = 2750.00; TradeSize = 0.03000000 }
    { Symbol = "BTC/USD"; Timestamp = DateTime(2017, 07, 28, 10, 43, 31); Price = 2750.01; TradeSize = 0.44120000 } 
    ]

let dbQuery<'T> (connection:SQLiteConnection) (sql:string) (parameters:IDictionary<string, obj> option) = 
    match parameters with
    | Some(p) -> connection.Query<'T>(sql, p)
    | None    -> connection.Query<'T>(sql)

let createAndPopulateDatabase =
    
    // Initialize connectionstring
    let databaseFilename = "sample.sqlite"
    let connectionStringFile = sprintf "Data Source=%s;Version=3;" databaseFilename  

    // Create database
    SQLiteConnection.CreateFile(databaseFilename)

    // Open connection
    let connection = new SQLiteConnection(connectionStringFile)
    connection.Open()

    // Create table structure
    let structureSql =
        "create table Trades (" +
        "Symbol varchar(20), " +
        "Timestamp datetime, " + 
        "Price float, " + 
        "TradeSize float)"

    let structureCommand = new SQLiteCommand(structureSql, connection)
    structureCommand.ExecuteNonQuery()
    
    let insertTradeSql =
        "insert into trades(symbol, timestamp, price, tradesize) " + 
        "values (@symbol, @timestamp, @price, @tradesize)"
        
    trades
    |> List.map (fun x -> connection.Execute(insertTradeSql, x))
    |> List.sum
    |> (fun recordsAdded -> printfn "Records added  : %d" recordsAdded)
    
    
    // Clear out records from previous insert, so I don't end up with duplicate records
    let deleteCount = connection.Execute("delete from trades")
    printfn "Records deleted: %A" deleteCount

    let dbExecute (connection:SQLiteConnection) (sql:string) (data:_) = 
        connection.Execute(sql, data)

    trades
    |> List.map (dbExecute connection insertTradeSql)
    |> List.sum
    |> (fun recordsAdded -> printfn "Records added  : %d" recordsAdded)
    
    
    let filteredSql = 
        "select symbol, timestamp, price, tradesize From trades " +
        "where symbol = @symbol and tradesize >= @mintradesize"

    let results1 = 
        connection.Query<TradeData>(
            filteredSql, 
            dict [ "symbol", box "BTC/USD"; "mintradesize", box 0.4 ])

    printfn "Query (1):"
    results1 
    |> Seq.iter (fun x -> 
        printfn "%-7s %-19s %.2f [%.8f]" x.Symbol (x.Timestamp.ToString("s")) x.Price x.TradeSize)
    
    let inline (=>) k v = k, box v

    /// Moved dbQuery outside of function!
    //        let dbQuery<'T> (connection:SQLiteConnection) (sql:string) (parameters:IDictionary<string, obj> option) = 
    //            match parameters with
    //            | Some(p) -> connection.Query<'T>(sql, p)
    //            | None    -> connection.Query<'T>(sql)

    let results2 = dbQuery<TradeData> connection filteredSql (Some (dict [ "symbol" => "BTC/USD"; 
                                                                           "mintradesize" => 0.4 ]))

    printfn "Query (2):"
    results2 
    |> Seq.iter (fun x -> 
        printfn "%-7s %-19s %.2f [%.8f]" x.Symbol (x.Timestamp.ToString("s")) x.Price x.TradeSize)
    
    connection.Close()

[<EntryPoint>]
let main argv =
    createAndPopulateDatabase
    0 // return an integer exit code