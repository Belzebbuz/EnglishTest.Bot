using Domain.Extensions;

namespace Domain.Common;

public abstract class BaseEntity : IEntity<Guid>
{
	public Guid Id { get; set ; }
	public DateTime CreateDate { get ; private set; } = DateTime.Now.ConvertToMoscowTime();
}