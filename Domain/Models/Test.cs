using Domain.Common;

namespace Domain.Models;

public class Test : BaseEntity
{
	public AppUser User { get; set; }
	public List<Question> Questions { get; set; }
	public DateTime StartTime { get; private set; }
	public DateTime EndTime { get; private set; }
	public bool Started { get; private set; }
	public bool Done { get; private set; }
	public void MarkAsStarted()
	{
		if(Started)
			throw new Exception($"Test with id {Id} has already been started");
		Started = true;
		StartTime = DateTime.Now;
	}
	public void MarkAsDone()
	{
		if (!Started)
			throw new Exception($"Test with id {Id} has not been started");
		if (Done)
			throw new Exception($"Test with id {Id} has already been completed");
		Done = true;
		EndTime = DateTime.Now;
	}
	internal Question GetQuestionById(Guid id)
	{
		if (Questions == null)
			throw new ArgumentNullException(nameof(Questions));

		var question = Questions.FirstOrDefault(x => x.Id == id);
		if (question == null)
			throw new ArgumentNullException($"{nameof(Questions)} no have question with id {id}");
		return question;
	}
	internal void SetQuestionAnswer(Guid questionId, string answer, bool isCorrect)
	{
		Question? question = GetQuestionById(questionId);
		question.SetAnswer(answer, isCorrect);
	}
}
