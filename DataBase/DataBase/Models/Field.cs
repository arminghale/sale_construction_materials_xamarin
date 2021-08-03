using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataBase
{
    public class Field
    {
        public Field()
        {

        }
        public int id { get; set; }
        public int prgroupid { get; set; }
        public string title { get; set; }
        public virtual PrGroup PrGroup { get; set; }
        public virtual List<FillField> FillFields { get; set; }
    }
}
