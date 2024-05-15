using Basic.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basic.DataAccesEF
{
    public class ItemContext : DbContext
    {
        public ItemContext(DbContextOptions options) : base(options) { }

        public DbSet<Item> Item
        {
            get;
            set;
        }
        public DbSet<Category> Category
        {
            get;
            set;
        }
    }
}
