﻿using Telegram.Bot.Types;
using Telegram.Bot;

namespace Client.Extensions;

public static class ContextExtensions
{
	public static async Task TryDeleteMessageAsync(this ITelegramBotClient client, ChatId chatId, int? messageId)
	{
		try
		{
			await client.DeleteMessageAsync(chatId, (int)messageId);
		}
		catch
		{
			return;
		}
	}
}
