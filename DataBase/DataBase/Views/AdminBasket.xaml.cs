using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DataBase.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AdminBasket : ContentPage
    {
        public AdminBasket()
        {
            InitializeComponent();
            menu();
            ActivitySpinner.IsRunning = false;
            btn_add.Clicked += async (s, e) =>
             {
                 await Navigation.PushAsync(new AdminAddBasket());
             };
        }
        public static int id;
        private async void btn_change(object sender, EventArgs e)
        {
            var but = (Button)sender;
            var grid = (Grid)but.Parent.Parent;
            var Label = (Label)grid.Children[0];
            var id = Label.ClassId;
            var formVariables = new List<KeyValuePair<string, string>>();
            formVariables.Add(new KeyValuePair<string, string>("id", id));
            formVariables.Add(new KeyValuePair<string, string>("ready", true.ToString()));
            var formContent = new FormUrlEncodedContent(formVariables);
            var response = Consts.client.PostAsync("http://localhost:1212/Admin/api/BasketEdit/", formContent).Result;
            if (response.IsSuccessStatusCode)
            {
                await DisplayAlert("موفق", "با موفقیت انجام شد", "مرسی");
                await Navigation.PushAsync(new AdminBasket());
            }
            else
            {
                await DisplayAlert("خطا", JsonConvert.DeserializeObject<ErrorMessages>(response.Content.ReadAsStringAsync().Result).message, "باشه");
            }
        }
        private async void btn_change2(object sender, EventArgs e)
        {
            var but = (Button)sender;
            var grid = (Grid)but.Parent.Parent;
            var Label = (Label)grid.Children[0];
            var id = Label.ClassId;
            var formVariables = new List<KeyValuePair<string, string>>();
            formVariables.Add(new KeyValuePair<string, string>("id", id));
            formVariables.Add(new KeyValuePair<string, string>("send", true.ToString()));
            var formContent = new FormUrlEncodedContent(formVariables);
            var response = Consts.client.PostAsync("http://localhost:1212/Admin/api/BasketEdit/", formContent).Result;
            if (response.IsSuccessStatusCode)
            {
                await DisplayAlert("موفق", "با موفقیت انجام شد", "مرسی");
                await Navigation.PushAsync(new AdminBasket());
            }
            else
            {
                await DisplayAlert("خطا", JsonConvert.DeserializeObject<ErrorMessages>(response.Content.ReadAsStringAsync().Result).message, "باشه");
            }
        }
        private async void btn_info(object sender, EventArgs e)
        {
            var but = (Button)sender;
            var grid = (Grid)but.Parent.Parent;
            var Label = (Label)grid.Children[0];
            var id = Label.ClassId;
            AdminBasket.id = int.Parse(id);
            var response = Consts.client.GetAsync("http://localhost:1212/Admin/api/BasketDetails/" + id).Result;
            if (response.IsSuccessStatusCode)
            {
                var list = JsonConvert.DeserializeObject<Basket>(response.Content.ReadAsStringAsync().Result);
                await Navigation.PushAsync(new AdminDetailsBasket(list));
            }
        }
        private async void btn_delete(object sender, EventArgs e)
        {
            var but = (Button)sender;
            var grid = (Grid)but.Parent.Parent;
            var Label = (Label)grid.Children[0];
            var id = Label.ClassId;
            var response = Consts.client.GetAsync("http://localhost:1212/Admin/api/BasketDelete/" + id).Result;
            if (response.IsSuccessStatusCode)
            {
                await DisplayAlert("موفق", "با موفقیت انجام شد", "مرسی");
                await Navigation.PushAsync(new AdminBasket());
            }
            else
            {
                await DisplayAlert("خطا", JsonConvert.DeserializeObject<ErrorMessages>(response.Content.ReadAsStringAsync().Result).message, "باشه");
            }
        }
        private async void btn_cansel(object sender, EventArgs e)
        {
            var but = (Button)sender;
            var grid = (Grid)but.Parent.Parent;
            var Label = (Label)grid.Children[0];
            var id = Label.ClassId;
            var response = Consts.client.GetAsync("http://localhost:1212/Admin/api/BasketCansel/?id=" + id+"&iscansel=true").Result;
            if (response.IsSuccessStatusCode)
            {
                await DisplayAlert("موفق", "با موفقیت انجام شد", "مرسی");
                await Navigation.PushAsync(new AdminBasket());
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
                await Navigation.PushAsync(new UserHome());
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