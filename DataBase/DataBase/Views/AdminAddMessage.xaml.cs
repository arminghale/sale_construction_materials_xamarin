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
    public partial class AdminAddMessage : ContentPage
    {
        int selectuser = 0;
        public AdminAddMessage(string email="",int userid=0)
        {
            InitializeComponent();
            ActivitySpinner.IsRunning = false;
            menu();
            if (!string.IsNullOrEmpty(email)||userid!=0)
            {
                user.IsVisible = false;
                username.IsVisible = false;
                se.IsVisible = false;
            }
            else
            {
                var response = Consts.client.GetAsync("http://localhost:1212/Admin/api/UsersIndex/").Result;
                if (response.IsSuccessStatusCode)
                {
                    var list = JsonConvert.DeserializeObject<List<User>>(response.Content.ReadAsStringAsync().Result);
                    foreach (var item in list)
                    {
                        if (item.Role.title!="admin"&& item.Role.title != "anbar")
                        {
                            username.Items.Add(item.username);
                        }
                    }
                }
            }

            username.SelectedIndexChanged += async (s, e) =>
            {
                var response = Consts.client.GetAsync("http://localhost:1212/Admin/api/UsersIndex/").Result;
                if (response.IsSuccessStatusCode)
                {
                    var list = JsonConvert.DeserializeObject<List<User>>(response.Content.ReadAsStringAsync().Result);
                    foreach (var item in list)
                    {
                        if (item.username==username.SelectedItem.ToString())
                        {
                            selectuser = item.id;
                        }
                    }
                }
            };

            btn_search.Clicked += async (s, e) =>
            {
                username.Items.Clear();
                var response = Consts.client.GetAsync("http://localhost:1212/Admin/api/UsersIndex/?search="+search.Text).Result;
                if (response.IsSuccessStatusCode)
                {
                    var list = JsonConvert.DeserializeObject<List<User>>(response.Content.ReadAsStringAsync().Result);
                    foreach (var item in list)
                    {
                        if (item.Role.title != "admin" && item.Role.title != "anbar")
                        {
                            username.Items.Add(item.username);
                        }
                    }
                }
            };

            btn_send.Clicked += async (s, e) =>
            {
                ActivitySpinner.IsRunning = true;
                if (string.IsNullOrEmpty(text.Text))
                {
                    await DisplayAlert("خطا!", "لطفا تمام فیلد ها را تکمیل کنید.", "باشه");
                    ActivitySpinner.IsRunning = false;
                }
                else
                {
                    if (!string.IsNullOrEmpty(email))
                    {
                        var formVariables = new List<KeyValuePair<string, string>>();
                        formVariables.Add(new KeyValuePair<string, string>("email", email));
                        formVariables.Add(new KeyValuePair<string, string>("text", text.Text));
                        var formContent = new FormUrlEncodedContent(formVariables);
                        var response = Consts.client.PostAsync("http://localhost:1212/Admin/api/MessageSendEmail", formContent).Result;
                        if (response.IsSuccessStatusCode)
                        {
                            await DisplayAlert("موفق!", "با موفقیت ارسال شد.", "باشه");
                            await Navigation.PushAsync(new AdminRecMessage());
                        }
                        else
                        {
                            await DisplayAlert("خطا!", JsonConvert.DeserializeObject<ErrorMessages>(response.Content.ReadAsStringAsync().Result).message, "باشه");
                            ActivitySpinner.IsRunning = false;
                        }
                    }
                    else if (userid != 0)
                    {
                        var formVariables = new List<KeyValuePair<string, string>>();
                        formVariables.Add(new KeyValuePair<string, string>("adminid", Consts.User.id.ToString()));
                        formVariables.Add(new KeyValuePair<string, string>("text", text.Text));
                        formVariables.Add(new KeyValuePair<string, string>("userid", userid.ToString()));
                        var formContent = new FormUrlEncodedContent(formVariables);
                        var response = Consts.client.PostAsync("http://localhost:1212/Admin/api/MessageCreate", formContent).Result;
                        if (response.IsSuccessStatusCode)
                        {
                            await DisplayAlert("موفق!", "با موفقیت ارسال شد.", "باشه");
                            await Navigation.PushAsync(new AdminRecMessage());
                        }
                        else
                        {
                            await DisplayAlert("خطا!", JsonConvert.DeserializeObject<ErrorMessages>(response.Content.ReadAsStringAsync().Result).message, "باشه");
                            ActivitySpinner.IsRunning = false;
                        }
                    }
                    else
                    {
                        var formVariables = new List<KeyValuePair<string, string>>();
                        formVariables.Add(new KeyValuePair<string, string>("adminid", Consts.User.id.ToString()));
                        formVariables.Add(new KeyValuePair<string, string>("text", text.Text));
                        formVariables.Add(new KeyValuePair<string, string>("userid", selectuser.ToString()));
                        var formContent = new FormUrlEncodedContent(formVariables);
                        var response = Consts.client.PostAsync("http://localhost:1212/Admin/api/MessageCreate", formContent).Result;
                        if (response.IsSuccessStatusCode)
                        {
                            await DisplayAlert("موفق!", "با موفقیت ارسال شد.", "باشه");
                            await Navigation.PushAsync(new AdminRecMessage());
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