using Domain.Common;

namespace Domain.Models;

public class Question : BaseEntity
{
	public int OrderNumber { get; private set; }
	public Word Word { get; set; }
	public string? Answer { get; private set; }
	public bool IsCorrect { get; private set; }

	internal string? ConfigureResultString()
	{
		return $"{OrderNumber}. | Слово: {Word.EnVersion} | Перевод: {Word.RuVersion} | Ваш ответ: {Answer}";
	}

	internal void SetAnswer(string answer, bool isCorrect)
	{
		if (Answer != null)
			throw new Exception($"{nameof(Question)} with id: {Id} already exist answer:{Answer}");
		Answer = answer;
		IsCorrect = isCorrect;
	}

	internal void SetOrderNumber(int orderNumber)
	{
		OrderNumber = orderNumber;
	}
}