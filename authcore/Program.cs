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
    return result.errorStrings.ToString();
  }

  user.Password = Aes.Encrypt(user.Password);

  ctx.Users.Add(user);
  await ctx.SaveChangesAsync();
  return "User registered";
});

app.Run();