using Core.Interfaces;
using System.Linq.Expressions;

namespace Core.Specificatios.Base
{
    public class BaseSpecification<T>(Expression<Func<T, bool>>? criteria) : ISpecification<T>
    {
        protected BaseSpecification() : this(null) { }

        public bool UseSplitQuery { get; private set; } = false;
        public Expression<Func<T, bool>>? Criteria => criteria;

        public Expression<Func<T, object>>? OrderBy { get; private set; }

        public Expression<Func<T, object>>? OrderByDescending { get; private set; }

        public List<Expression<Func<T, object>>> Includes { get; } = new List<Expression<Func<T, object>>>();

        public int Take { get; private set; }

        public int Skip { get; private set; }

        public bool IsPagingEnabled { get; private set; }

        public List<string> IncludeStrings { get; } = [];

        public int? Limit { get; private set; }


        protected void ApplyLimit(int count)
        {
            Limit = count;
        }

        protected void EnableSplitQuery()
        {
            UseSplitQuery = true;
        }

        protected void AddInclude(Expression<Func<T, object>> includeExpression)
        {
            Includes.Add(includeExpression);
        }

        protected void AddInclude(string includeString)
        {
            IncludeStrings.Add(includeString); // For ThenInclude
        }
        protected void AddOrderBy(Expression<Func<T, object>>? orderByExpression)
        {
            OrderBy = orderByExpression;
        }
        protected void AddOrderByDescending(Expression<Func<T, object>>? orderByDescExpression)
        {
            OrderByDescending = orderByDescExpression;
        }
        protected void ApplyPaging(int skip, int take)
        {
            Skip = skip;
            Take = take;
            IsPagingEnabled = true;
        }
        public IQueryable<T> ApplyCriteria(IQueryable<T> query)
        {
            if (Criteria != null)
            {
                query = query.Where(Criteria);
            }

            return query;
        }
    }
}
