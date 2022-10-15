using Domain.Common;

namespace Domain.Models;

public class VocabularySession : BaseEntity
{
	public int MessageId { get; set; }
	public long ChatId { get; set; }
}
