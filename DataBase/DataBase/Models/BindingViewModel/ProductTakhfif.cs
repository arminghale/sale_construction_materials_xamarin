using DataBase.Views;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace DataBase
{
   public class ProductTakhfif
    {
        public ObservableCollection<Takhfif> Takhfifs { get; set; }
        public ProductTakhfif()
        {
            var response = Consts.client.GetAsync("http://localhost:1212/Admin/api/ProductTakhfifs/"+AdminProduct.id).Result;
            if (response.IsSuccessStatusCode)
            {
                var list = JsonConvert.DeserializeObject<List<Takhfif>>(response.Content.ReadAsStringAsync().Result);
                Takhfifs = new ObservableCollection<Takhfif>();
                foreach (var item in list)
                {
                    Takhfifs.Add(item);
                }
            }
        }
    }
}
