open System
open System.Data.SQLite

type TradeData = {
    Symbol : string
    Timestamp : DateTime
    Price : float
    TradeSize : float
}

let trades = [
    { Symbol = "BTC/USD"; Timestamp = DateTime(2017, 07, 28, 10, 44, 33); Price = 2751.20; TradeSize = 0.01000000 }
    { Symbol = "BTC/USD"; Timestamp = DateTime(2017, 07, 28, 10, 44, 21); Price = 2750.20; TradeSize = 0.01000000 }
    { Symbol = "BTC/USD"; Timestamp = DateTime(2017, 07, 28, 10, 44, 21); Price = 2750.01; TradeSize = 0.40000000 }
    { Symbol = "BTC/USD"; Timestamp = DateTime(2017, 07, 28, 10, 44, 21); Price = 2750.01; TradeSize = 0.55898959 }
    { Symbol = "BTC/USD"; Timestamp = DateTime(2017, 07, 28, 10, 44, 03); Price = 2750.00; TradeSize = 0.86260000 }
    { Symbol = "BTC/USD"; Timestamp = DateTime(2017, 07, 28, 10, 44, 03); Price = 2750.00; TradeSize = 0.03000000 }
    { Symbol = "BTC/USD"; Timestamp = DateTime(2017, 07, 28, 10, 43, 31); Price = 2750.01; TradeSize = 0.44120000 } 
]

let createAndPopulateDatabase =
    
    // Create database from scratch
    let databaseFilename = "sample.sqlite"
    let connectionString = sprintf "DataSource=%s;Version=3;" databaseFilename

    SQLiteConnection.CreateFile(databaseFilename)
    let connection = new SQLiteConnection(connectionString)
    connection.Open()
    let structureSql =
        "create table Trades (" +
        "Symbol varchar(20), " +
        "Timestamp datetime, " + 
        "Price float, " + 
        "TradeSize float)"

    let structureCommand = new SQLiteCommand(structureSql, connection)
    structureCommand.ExecuteNonQuery() |> ignore 

    // Add records
    let insertSql = 
        "insert into trades(symbol, timestamp, price, tradesize) " + 
        "values (@symbol, @timestamp, @price, @tradesize)"

    trades
    |> List.map(fun x ->
        use command = new SQLiteCommand(insertSql, connection)
        command.Parameters.AddWithValue("@symbol", x.Symbol) |> ignore
        command.Parameters.AddWithValue("@timestamp", x.Timestamp) |> ignore
        command.Parameters.AddWithValue("@price", x.Price) |> ignore
        command.Parameters.AddWithValue("@tradesize", x.TradeSize) |> ignore

        command.ExecuteNonQuery())
    |> List.sum
    |> (fun recordsAdded -> printfn "Records added: %d" recordsAdded)

    // Query records
    let selectSql = "select * from trades order by timestamp desc"
    let selectCommand = new SQLiteCommand(selectSql, connection)
    let reader = selectCommand.ExecuteReader()
    while reader.Read() do
        printfn "%-7s %-19s %.2f [%.8f]" 
            (reader.["symbol"].ToString()) 
            (Convert.ToDateTime(reader.["timestamp"]).ToString("s"))
            (Convert.ToDouble(reader.["price"])) 
            (Convert.ToDouble(reader.["tradesize"]))

    connection.Close()
        
[<EntryPoint>]
let main argv =
    createAndPopulateDatabase
    0 // return an integer exit code