using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

// database
await using var ctx = new UserContext();
await ctx.Database.EnsureDeletedAsync();
await ctx.Database.EnsureCreatedAsync();

app.MapGet("/", () => "Hello World!");

app.MapPost("/test", async (User user) => {
  // todo: validate user
  if(await ctx.Users.AnyAsync(u => u.Email == user.Email)){
    return "User already exists";
  }

  user.Password = Aes.Encrypt(user.Password);

  ctx.Users.Add(user);
  await ctx.SaveChangesAsync();
  return "User added";
});

app.Run();