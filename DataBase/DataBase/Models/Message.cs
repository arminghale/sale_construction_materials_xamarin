using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataBase
{
    public class Message
    {
        public Message()
        {

        }
        public int id { get; set; }   
        public int userid { get; set; }
        public int? user2id { get; set; }
        public string text { get; set; }
        public bool isseen { get; set; }
        public System.DateTime createdate { get; set; }
        public string sendername { get; set; }
        public string senderlastname { get; set; }
        public string senderemail { get; set; }
        public virtual User User { get; set; }
        public virtual User User2 { get; set; }


        public bool see
        {
            get
            {
                return !isseen;
            }
        }

        public string te2
        {
            get
            {
                if (User2==null)
                {
                    return string.Format("{0} {1}", sendername,senderlastname);
                }
                else if (User2.Person!=null)
                {
                    return string.Format("{0} {1}", User2.Person.name, User2.Person.lastname);
                }
                else if (User2.Person==null)
                {
                    return string.Format("{0}", User2.username);
                }
                return te;
            }
        }

        public string te
        {
            get
            {
                string a = "";
                foreach (var item in text.Split(' ').Take(5))
                {
                    a += item + " ";
                }
                return string.Format("{0} . . .",a );
            }
        }
    }
}
