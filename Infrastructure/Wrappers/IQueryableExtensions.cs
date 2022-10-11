namespace Infrastructure.Wrappers;

public static class IQueryableExtensions
{
	public static IQueryable<T> Paginate<T>(this IQueryable<T> queryable, Pagination pagination)
	{
		pagination.Setup(queryable.Count());

		return queryable
					.Skip((pagination.Page - 1) * pagination.RecordsPerPage)
					.Take(pagination.RecordsPerPage);

	}
}
