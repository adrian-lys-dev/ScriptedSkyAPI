using Core.Entities;
using Core.Specificatios.Base;

namespace Core.Specificatios
{
    public class SingleBookSpecification : BaseSpecification<Book>
    {
        public SingleBookSpecification(int id) : base(x => x.Id == id) 
        {
            AddInclude(p => p.Author);
            AddInclude(p => p.Genre);
            AddInclude(p => p.Publisher);
        }
    }
}
