using System.Linq.Expressions;

namespace Application.Interfaces
{
    public interface ISpecification<T>
    {
        Expression<Func<T, bool>>? Criteria { get; }
        List<Expression<Func<T, object>>> Includes { get; }
        List<string> IncludeStrings { get; } // For ThenInclude
        Expression<Func<T, object>>? OrderBy { get; }
        Expression<Func<T, object>>? OrderByDescending { get; }
        int Take { get; }
        int Skip { get; }
        bool IsPagingEnabled { get; }
        int? Limit { get; }
        IQueryable<T> ApplyCriteria(IQueryable<T> query);

        bool UseSplitQuery { get; }
    }
}
