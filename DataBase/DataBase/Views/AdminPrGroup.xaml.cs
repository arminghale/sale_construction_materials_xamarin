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
    public partial class AdminPrGroup : ContentPage
    {
        public AdminPrGroup()
        {
            InitializeComponent();
            ActivitySpinner.IsRunning = false;
            menu();
            btn_add.Clicked += async (s, e) =>
             {
                 await Navigation.PushAsync(new AdminAddEditPrGroup());
             };
        }

        private async void btn_edit(object sender, EventArgs e)
        {
            var but = (Button)sender;
            var grid = (Grid)but.Parent.Parent;
            var Label = (Label)grid.Children[0];
            var id = Label.ClassId;
            var response = Consts.client.GetAsync("http://localhost:1212/Admin/api/PrGroupIndex/").Result;
            if (response.IsSuccessStatusCode)
            {
                var list = JsonConvert.DeserializeObject<List<PrGroup>>(response.Content.ReadAsStringAsync().Result);
                await Navigation.PushAsync(new AdminAddEditPrGroup(list.FirstOrDefault(w=>w.id==int.Parse(id))));
            }
        }
        private async void btn_info(object sender, EventArgs e)
        {
            var but = (Button)sender;
            var grid = (Grid)but.Parent.Parent;
            var Label = (Label)grid.Children[0];
            var id = Label.ClassId;
            var response = Consts.client.GetAsync("http://localhost:1212/Admin/api/PrGroupIndex/").Result;
            if (response.IsSuccessStatusCode)
            {
                var list = JsonConvert.DeserializeObject<List<PrGroup>>(response.Content.ReadAsStringAsync().Result);
                await Navigation.PushAsync(new AdminDetailsPrGroup(list.FirstOrDefault(w => w.id == int.Parse(id))));
            }
        }
        private async void btn_delete(object sender, EventArgs e)
        {
            var but = (Button)sender;
            var grid = (Grid)but.Parent.Parent;
            var Label = (Label)grid.Children[0];
            var id = Label.ClassId;
            var response = Consts.client.GetAsync("http://localhost:1212/Admin/api/PrGroupDelete/"+id).Result;
            if (response.IsSuccessStatusCode)
            {
                await DisplayAlert("موفق", "با موفقیت انجام شد", "مرسی");
                await Navigation.PushAsync(new AdminPrGroup());
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