var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();
await using var ctx = new UserContext();
await ctx.Database.EnsureDeletedAsync();
await ctx.Database.EnsureCreatedAsync();

app.MapGet("/", () => "Hello World!");

app.MapPost("/test", async (User user) => {
  // todo: validate user
  ctx.Users.Add(user);
  await ctx.SaveChangesAsync();
  return "User added";
});

app.Run();