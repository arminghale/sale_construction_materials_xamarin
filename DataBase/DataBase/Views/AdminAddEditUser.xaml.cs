using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Newtonsoft.Json;
using Plugin.Media;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DataBase.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AdminAddEditUser : ContentPage
    {
        List<Role> list;
        int select;
        public AdminAddEditUser(User user = null)
        {
            InitializeComponent();
            ActivitySpinner.IsRunning = false;
            menu();

            var response = Consts.client.GetAsync("http://localhost:1212/Admin/api/RoleIndex/").Result;
            if (response.IsSuccessStatusCode)
            {
                list = JsonConvert.DeserializeObject<List<Role>>(response.Content.ReadAsStringAsync().Result);
                foreach (var item in list)
                {
                    if (item.title!="admin")
                    {
                        roleid.Items.Add(item.title);
                        if (user != null && item.id == user.roleid)
                        {
                            roleid.SelectedItem = item.title;
                            select = item.id;
                        }
                    }   
                }
            }
            if (user!=null)
            {

                username.Text = user.username;
                password.Text = user.password;
                email.Text = user.email;
            }
            roleid.SelectedIndexChanged += async (s, e) =>
              {
                  var picker = (Picker)s;
                  int selectedIndex = picker.SelectedIndex;
                  Role p = list.FirstOrDefault(w => w.title == roleid.Items[selectedIndex]);
                  select = p.id;
              };

            btn_laghv.Clicked += async (s, e) =>
              {
                  await Navigation.PopAsync();
              };

            btn_taiid.Clicked += async (s, e) =>
            {
                if (user!=null)
                {
                    var items = new
                    {
                        id=user.id,
                        email = email.Text,
                        username = username.Text,
                        password = password.Text,
                        roleid = select
                    };
                    var content = new StringContent(JsonConvert.SerializeObject(items), Encoding.UTF8, "application/json");
                    var response2 = Consts.client.PostAsync("http://localhost:1212/Admin/api/UsersEdit", content).Result;
                    if (response2.IsSuccessStatusCode)
                    {
                        await DisplayAlert("موفق", "با موفقیت ثبت شد", "مرسی");
                        await Navigation.PushAsync(new AdminUser());
                    }
                    else
                    {
                        await DisplayAlert("خطا", JsonConvert.DeserializeObject<ErrorMessages>(response2.Content.ReadAsStringAsync().Result).message, "باشه");
                    }
                }
                else
                {
                    var items = new
                    {
                        email = email.Text,
                        username = username.Text,
                        password = password.Text,
                        roleid = select,
                        id=0
                    };
                     var  content = new StringContent(JsonConvert.SerializeObject(items), Encoding.UTF8, "application/json");
                    var response2 = Consts.client.PostAsync("http://localhost:1212/Admin/api/UsersCreate", content).Result;
                    if (response2.IsSuccessStatusCode)
                    {
                        await DisplayAlert("موفق", "با موفقیت ثبت شد", "مرسی");
                        await Navigation.PushAsync(new AdminUser());
                    }
                    else
                    {
                        await DisplayAlert("خطا", JsonConvert.DeserializeObject<ErrorMessages>(response2.Content.ReadAsStringAsync().Result).message, "باشه");
                    }
                }
                
            };
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