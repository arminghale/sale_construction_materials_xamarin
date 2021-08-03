using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataBase
{
    public class PrGroup
    {
        public PrGroup()
        {

        }
        public int id { get; set; }
        public string title { get; set; }
        public string vahed { get; set; }

        public virtual List<Field> Fields { get; set; }
        public virtual List<Product> Products { get; set; }
    }
}
