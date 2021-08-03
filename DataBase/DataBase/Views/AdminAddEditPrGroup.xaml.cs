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
    public partial class AdminAddEditPrGroup : ContentPage
    {
        private List<string> f = new List<string>();
        public AdminAddEditPrGroup(PrGroup prGroup=null)
        {
            InitializeComponent();
            ActivitySpinner.IsRunning = false;
            menu();
            if (prGroup!=null)
            {
                title.Text = prGroup.title;
                vahed.Text = prGroup.vahed;
                foreach (var item in prGroup.Fields)
                {
                    f.Add(item.title);
                    fieldadd(item.title);
                }
            }
            btn_field.Clicked += async (s, e) =>
            {
                string result = await DisplayPromptAsync("عنوان فیلد", "لطفا عنوان فیلد را وارد کنید");
                if (!string.IsNullOrEmpty(result))
                {
                    f.Add(result);
                    fieldadd(result);
                }
            };

            btn_laghv.Clicked += async (s, e) =>
              {
                  await Navigation.PopAsync();
              };

            btn_taiid.Clicked += async (s, e) =>
            {
                if (prGroup!=null)
                {
                    var formVariables = new List<KeyValuePair<string, string>>();
                    formVariables.Add(new KeyValuePair<string, string>("title", title.Text));
                    formVariables.Add(new KeyValuePair<string, string>("vahed", vahed.Text));
                    formVariables.Add(new KeyValuePair<string, string>("id", prGroup.id.ToString()));
                    foreach (var item in f)
                    {
                        formVariables.Add(new KeyValuePair<string, string>("field", item));
                    }
                    var formContent = new FormUrlEncodedContent(formVariables);
                    var response2 = Consts.client.PostAsync("http://localhost:1212/Admin/api/PrGroupEdit", formContent).Result;
                    if (response2.IsSuccessStatusCode)
                    {
                        await DisplayAlert("موفق", "با موفقیت ثبت شد", "مرسی");
                        await Navigation.PushAsync(new AdminPrGroup());
                    }
                    else
                    {
                        await DisplayAlert("خطا", JsonConvert.DeserializeObject<ErrorMessages>(response2.Content.ReadAsStringAsync().Result).message, "باشه");
                    }
                }
                else
                {
                    var formVariables = new List<KeyValuePair<string, string>>();
                    formVariables.Add(new KeyValuePair<string, string>("title", title.Text));
                    formVariables.Add(new KeyValuePair<string, string>("vahed", vahed.Text));
                    foreach (var item in f)
                    {
                        formVariables.Add(new KeyValuePair<string, string>("field", item));
                    }
                    var formContent = new FormUrlEncodedContent(formVariables);
                    var response2 = Consts.client.PostAsync("http://localhost:1212/Admin/api/PrGroupCreate", formContent).Result;
                    if (response2.IsSuccessStatusCode)
                    {
                        await DisplayAlert("موفق", "با موفقیت ثبت شد", "مرسی");
                        await Navigation.PushAsync(new AdminPrGroup());
                    }
                    else
                    {
                        await DisplayAlert("خطا", JsonConvert.DeserializeObject<ErrorMessages>(response2.Content.ReadAsStringAsync().Result).message, "باشه");
                    }
                }
            };
        }

        private void btn_fielddelete(object sender, EventArgs e)
        {
            var button2 = (Button)sender;
            var p = button2.Parent;
            var p2 = (StackLayout)p.Parent;
            var lable2 = (Label)p2.Children[0];
            f.Remove(lable2.Text);
            fields.Children.Clear();
            for (int i = 1; i <= f.Count; i++)
            {
                var stack = new StackLayout();
                Grid.SetRow(stack, (f.Count - i) / 4);
                Grid.SetColumn(stack, (f.Count - i) % 4);
                var lable = new Label { Text = f[i-1], TextColor = Color.Black, HorizontalOptions = LayoutOptions.CenterAndExpand, VerticalOptions = LayoutOptions.CenterAndExpand };
                stack.Children.Add(lable);
                var stack2 = new StackLayout { BackgroundColor = Color.DarkRed };
                var button = new Button { IsEnabled = true, Text = "حذف فیلد", BackgroundColor = Color.DarkGray, TextColor = Color.White };
                button.Clicked += btn_fielddelete;
                stack2.Children.Add(button);
                stack.Children.Add(stack2);
                fields.Children.Add(stack);
            }
        }

        private void fieldadd(string a)
        {
            var stack = new StackLayout();
            Grid.SetRow(stack, (f.Count - 1) / 4);
            Grid.SetColumn(stack, (f.Count - 1) % 4);
            var lable = new Label { Text = a, TextColor = Color.Black, HorizontalOptions = LayoutOptions.CenterAndExpand, VerticalOptions = LayoutOptions.CenterAndExpand };
            stack.Children.Add(lable);
            var stack2 = new StackLayout { BackgroundColor = Color.DarkRed };
            var button = new Button { IsEnabled = true, Text = "حذف فیلد", BackgroundColor = Color.DarkGray, TextColor = Color.White };
            button.Clicked += btn_fielddelete;
            stack2.Children.Add(button);
            stack.Children.Add(stack2);
            fields.Children.Add(stack);
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