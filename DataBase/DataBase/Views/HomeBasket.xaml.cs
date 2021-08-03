using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DataBase.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HomeBasket : ContentPage
    {
        private Basket bask = null;
        public HomeBasket(Basket basket)
        {
            InitializeComponent();
            ActivitySpinner.IsRunning = false;
            menu();

            bask = basket;
            var response2 = Consts.client.GetAsync("http://localhost:1212/User/api/AllAddress/" + Consts.User.id).Result;
            if (response2.IsSuccessStatusCode)
            {
                var list2 = JsonConvert.DeserializeObject<List<Address>>(response2.Content.ReadAsStringAsync().Result);
                foreach (var item2 in list2)
                {
                    var te = item2.id + "-" + item2.ostan + "-" + item2.shahr + "-" + item2.text + "--" + item2.codeposti;
                    Address.Items.Add(te);
                }
            }

            Items.ItemsSource = basket.BasketItems;

            Total.Text = "مبلغ: " + basket.total2;

            Address.SelectedIndexChanged += (s, e) =>
            {
                var selectaddress = int.Parse(Address.SelectedItem.ToString().Split('-')[0]);
                var response3 = Consts.client.GetAsync("http://localhost:1212/User/api/AllAddress/" + Consts.User.id).Result;
                if (response3.IsSuccessStatusCode)
                {
                    var list2 = JsonConvert.DeserializeObject<List<Address>>(response3.Content.ReadAsStringAsync().Result);
                    var choose = list2.FirstOrDefault(w => w.id == selectaddress);
                    Girande.Text = "گیرنده: " + choose.girandename + " " + choose.girandelastname + "-" + choose.girandephonenumber;
                }
            };

            btn_add.Clicked +=async (s, e) =>
            {
                if (Address.SelectedIndex==-1)
                {
                    await DisplayAlert("خطا!", ".لطفا آدرس مورد نظر را انتخاب کنید", "باشه");
                }
                else
                {
                   var selectaddress = int.Parse(Address.SelectedItem.ToString().Split('-')[0]);
                    var response3 = Consts.client.GetAsync("http://localhost:1212/api/BasketOK/?id="+Consts.User.id+"&address=" + selectaddress).Result;
                    if (response3.IsSuccessStatusCode)
                    {
                        await DisplayAlert("موفق!", JsonConvert.DeserializeObject<ErrorMessages>(response3.Content.ReadAsStringAsync().Result).message, "مرسی");
                        await Navigation.PushAsync(new Home());
                    }
                    else
                    {
                        await DisplayAlert("", JsonConvert.DeserializeObject<ErrorMessages>(response3.Content.ReadAsStringAsync().Result).message, "باشه");
                    }
                }
            };

        }
        private async void Items_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            var item = e.Item as BasketItem;
            var response2 = Consts.client.GetAsync("http://localhost:1212/api/Products/" + item.productid).Result;
            if (response2.IsSuccessStatusCode)
            {
                var list = JsonConvert.DeserializeObject<Product>(response2.Content.ReadAsStringAsync().Result);
                await Navigation.PushAsync(new HomeProduct(list));
            }
        }
        private async void btn_up_Clicked(object sender, EventArgs e)
        {
            var but = (Button)sender;
            var grid = (Grid)but.Parent.Parent;
            var Label = (Label)grid.Children[0];
            var id = Label.ClassId;
            var formVariables = new List<KeyValuePair<string, string>>();
            formVariables.Add(new KeyValuePair<string, string>("userid", Consts.User.id.ToString()));
            formVariables.Add(new KeyValuePair<string, string>("prid", bask.BasketItems.FirstOrDefault(w=>w.id==int.Parse(id)).productid.ToString()));
            var formContent = new FormUrlEncodedContent(formVariables);
            var response = Consts.client.PostAsync("http://localhost:1212/api/BasketAdd/", formContent).Result;
            if (response.IsSuccessStatusCode)
            {
                var list = JsonConvert.DeserializeObject<Basket>(response.Content.ReadAsStringAsync().Result);
                await DisplayAlert("موفق", "با موفقیت انجام شد", "مرسی");
                await Navigation.PushAsync(new HomeBasket(list));
            }
            else
            {
                await DisplayAlert("خطا", JsonConvert.DeserializeObject<ErrorMessages>(response.Content.ReadAsStringAsync().Result).message, "باشه");
            }
        }

        private async void btn_down_Clicked(object sender, EventArgs e)
        {
            var but = (Button)sender;
            var grid = (Grid)but.Parent.Parent;
            var Label = (Label)grid.Children[0];
            var id = Label.ClassId;
            var formVariables = new List<KeyValuePair<string, string>>();
            formVariables.Add(new KeyValuePair<string, string>("id", id));
            var formContent = new FormUrlEncodedContent(formVariables);
            var response = Consts.client.PostAsync("http://localhost:1212/api/BasketDown/", formContent).Result;
            if (response.IsSuccessStatusCode)
            {
                var list = JsonConvert.DeserializeObject<Basket>(response.Content.ReadAsStringAsync().Result);
                await DisplayAlert("موفق", "با موفقیت انجام شد", "مرسی");
                await Navigation.PushAsync(new HomeBasket(list));
            }
            else
            {
                await DisplayAlert("خطا", JsonConvert.DeserializeObject<ErrorMessages>(response.Content.ReadAsStringAsync().Result).message, "باشه");
            }
        }

        private async void btn_del_Clicked(object sender, EventArgs e)
        {
            var but = (Button)sender;
            var grid = (Grid)but.Parent.Parent;
            var Label = (Label)grid.Children[0];
            var id = Label.ClassId;
            var formVariables = new List<KeyValuePair<string, string>>();
            formVariables.Add(new KeyValuePair<string, string>("id", id));
            var formContent = new FormUrlEncodedContent(formVariables);
            var response = Consts.client.PostAsync("http://localhost:1212/api/BasketItemDelete/", formContent).Result;
            if (response.IsSuccessStatusCode)
            {
                var list = JsonConvert.DeserializeObject<Basket>(response.Content.ReadAsStringAsync().Result);
                await DisplayAlert("موفق", "با موفقیت انجام شد", "مرسی");
                await Navigation.PushAsync(new HomeBasket(list));
            }
            else
            {
                await DisplayAlert("خطا", JsonConvert.DeserializeObject<ErrorMessages>(response.Content.ReadAsStringAsync().Result).message, "باشه");
            }
        }


        private void menu()
        {
            if (Consts.isUserLoged)
            {
                logreg.IsEnabled = false;
            }
            else
            {
                prof.IsEnabled = false;
            }
        }
        private async void btn_notlog(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new Login());
        }
        private async void btn_profile(object sender, EventArgs e)
        {
            if (Consts.User.roleid == 2)
            {
              await  Navigation.PushAsync(new UserHome());
            }
            else if (Consts.User.roleid == 1)
            {
                await Navigation.PushAsync(new AdminHome());
            }
            else
            {
                await Navigation.PushAsync(new AnbarHome());
            }
        }
        private async void btn_Home(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new Home());
        }

        
    }
}