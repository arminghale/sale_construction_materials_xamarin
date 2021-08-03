using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace DataBase
{
   public class AdminRecMessages
    {
        public ObservableCollection<Message> messages { get; set; }
        public AdminRecMessages()
        {
            var response = Consts.client.GetAsync("http://localhost:1212/Admin/api/MessageIndex/"+Consts.User.id).Result;
            if (response.IsSuccessStatusCode)
            {
                var list = JsonConvert.DeserializeObject<List<Message>>(response.Content.ReadAsStringAsync().Result);
                messages = new ObservableCollection<Message>();
                foreach (var item in list)
                {
                    messages.Add(item);
                }
            }
        }
    }
}
