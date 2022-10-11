namespace Infrastructure.DTOs;

public class AppUserDTO
{
	public AppUserDTO(long tgUserId, string fullName, string userName, long chatId)
	{
		TgUserId = tgUserId;
		FullName = fullName;
		UserName = userName;
		ChatId = chatId;
	}

	public long TgUserId { get; private set; }
	public string FullName { get; private set; }
	public string UserName { get; private set; }
	public long ChatId { get; private set; }
}
