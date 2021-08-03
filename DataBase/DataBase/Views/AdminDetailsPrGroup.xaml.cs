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
    public partial class AdminDetailsPrGroup : ContentPage
    {
        private List<string> f = new List<string>();
        public AdminDetailsPrGroup(PrGroup prGroup)
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
        }

        private void fieldadd(string a)
        {
            var stack = new StackLayout();
            Grid.SetRow(stack, (f.Count - 1) / 4);
            Grid.SetColumn(stack, (f.Count - 1) % 4);
            var lable = new Label { Text = a, TextColor = Color.Black, HorizontalOptions = LayoutOptions.CenterAndExpand, VerticalOptions = LayoutOptions.CenterAndExpand,FontAttributes=FontAttributes.Bold };
            stack.Children.Add(lable);
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