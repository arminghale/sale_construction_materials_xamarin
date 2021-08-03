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
    public partial class AnbarProduct : ContentPage
    {
        public AnbarProduct()
        {
            InitializeComponent();
            ActivitySpinner.IsRunning = false;
            menu();
        }
        public static int id;
        private async void btn_edit(object sender, EventArgs e)
        {
            var but = (Button)sender;
            var grid = (Grid)but.Parent.Parent;
            var Label = (Label)grid.Children[0];
            var id = Label.ClassId;
            var response = Consts.client.GetAsync("http://localhost:1212/Anbar/api/ProductDetails/" + id).Result;
            if (response.IsSuccessStatusCode)
            {
                var list = JsonConvert.DeserializeObject<Product>(response.Content.ReadAsStringAsync().Result);
                string result = await DisplayPromptAsync("بروزرسانی تعداد", "موجودی فعلی: "+list.count);
                if (!string.IsNullOrEmpty(result))
                {
                    var formVariables = new List<KeyValuePair<string, string>>();
                    formVariables.Add(new KeyValuePair<string, string>("id", id));
                    formVariables.Add(new KeyValuePair<string, string>("count", result));
                    var formContent = new FormUrlEncodedContent(formVariables);
                    var response4 = Consts.client.PostAsync("http://localhost:1212/Anbar/api/ProductUpdate/", formContent).Result;
                    if (response4.IsSuccessStatusCode)
                    {
                        await DisplayAlert("موفق!", "با موفقیت انجام شد", "باشه");
                        ActivitySpinner.IsRunning = false;

                        await Navigation.PushAsync(new AnbarProduct());
                    }
                    else
                    {
                        await DisplayAlert("خطا!", JsonConvert.DeserializeObject<ErrorMessages>(response4.Content.ReadAsStringAsync().Result).message, "باشه");
                        ActivitySpinner.IsRunning = false;
                    }
                }
                
            }
        }
        private async void btn_info(object sender, EventArgs e)
        {
            var but = (Button)sender;
            var grid = (Grid)but.Parent.Parent;
            var Label = (Label)grid.Children[0];
            var id = Label.ClassId;
            var response = Consts.client.GetAsync("http://localhost:1212/Anbar/api/ProductDetails/"+id).Result;
            if (response.IsSuccessStatusCode)
            {
                var list = JsonConvert.DeserializeObject<Product>(response.Content.ReadAsStringAsync().Result);
                await Navigation.PushAsync(new AnbarDetailsProduct(list));
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