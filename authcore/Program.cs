using System.Reflection.Metadata.Ecma335;
using System.Text.Json;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

// database
await using var ctx = new UserContext();
await ctx.Database.EnsureDeletedAsync();
await ctx.Database.EnsureCreatedAsync();

app.MapGet("/", () => "Hello World!");

app.MapPost("/register", async (User user) =>
{
  isValidResult result = await UserValidation.IsValid(user, ctx);
  if (!result.isValid)
  {
    var jsonString = JsonSerializer.Serialize(result.errorStrings);
    return Results.BadRequest(jsonString);
  }

  user.Password = Aes.Encrypt(user.Password);

  ctx.Users.Add(user);
  await ctx.SaveChangesAsync();
  return Results.Ok("User registered"); 
});

app.MapPost("/login", async (User user) =>
{
  var fetchedUser = await ctx.Users.FirstOrDefaultAsync(u => u.Email == user.Email);
  if (fetchedUser == null)
  {
    return Results.BadRequest("Invalid cridentials");
  }
  string decriptedPassword;
  try {
    decriptedPassword = Aes.Decrypt(fetchedUser.Password, user.Password);
  } catch (Exception e) {
    return Results.BadRequest("Invalid cridentials");
  }
  if (decriptedPassword == user.Password)
  {
    return Results.Ok("Correct cridentials");
  }
  else
  {
    return Results.BadRequest("Invalid cridentials");
  }
});

app.Run();