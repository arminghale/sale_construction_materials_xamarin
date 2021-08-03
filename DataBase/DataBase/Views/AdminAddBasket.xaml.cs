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
    public partial class AdminAddBasket : ContentPage
    {
        int selectuser = 0;
        int selectaddress = 0;
        List<int> ids = new List<int>();
        List<int> counts= new List<int>();
        public AdminAddBasket()
        {
            InitializeComponent();
            ActivitySpinner.IsRunning = false;
            menu();

            var response3 = Consts.client.GetAsync("http://localhost:1212/Admin/api/UsersIndex/").Result;
            if (response3.IsSuccessStatusCode)
            {
                var list = JsonConvert.DeserializeObject<List<User>>(response3.Content.ReadAsStringAsync().Result);
                foreach (var item in list)
                {
                    if (item.Role.title != "admin" && item.Role.title != "anbar")
                    {
                        username.Items.Add(item.username);
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
                        if (item.username == username.SelectedItem.ToString())
                        {
                            selectuser = item.id;
                            var response2 = Consts.client.GetAsync("http://localhost:1212/Admin/api/UsersAddress/" + selectuser).Result;
                            if (response2.IsSuccessStatusCode)
                            {
                                address.Items.Clear();
                                var list2 = JsonConvert.DeserializeObject<List<Address>>(response2.Content.ReadAsStringAsync().Result);
                                foreach (var item2 in list2)
                                {
                                    var te = item2.id + "-" + item2.ostan + "-" + item2.shahr + "-" + item2.text + "--" + item2.codeposti;
                                    address.Items.Add(te);
                                }
                            }
                        }
                    }
                }
            };

            address.SelectedIndexChanged += (s, e) =>
            {
                selectaddress = int.Parse(address.SelectedItem.ToString().Split('-')[0]);
            };

            btn_search.Clicked += async (s, e) =>
            {
                username.Items.Clear();
                var response = Consts.client.GetAsync("http://localhost:1212/Admin/api/UsersIndex/?search=" + search.Text).Result;
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
                var formVariables = new List<KeyValuePair<string, string>>();
                formVariables.Add(new KeyValuePair<string, string>("addressid", selectaddress.ToString()));
                formVariables.Add(new KeyValuePair<string, string>("userid", selectuser.ToString()));
                int total = 0;
                for (int i = 0; i < ids.Count; i++)
                {
                    var response4 = Consts.client.GetAsync("http://localhost:1212/Admin/api/ProductBaTakhfif/" + ids[i]).Result;
                    if (response4.IsSuccessStatusCode)
                    {
                      total+= (int.Parse(JsonConvert.DeserializeObject<ErrorMessages>(response4.Content.ReadAsStringAsync().Result).message)*counts[i]);
                    }
                }
                formVariables.Add(new KeyValuePair<string, string>("total", total.ToString()));
                foreach (var item in ids)
                {
                    formVariables.Add(new KeyValuePair<string, string>("ids", item.ToString()));
                }
                foreach (var item in counts)
                {
                    formVariables.Add(new KeyValuePair<string, string>("counts", item.ToString()));
                }
                var formContent = new FormUrlEncodedContent(formVariables);
                var response = Consts.client.PostAsync("http://localhost:1212/Admin/api/BasketCreate", formContent).Result;
                if (response.IsSuccessStatusCode)
                {
                    await DisplayAlert("موفق!", "با موفقیت ارسال شد.", "باشه");
                    await Navigation.PushAsync(new AdminBasket());
                }
                else
                {
                    await DisplayAlert("خطا!", JsonConvert.DeserializeObject<ErrorMessages>(response.Content.ReadAsStringAsync().Result).message, "باشه");
                    ActivitySpinner.IsRunning = false;
                }
            };
        }

        public async void ListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            Product product = e.Item as Product;
            string result = "";
            do
            {
                result = await DisplayPromptAsync(product.count + "موجودی:", "لطفا  تعداد را وارد کنید");
                if (int.Parse(result) > product.count)
                {
                  await  DisplayAlert("خطا!", "تعداد وارد شده بیشتر از موجودی است", "باشه");
                }
            } while (int.Parse(result) > product.count);
            ids.Add(product.id);
            counts.Add(int.Parse(result));
            var stack = new StackLayout { ClassId = (ids.Count - 1).ToString() };
            Grid.SetRow(stack, ids.Count / 4);
            Grid.SetColumn(stack, (ids.Count % 4)-1);
            var lable = new Image { Source = product.imagesource, HorizontalOptions = LayoutOptions.CenterAndExpand, VerticalOptions = LayoutOptions.CenterAndExpand };
            stack.Children.Add(lable);
            var stack2 = new StackLayout { BackgroundColor = Color.DarkRed };
            var button = new Button { IsEnabled = true, Text = "حذف کالا", BackgroundColor = Color.DarkGray, TextColor = Color.White };
            var lable2 = new Label { Text = (product.pricewithtakhfifuse * int.Parse(result)) + "تومان", TextColor = Color.Black, HorizontalOptions = LayoutOptions.CenterAndExpand, VerticalOptions = LayoutOptions.CenterAndExpand };
            button.Clicked += btn_prosdelete;
            stack2.Children.Add(button);
            stack.Children.Add(stack2);
            stack.Children.Add(lable2);
            pros.Children.Add(stack);
        }
        private void btn_prosdelete(object sender, EventArgs e)
        {
            var button2 = (Button)sender;
            var p = button2.Parent;
            var p2 = (StackLayout)p.Parent;
            var image = (Image)p2.Children[0];
            ids.RemoveAt(int.Parse(p2.ClassId));
            counts.RemoveAt(int.Parse(p2.ClassId));
            pros.Children.Clear();
            for (int i = 1; i <= ids.Count; i++)
            {
                var response = Consts.client.GetAsync("http://localhost:1212/Admin/api/ProductDetails/" + ids[i-1]).Result;
                if (response.IsSuccessStatusCode)
                {
                    var list = JsonConvert.DeserializeObject<Product>(response.Content.ReadAsStringAsync().Result);
                    var stack = new StackLayout { ClassId = (i - 1).ToString() };
                    Grid.SetRow(stack, (i-1) / 4);
                    Grid.SetColumn(stack, (i - 1) % 4);
                    var lable = new Image { Source = list.imagesource, HorizontalOptions = LayoutOptions.CenterAndExpand, VerticalOptions = LayoutOptions.CenterAndExpand };
                    stack.Children.Add(lable);
                    var stack2 = new StackLayout { BackgroundColor = Color.DarkRed };
                    var button = new Button { IsEnabled = true, Text = "حذف کالا", BackgroundColor = Color.DarkGray, TextColor = Color.White };
                    var lable2 = new Label { Text = (list.pricewithtakhfifuse*counts[i-1])+"تومان", TextColor = Color.Black, HorizontalOptions = LayoutOptions.CenterAndExpand, VerticalOptions = LayoutOptions.CenterAndExpand };
                    button.Clicked += btn_prosdelete;
                    stack2.Children.Add(button);
                    stack.Children.Add(stack2);
                    stack.Children.Add(lable2);
                    pros.Children.Add(stack);
                }
                
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