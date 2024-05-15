using Basic.Domain.Interfaces;
using Basic.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basic.DataAccesEF.TypeRepository
{
    internal class CategoryRepository : GenericRepository<Category>, ICategoryRepository
    {
        public CategoryRepository(ItemContext context) : base(context)
        {
        }
    }
}
