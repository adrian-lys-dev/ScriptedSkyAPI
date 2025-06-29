using Core.Entities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class Author : BaseEntity
    {
        public required string Name { get; set; }
        public List<Book>? Book { get; set; }
    }
}
