using Microsoft.Data.Sqlite;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
        policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
});

var app = builder.Build();

app.UseCors();

var dbPath = Path.Combine(app.Environment.ContentRootPath, "..", "BowlingLeague.sqlite");

app.MapGet("/api/bowlers", () =>
{
    var bowlers = new List<Bowler>();

    using var connection = new SqliteConnection($"Data Source={dbPath}");
    connection.Open();

    var cmd = connection.CreateCommand();
    cmd.CommandText = """
        SELECT
            b.BowlerFirstName,
            b.BowlerMiddleInit,
            b.BowlerLastName,
            t.TeamName,
            b.BowlerAddress,
            b.BowlerCity,
            b.BowlerState,
            b.BowlerZip,
            b.BowlerPhoneNumber
        FROM Bowlers b
        JOIN Teams t ON b.TeamID = t.TeamID
        WHERE t.TeamName IN ('Marlins', 'Sharks')
        ORDER BY t.TeamName, b.BowlerLastName, b.BowlerFirstName
        """;

    using var reader = cmd.ExecuteReader();
    while (reader.Read())
    {
        bowlers.Add(new Bowler(
            reader.GetString(0),
            reader.IsDBNull(1) ? "" : reader.GetString(1),
            reader.GetString(2),
            reader.GetString(3),
            reader.GetString(4),
            reader.GetString(5),
            reader.GetString(6),
            reader.GetString(7),
            reader.GetString(8)
        ));
    }

    return Results.Json(bowlers);
});

app.Run();

record Bowler(
    string FirstName,
    string MiddleInit,
    string LastName,
    string TeamName,
    string Address,
    string City,
    string State,
    string Zip,
    string Phone
);
