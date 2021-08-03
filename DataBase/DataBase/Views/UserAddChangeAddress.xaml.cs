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
    public partial class UserAddChangeAddress : ContentPage
    {
        public UserAddChangeAddress(Address address=null)
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
                    if (address!=null&&item.name==address.ostan)
                    {
                        ostan.SelectedItem = item.name;
                        selectostan = item.id;
                    }
                }
            }
            var response3 = Consts.client.GetAsync("http://localhost:1212/api/City/").Result;
            if (response3.IsSuccessStatusCode)
            {
                City = JsonConvert.DeserializeObject<List<City>>(response3.Content.ReadAsStringAsync().Result);
                if (address != null)
                {
                    foreach (var item in City.Where(w=>w.province_id==selectostan))
                    {
                        shahr.Items.Add(item.name);
                        if (item.name == address.shahr)
                        {
                            shahr.SelectedItem = item.name;
                            selectshahr = item.id;
                        }
                    }
                }
                
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

            if (address!=null)
            {
                if (address.girandelastname==Consts.User.Person.lastname&&
                    address.girandename == Consts.User.Person.name&& address.girandephonenumber == Consts.User.Person.phonenumber)
                {
                    shakhs.Items.Add("خودم");
                    shakhs.Items.Add("شخص دیگر");
                    shakhs.SelectedIndex = 0;
                    lg1.IsVisible = false;
                    lg2.IsVisible = false;
                    lg3.IsVisible = false;
                    girandelastname.IsVisible = false;
                    girandename.IsVisible = false;
                    girandephonenumber.IsVisible = false;
                }
                else
                {
                    shakhs.Items.Add("خودم");
                    shakhs.Items.Add("شخص دیگر");
                    shakhs.SelectedIndex = 1;
                }
                girandelastname.Text = address.girandelastname;
                girandename.Text = address.girandename;
                girandephonenumber.Text = address.girandephonenumber;
                codeposti.Text = address.codeposti;
                text.Text = address.text;
            }
            else
            {
                shakhs.Items.Add("خودم");
                shakhs.Items.Add("شخص دیگر");
                shakhs.SelectedIndex = 1;
            }

            shakhs.SelectedIndexChanged += (s, e) =>
            {
                if (shakhs.SelectedIndex==0)
                {
                    lg1.IsVisible = false;
                    lg2.IsVisible = false;
                    lg3.IsVisible = false;
                    girandelastname.IsVisible = false;
                    girandename.IsVisible = false;
                    girandephonenumber.IsVisible = false;
                    girandelastname.Text = Consts.User.Person.lastname;
                    girandename.Text = Consts.User.Person.name;
                    girandephonenumber.Text = Consts.User.Person.phonenumber;
                }
                else
                {
                    lg1.IsVisible = true;
                    lg2.IsVisible = true;
                    lg3.IsVisible = true;
                    girandelastname.IsVisible = true;
                    girandename.IsVisible = true;
                    girandephonenumber.IsVisible = true;
                    girandelastname.Text = "";
                    girandename.Text = "";
                    girandephonenumber.Text = "";
                }
            };

            btn_laghv.Clicked += async (s, e) =>
              {
                  await Navigation.PopAsync();
              };

            btn_taiid.Clicked += async (s, e) =>
            {
                if (string.IsNullOrEmpty(girandename.Text) || string.IsNullOrEmpty(girandelastname.Text) || string.IsNullOrEmpty(girandephonenumber.Text)
                || string.IsNullOrEmpty(text.Text) || string.IsNullOrEmpty(codeposti.Text))
                {
                    await DisplayAlert("خطا!", "لطفا تمام فیلد ها را تکمیل کنید.", "باشه");
                    ActivitySpinner.IsRunning = false;
                }
                else
                {
                    ActivitySpinner.IsRunning = true;
                    foreach (var item in City)
                    {
                        if (item.name == shahr.SelectedItem.ToString())
                        {
                            selectshahr = item.id;
                        }
                    }

                    if (address==null)
                    {
                        var formVariables = new List<KeyValuePair<string, string>>();
                        formVariables.Add(new KeyValuePair<string, string>("personid", Consts.User.id.ToString()));
                        formVariables.Add(new KeyValuePair<string, string>("ostan", selectostan.ToString()));
                        formVariables.Add(new KeyValuePair<string, string>("shahr", selectshahr.ToString()));
                        formVariables.Add(new KeyValuePair<string, string>("girande", shakhs.SelectedIndex.ToString()));
                        formVariables.Add(new KeyValuePair<string, string>("codeposti", codeposti.Text));
                        formVariables.Add(new KeyValuePair<string, string>("text", text.Text));
                        formVariables.Add(new KeyValuePair<string, string>("girandename", girandename.Text));
                        formVariables.Add(new KeyValuePair<string, string>("girandelastname", girandelastname.Text));
                        formVariables.Add(new KeyValuePair<string, string>("girandephonenumber", girandephonenumber.Text));
                        var formContent = new FormUrlEncodedContent(formVariables);
                        var response4 = Consts.client.PostAsync("http://localhost:1212/User/api/AddAddress/", formContent).Result;
                        if (response4.IsSuccessStatusCode)
                        {
                            await DisplayAlert("موفق!", "با موفقیت انجام شد", "باشه");
                            ActivitySpinner.IsRunning = false;

                            await Navigation.PushAsync(new UserHome());
                        }
                        else
                        {
                            await DisplayAlert("خطا!", JsonConvert.DeserializeObject<ErrorMessages>(response4.Content.ReadAsStringAsync().Result).message, "باشه");
                            ActivitySpinner.IsRunning = false;
                        }
                    }
                    else
                    {
                        var formVariables = new List<KeyValuePair<string, string>>();
                        formVariables.Add(new KeyValuePair<string, string>("personid", Consts.User.id.ToString()));
                        formVariables.Add(new KeyValuePair<string, string>("id", address.id.ToString()));
                        formVariables.Add(new KeyValuePair<string, string>("ostan", selectostan.ToString()));
                        formVariables.Add(new KeyValuePair<string, string>("shahr", selectshahr.ToString()));
                        formVariables.Add(new KeyValuePair<string, string>("girande", shakhs.SelectedIndex.ToString()));
                        formVariables.Add(new KeyValuePair<string, string>("codeposti", codeposti.Text));
                        formVariables.Add(new KeyValuePair<string, string>("text", text.Text));
                        formVariables.Add(new KeyValuePair<string, string>("girandename", girandename.Text));
                        formVariables.Add(new KeyValuePair<string, string>("girandelastname", girandelastname.Text));
                        formVariables.Add(new KeyValuePair<string, string>("girandephonenumber", girandephonenumber.Text));
                        var formContent = new FormUrlEncodedContent(formVariables);
                        var response4 = Consts.client.PostAsync("http://localhost:1212/User/api/ChangeAddress/", formContent).Result;
                        if (response4.IsSuccessStatusCode)
                        {
                            await DisplayAlert("موفق!", "با موفقیت انجام شد", "باشه");
                            ActivitySpinner.IsRunning = false;

                            await Navigation.PushAsync(new UserHome());
                        }
                        else
                        {
                            await DisplayAlert("خطا!", JsonConvert.DeserializeObject<ErrorMessages>(response4.Content.ReadAsStringAsync().Result).message, "باشه");
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