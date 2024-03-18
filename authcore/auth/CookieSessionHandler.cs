using System.Text.Json;
using Microsoft.EntityFrameworkCore;

public static class CookieSessionHandler
{
public static async Task<string> CreateSession(int userId, CookieSessionContext sessionCtx)
{
    CookieSessionDto sessionDto = new();
    sessionDto.UserId = userId;
    sessionDto.CreationDate = DateTime.UtcNow;
    sessionDto.ExpirationDate = DateTime.UtcNow.AddMinutes(5);

    var setializedDto = JsonSerializer.Serialize(sessionDto);
    var sessionId = Aes.Encrypt(setializedDto);

    CookieSession session = new();
    session.Id = sessionId;
    session.UserId = userId;
    session.CreationDate = sessionDto.CreationDate;
    session.ExpirationDate = sessionDto.ExpirationDate;

    sessionCtx.Add(session);
    await sessionCtx.SaveChangesAsync();

    return session.Id;
}

	public static async Task<bool> ValidateSession(string sessionId, CookieSessionContext sessionCtx)
	{
		var session = await sessionCtx.CookieSessions.FirstOrDefaultAsync(s => s.Id == sessionId);
		if (session == null)
		{
			return false;
		}
		if (session.ExpirationDate < DateTime.Now)
		{
			sessionCtx.Remove(session);
			await sessionCtx.SaveChangesAsync();
			return false;
		}
		return true;
	}
}