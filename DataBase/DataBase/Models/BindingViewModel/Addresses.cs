using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace DataBase
{
   public class Addresses
    {
        public ObservableCollection<Address> addresses { get; set; }
        public Addresses()
        {
            var response = Consts.client.GetAsync("http://localhost:1212/User/api/AllAddress/"+Consts.User.id).Result;
            if (response.IsSuccessStatusCode)
            {
                var list = JsonConvert.DeserializeObject<List<Address>>(response.Content.ReadAsStringAsync().Result);
                addresses = new ObservableCollection<Address>();
                foreach (var item in list)
                {
                    addresses.Add(item);
                }
            }
        }
    }
}
