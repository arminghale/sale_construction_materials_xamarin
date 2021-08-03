using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataBase
{
    public class Person
    {
        public Person()
        {

        }
        public string name { get; set; }

        public string lastname { get; set; }

        public string phonenumber { get; set; }
        public virtual User User { get; set; }
    }
}
