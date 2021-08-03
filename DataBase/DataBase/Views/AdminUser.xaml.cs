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
    public partial class AdminUser : ContentPage
    {
        public AdminUser()
        {
            InitializeComponent();
            ActivitySpinner.IsRunning = false;
            menu();
            btn_add.Clicked += async (s, e) =>
             {
                 await Navigation.PushAsync(new AdminAddEditUser());
             };
        }
        public static int id;
        private async void btn_edit(object sender, EventArgs e)
        {
            var but = (Button)sender;
            var grid = (Grid)but.Parent.Parent;
            var Label = (Label)grid.Children[0];
            var id = Label.ClassId;
            var response = Consts.client.GetAsync("http://localhost:1212/Admin/api/UsersIndex/").Result;
            if (response.IsSuccessStatusCode)
            {
                var list = JsonConvert.DeserializeObject<List<User>>(response.Content.ReadAsStringAsync().Result);
                await Navigation.PushAsync(new AdminAddEditUser(list.FirstOrDefault(w=>w.id==int.Parse(id))));
            }
        }
        private async void btn_info(object sender, EventArgs e)
        {
            var but = (Button)sender;
            var grid = (Grid)but.Parent.Parent;
            var Label = (Label)grid.Children[0];
            var id = Label.ClassId;
            var response = Consts.client.GetAsync("http://localhost:1212/Admin/api/UsersDetails/" + id).Result;
            if (response.IsSuccessStatusCode)
            {
                var list = JsonConvert.DeserializeObject<User>(response.Content.ReadAsStringAsync().Result);
                await Navigation.PushAsync(new AdminDetailsUser(list));
            }
        }
        private async void btn_delete(object sender, EventArgs e)
        {
            var but = (Button)sender;
            var grid = (Grid)but.Parent.Parent;
            var Label = (Label)grid.Children[0];
            var id = Label.ClassId;
            var response = Consts.client.GetAsync("http://localhost:1212/Admin/api/UsersDelete/" + id).Result;
            if (response.IsSuccessStatusCode)
            {
                await DisplayAlert("موفق", "با موفقیت انجام شد", "مرسی");
                await Navigation.PushAsync(new AdminUser());
            }
            else
            {
                await DisplayAlert("خطا", JsonConvert.DeserializeObject<ErrorMessages>(response.Content.ReadAsStringAsync().Result).message, "باشه");
            }
        }
        private async void btn_person(object sender, EventArgs e)
        {
            var but = (Button)sender;
            var grid = (Grid)but.Parent.Parent;
            var Label = (Label)grid.Children[0];
            var id = Label.ClassId;
            var response = Consts.client.GetAsync("http://localhost:1212/Admin/api/UsersDetails/" + id).Result;
            if (response.IsSuccessStatusCode)
            {
                var list = JsonConvert.DeserializeObject<User>(response.Content.ReadAsStringAsync().Result);
                await Navigation.PushAsync(new AdminAddEditPerson(list.id, list.Person));
            }
        }
        private async void btn_address(object sender, EventArgs e)
        {
            var but = (Button)sender;
            var grid = (Grid)but.Parent.Parent;
            var Label = (Label)grid.Children[0];
            var id = Label.ClassId;
            AdminUser.id = int.Parse(id);
            await Navigation.PushAsync(new AdminUserAddresses());
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