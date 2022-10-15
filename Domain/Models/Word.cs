using Domain.Common;

namespace Domain.Models;

public class Word : BaseEntity
{
	public Word(string enVersion, string ruVersion)
	{
		EnVersion = enVersion;
		RuVersion = ruVersion;
	}
	public AppUser AppUser { get; private set; }
	public string EnVersion { get; private set; }
	public string RuVersion { get; private set; }
}
