using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace DataBase
{
   public class PrGroups
    {
        public ObservableCollection<PrGroup> groups { get; set; }
        public PrGroups()
        {
            var response = Consts.client.GetAsync("http://localhost:1212/Admin/api/PrGroupIndex/").Result;
            if (response.IsSuccessStatusCode)
            {
                var list = JsonConvert.DeserializeObject<List<PrGroup>>(response.Content.ReadAsStringAsync().Result);
                groups = new ObservableCollection<PrGroup>();
                foreach (var item in list)
                {
                    groups.Add(item);
                }
            }
        }
    }
}
