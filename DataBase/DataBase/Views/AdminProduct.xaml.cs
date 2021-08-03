using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DataBase.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AdminProduct : ContentPage
    {
        public AdminProduct()
        {
            InitializeComponent();
            menu();
            ActivitySpinner.IsRunning = false;
            btn_add.Clicked += async (s, e) =>
             {
                 await Navigation.PushAsync(new AdminAddEditProduct());
             };
        }
        public static int id;
        private async void btn_edit(object sender, EventArgs e)
        {
            var but = (Button)sender;
            var grid = (Grid)but.Parent.Parent;
            var Label = (Label)grid.Children[0];
            var id = Label.ClassId;
            var response = Consts.client.GetAsync("http://localhost:1212/Admin/api/ProductIndex/").Result;
            if (response.IsSuccessStatusCode)
            {
                var list = JsonConvert.DeserializeObject<List<Product>>(response.Content.ReadAsStringAsync().Result);
                await Navigation.PushAsync(new AdminAddEditProduct(list.FirstOrDefault(w=>w.id==int.Parse(id))));
            }
        }
        private async void btn_info(object sender, EventArgs e)
        {
            var but = (Button)sender;
            var grid = (Grid)but.Parent.Parent;
            var Label = (Label)grid.Children[0];
            var id = Label.ClassId;
            var response = Consts.client.GetAsync("http://localhost:1212/Admin/api/ProductDetails/"+id).Result;
            if (response.IsSuccessStatusCode)
            {
                var list = JsonConvert.DeserializeObject<Product>(response.Content.ReadAsStringAsync().Result);
                await Navigation.PushAsync(new AdminDetailsProduct(list));
            }
        }
        private async void btn_delete(object sender, EventArgs e)
        {
            var but = (Button)sender;
            var grid = (Grid)but.Parent.Parent;
            var Label = (Label)grid.Children[0];
            var id = Label.ClassId;
            var response = Consts.client.GetAsync("http://localhost:1212/Admin/api/ProductDelete/" + id).Result;
            if (response.IsSuccessStatusCode)
            {
                await DisplayAlert("موفق", "با موفقیت انجام شد", "مرسی");
                await Navigation.PushAsync(new AdminProduct());
            }
            else
            {
                await DisplayAlert("خطا", JsonConvert.DeserializeObject<ErrorMessages>(response.Content.ReadAsStringAsync().Result).message, "باشه");
            }
        }
        private async void btn_takhfifs(object sender, EventArgs e)
        {
            var but = (Button)sender;
            var grid = (Grid)but.Parent.Parent;
            var Label = (Label)grid.Children[0];
            var id = Label.ClassId;
            AdminProduct.id = int.Parse(id);
            await Navigation.PushAsync(new AdminTakhfifsProduct(AdminProduct.id));
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