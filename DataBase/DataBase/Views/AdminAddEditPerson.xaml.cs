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
    public partial class AdminAddEditPerson : ContentPage
    {
        public AdminAddEditPerson(int userid,Person user = null)
        {
            InitializeComponent();
            ActivitySpinner.IsRunning = false;
            menu();

            if (user!=null)
            {

                name.Text = user.name;
                lastname.Text = user.lastname;
                phonenumber.Text = user.phonenumber;
            }

            btn_laghv.Clicked += async (s, e) =>
              {
                  await Navigation.PopAsync();
              };

            btn_taiid.Clicked += async (s, e) =>
            {
                    var items = new
                    {
                        id= userid,
                        name = name.Text,
                        lastname = lastname.Text,
                        phonenumber = phonenumber.Text,
                    };
                    var content = new StringContent(JsonConvert.SerializeObject(items), Encoding.UTF8, "application/json");
                    var response2 = Consts.client.PostAsync("http://localhost:1212/Admin/api/UsersPerson", content).Result;
                    if (response2.IsSuccessStatusCode)
                    {
                        await DisplayAlert("موفق", "با موفقیت ثبت شد", "مرسی");
                        await Navigation.PushAsync(new AdminUser());
                    }
                    else
                    {
                        await DisplayAlert("خطا", JsonConvert.DeserializeObject<ErrorMessages>(response2.Content.ReadAsStringAsync().Result).message, "باشه");
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