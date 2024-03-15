using Microsoft.EntityFrameworkCore;

public struct isValidResult
{
  public bool isValid;
  public List<string> errorStrings;
}

public static class UserValidation
{
  public static async Task<isValidResult> IsValid(User user, UserContext ctx)
  {
    List<string> errorStrings = new();
    isValidResult result = new isValidResult();

    if (string.IsNullOrEmpty(user.Password) || user.Password.Length < 8 || user.Password.Length > 32)
    {
      errorStrings.Add("Invalid password");
    }
    if (await ctx.Users.AnyAsync(u => u.Email == user.Email))
    {
      errorStrings.Add("Email has already been taken");
    }
    if (string.IsNullOrEmpty(user.Email) || user.Email.Length < 3 || !user.Email.Contains("@"))
    {
      errorStrings.Add("Invalid email");
    }
    if (errorStrings.Count > 0)
    {
      result.isValid = false;
      result.errorStrings = errorStrings;
      return result;
    }
    result.isValid = true;
    result.errorStrings = errorStrings;
    return result;
  }
}