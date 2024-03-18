using System.Text.Json;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// database
await using var ctx = new UserContext();
// await ctx.Database.EnsureDeletedAsync();
await ctx.Database.EnsureCreatedAsync();

await using var sessionCtx = new CookieSessionContext();
// await sessionCtx.Database.EnsureDeletedAsync();
await sessionCtx.Database.EnsureCreatedAsync();

builder.Services.AddDbContext<UserContext>();
builder.Services.AddDbContext<CookieSessionContext>();
builder.Services.AddCors();

var app = builder.Build();

app.UseCors(builder => builder
    .WithOrigins("http://localhost:5173")
    .AllowAnyMethod()
    .AllowAnyHeader()
    .AllowCredentials()
);

app.UseCors(builder => builder.AllowAnyOrigin());

app.MapGet("/", async context =>
{
  var sessionCtx = context.RequestServices.GetService<CookieSessionContext>();
  var cookie = context.Request.Cookies["sessionId"];

  if (sessionCtx == null || string.IsNullOrEmpty(cookie))
  {
    context.Response.StatusCode = StatusCodes.Status500InternalServerError;
    await context.Response.WriteAsJsonAsync("1");
    return;
  }
  if (await CookieSessionHandler.ValidateSession(cookie, sessionCtx))
  {
    context.Response.StatusCode = StatusCodes.Status200OK;
    await context.Response.WriteAsJsonAsync("Hello World");
  }
  else
  {
    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
    await context.Response.WriteAsJsonAsync("2");
  }
});

app.MapPost("/register", async (User user, UserContext ctx) =>
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

app.MapPost("/login", async (User user, HttpContext httpCtx, UserContext ctx, CookieSessionContext sessionCtx) =>
{
  var fetchedUser = await ctx.Users.FirstOrDefaultAsync(u => u.Email == user.Email);
  if (fetchedUser == null)
  {
    return Results.BadRequest("Invalid cridentials1");
  }
  string decriptedPassword;
  try
  {
    decriptedPassword = Aes.Decrypt(fetchedUser.Password, user.Password);
  }
  catch (Exception e)
  {
    return Results.BadRequest("Invalid cridentials2");
  }
  if (decriptedPassword == user.Password)
  {
    var sessionId = await CookieSessionHandler.CreateSession(fetchedUser.Id, sessionCtx);
    if (sessionId == null || string.IsNullOrEmpty(sessionId))
    {
      return Results.BadRequest("Invalid cridentials3");
    }
    httpCtx.Response.Headers.Append("Set-Cookie", $"sessionId={sessionId}; Max-Age=900;");
    return Results.Ok(user.Email);
  }
  else
  {
    return Results.BadRequest("Invalid cridentials4");
  }
});

app.Run();

// using (var scope = app.Services.CreateScope())
// {
//   var services = scope.ServiceProvider;

//   var ctx = services.GetRequiredService<UserContext>();
//   await ctx.Database.EnsureCreatedAsync();
//   if (ctx != null && ctx.Database.CanConnect())
//   {
//     Console.WriteLine("User migration");
//     await ctx.Database.MigrateAsync();
//   }

//   var sessionCtx = services.GetRequiredService<CookieSessionContext>();
//   await sessionCtx.Database.EnsureCreatedAsync();

//   if (sessionCtx != null && sessionCtx.Database.CanConnect())
//   {
//     Console.WriteLine("Session migration");
//     await sessionCtx.Database.MigrateAsync();
//   }
// }