namespace Domain.Common;

public interface IEntity<Tid>
{
	Tid Id { get; set; }
	public DateTime CreateDate { get; }
}
