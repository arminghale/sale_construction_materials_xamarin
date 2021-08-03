using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace DataBase
{
   public class Users
    {
        public ObservableCollection<User> users { get; set; }
        public Users()
        {
            var response = Consts.client.GetAsync("http://localhost:1212/Admin/api/UsersIndex/").Result;
            if (response.IsSuccessStatusCode)
            {
                var list = JsonConvert.DeserializeObject<List<User>>(response.Content.ReadAsStringAsync().Result);
                users = new ObservableCollection<User>();
                foreach (var item in list)
                {
                    users.Add(item);
                }
            }
        }
    }
}
