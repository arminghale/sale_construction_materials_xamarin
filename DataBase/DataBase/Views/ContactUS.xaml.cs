using Newtonsoft.Json;
using System;
using System.Collections.Generic;
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
    public partial class ContactUS : ContentPage
    {
        public ContactUS()
        {
            InitializeComponent();
            ActivitySpinner.IsRunning = false;
            menu();
            if (Consts.isUserLoged)
            {
                name.IsVisible = false;
                lastname.IsVisible = false;
                email.IsVisible = false;
                lemail.IsVisible = false;
                llastname.IsVisible = false;
                lname.IsVisible = false;
            }

            btn_send.Clicked += async (s, e) =>
            {
                ActivitySpinner.IsRunning = true;
                if (Consts.isUserLoged)
                {
                    if (string.IsNullOrEmpty(text.Text))
                    {
                        await DisplayAlert("خطا!", "لطفا فیلد را تکمیل کنید.", "باشه");
                        ActivitySpinner.IsRunning = false;
                    }
                    else
                    {
                        var user = new StringContent(JsonConvert.SerializeObject(new { user2id= Consts.User.id, text = text.Text, createdate = System.DateTime.Now }), Encoding.UTF8, "application/json");

                        var response = Consts.client.PostAsync("http://localhost:1212/api/MessageToAdmin", user).Result;
                        if (response.IsSuccessStatusCode)
                        {
                            await DisplayAlert("خطا!", "با موفقیت ارسال شد.", "باشه");
                            await Navigation.PushAsync(new UserHome());
                        }
                        else
                        {
                            await DisplayAlert("خطا!", JsonConvert.DeserializeObject<ErrorMessages>(response.Content.ReadAsStringAsync().Result).message, "باشه");
                            ActivitySpinner.IsRunning = false;
                        }
                    }
                    
                }
                else
                {
                    if (string.IsNullOrEmpty(name.Text) || string.IsNullOrEmpty(lastname.Text) || string.IsNullOrEmpty(email.Text) || string.IsNullOrEmpty(text.Text))
                    {
                        await DisplayAlert("خطا!", "لطفا تمام فیلد ها را تکمیل کنید.", "باشه");
                        ActivitySpinner.IsRunning = false;
                    }
                    else
                    {
                        var user = new StringContent(JsonConvert.SerializeObject(new { sendername = name.Text, senderlastname = lastname.Text, senderemail = email.Text, text = text.Text,createdate=System.DateTime.Now }), Encoding.UTF8, "application/json");

                        var response = Consts.client.PostAsync("http://localhost:1212/api/MessageToAdmin", user).Result;
                        if (response.IsSuccessStatusCode)
                        {
                            await DisplayAlert("خطا!", "با موفقیت ارسال شد.", "باشه");
                            await Navigation.PushAsync(new UserHome());
                        }
                        else
                        {
                            await DisplayAlert("خطا!", JsonConvert.DeserializeObject<ErrorMessages>(response.Content.ReadAsStringAsync().Result).message, "باشه");
                            ActivitySpinner.IsRunning = false;
                        }
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