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
    public partial class Register2 : ContentPage
    {
        public Register2(int id)
        {
            InitializeComponent();
            ActivitySpinner.IsRunning = false;
            menu();
            List<State> state = null;
            List<City> City = null;
            int selectostan = 0;
            int selectshahr = 0;
            var response2 = Consts.client.GetAsync("http://localhost:1212/api/State/").Result;
            if (response2.IsSuccessStatusCode)
            {
                state = JsonConvert.DeserializeObject<List<State>>(response2.Content.ReadAsStringAsync().Result);
                foreach (var item in state)
                {
                    ostan.Items.Add(item.name);
                }
            }
            var response3 = Consts.client.GetAsync("http://localhost:1212/api/City/").Result;
            if (response3.IsSuccessStatusCode)
            {
                City = JsonConvert.DeserializeObject<List<City>>(response3.Content.ReadAsStringAsync().Result);
            }

            ostan.SelectedIndexChanged += (s, e) =>
            {
                foreach (var item in state)
                {
                    if (ostan.SelectedItem.ToString() == item.name)
                    {
                        selectostan = item.id;
                        shahr.Items.Clear();
                        foreach (var item2 in City)
                        {
                            if (item2.province_id == item.id)
                            {
                                shahr.Items.Add(item2.name);
                            }
                        }
                    }
                }
            };

            btn_register.Clicked += async (s, e) =>
            {
                if (string.IsNullOrEmpty(name.Text) || string.IsNullOrEmpty(lastname.Text) || string.IsNullOrEmpty(phonenumber.Text)
                || string.IsNullOrEmpty(text.Text) || string.IsNullOrEmpty(codeposti.Text))
                {
                    await DisplayAlert("خطا!", "لطفا تمام فیلد ها را تکمیل کنید.", "باشه");
                    ActivitySpinner.IsRunning = false;
                }
                else
                {
                    foreach (var item in City)
                    {
                        if (item.name == shahr.SelectedItem.ToString())
                        {
                            selectshahr = item.id;
                        }
                    }
                    ActivitySpinner.IsRunning = true;
                    var user = new StringContent(JsonConvert.SerializeObject(new
                    {
                        name = name.Text,
                        lastname = lastname.Text,
                        phonenumber = phonenumber.Text,
                        ostan = selectostan
                    ,
                        shahr = selectshahr,
                        codeposti = codeposti.Text,
                        text = text.Text,
                        id=id
                    }), Encoding.UTF8, "application/json");

                    var response = Consts.client.PostAsync("http://localhost:1212/api/register2", user).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        Consts.isUserLoged = true;
                        var res2 = JsonConvert.DeserializeObject<User>(response.Content.ReadAsStringAsync().Result);
                        Consts.User = res2;
                        await Navigation.PushAsync(new UserHome());
                    }
                    else
                    {
                        await DisplayAlert("خطا!", JsonConvert.DeserializeObject<ErrorMessages>(response.Content.ReadAsStringAsync().Result).message, "باشه");
                        ActivitySpinner.IsRunning = false;
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