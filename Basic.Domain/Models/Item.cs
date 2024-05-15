using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basic.Domain.Models
{
    public class Item
    {
        public int Id { get; set; }
        public string ItemName { get; set; }
        public int ItemUnit { get; set; }
        public int ItemQuantity { get; set; }
        public Category Category { get; set; }

    }
}
