namespace Infrastructure.DTOs;

public class TestDTO
{
	public Guid Id { get; set; }
	public List<QuestionDTO> Questions { get; set; }
}

public class QuestionDTO
{
	public Guid Id { get; set; }
	public WordDTO Word { get; set; }
}

public class WordDTO
{
	public WordDTO(Guid id, string enVersion, string ruVersion)
	{
		Id = id;
		EnVersion = enVersion;
		RuVersion = ruVersion;
	}

	public Guid Id { get; set; }
	public string EnVersion { get; set; }
	public string RuVersion { get; set; }
}