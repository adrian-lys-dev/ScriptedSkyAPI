using Core.Entities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class Publisher : BaseEntity
    {
        public required string Name { get; set; }
        public List<Book>? Books { get; set; }
    }
}
