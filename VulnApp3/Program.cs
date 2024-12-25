using System.Text;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

var secret = "hidden";
//example jwt: eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJuYW1lIjoidXNlciJ9.a2Z1XNb5neg7-Xgn4L0N4dmpc-4J7A0Ao3E2rcmhEAA
app.MapGet("/login/{jwt}", (string jwt) =>
{
    string? json = default;
    try
    {
        json = Jose.JWT.Decode(jwt, Encoding.UTF8.GetBytes(secret), Jose.JwsAlgorithm.HS256);
    }
    catch
    {
        return Results.Unauthorized();
    }

    if (json == null)
    {
        return Results.Unauthorized();
    }

    var user = JsonSerializer.Deserialize<User>(json);

    if (user.Name == "admin")
        return Results.Ok("you are the boss");

    return Results.Ok("you are the user");
});

app.Run();

record User(string Name);