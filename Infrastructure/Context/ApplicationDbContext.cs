using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Context;

public class ApplicationDbContext : DbContext
{
	public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
		: base(options)
	{

	}

	public DbSet<AppUser> Users => Set<AppUser>();
	public DbSet<Test> Tests => Set<Test>();
	public DbSet<Word> Words => Set<Word>();
	public DbSet<Question> Questions => Set<Question>();
	public DbSet<VocabularySession> VocabularyOpenHistory => Set<VocabularySession>();
}
