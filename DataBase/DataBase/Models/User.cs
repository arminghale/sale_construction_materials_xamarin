using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataBase
{
    public class User
    {
        public User()
        {

        }

        public int id { get; set; }
        public int roleid { get; set; }

        public string email { get; set; }

        public string username { get; set; }

        public string password { get; set; }

        public System.DateTime lastlogin { get; set; }

        public int activecode { get; set; }
        public bool isactive { get; set; }
        public bool ad2 { get; set; }

        public string ad
        {
            get
            {
                if (Person == null)
                {
                    ad2 = false;
                    return username;
                }
                else
                {
                    ad2 = true;
                    return Person.name + " " + Person.lastname;
                }
                 
            }
        }
        public bool ad3
        {
            get
            {
                if (roleid == 3)
                {
                    return false;
                }
                else
                {
                    return true;
                }

            }
        }

        public virtual Role Role { get; set; }
        public virtual Person Person { get; set; }
        public virtual List<Message> Messages { get; set; }

    }
}
