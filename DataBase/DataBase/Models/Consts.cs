using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using Xamarin.Essentials;

namespace DataBase
{
   public class Consts
    {
        public static bool isUserLoged = false;
        public static string Username { get; set; }
        public static string Password { get; set; }
        public static string Token { get; set; }
        public static User User { get; set; }

        public static HttpClient client = new HttpClient();
        public static HttpClientHandler handler = new HttpClientHandler();

        public static string checkInfo()
        {
            if (!Consts.Username.Equals("") && !Consts.Password.Equals(""))
            {
                var user = new StringContent(JsonConvert.SerializeObject(new { username = Consts.Username, password = Consts.Password }), Encoding.UTF8, "application/json");
                
                var response = client.PostAsync("http://localhost:1212/api/Login", user).Result;
                
                if (response.IsSuccessStatusCode)
                {
                    var res = JsonConvert.DeserializeObject<Token>(response.Content.ReadAsStringAsync().Result);
                    Consts.Token = res.cookie;

                    client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", Token);

                    //Uri url = new Uri("http://localhost:1212/");

                    //handler.CookieContainer = new CookieContainer();
                    //handler.CookieContainer.Add(url, new Cookie("cookie", Token));
                    //client = new HttpClient(handler);

                    var formVariables = new List<KeyValuePair<string, string>>();
                    formVariables.Add(new KeyValuePair<string, string>("id", res.id.ToString()));
                    var formContent = new FormUrlEncodedContent(formVariables);
                    var response2 = client.PostAsync("http://localhost:1212/api/Profile", formContent).Result;
                    if (response2.IsSuccessStatusCode)
                    {
                        var res2 = JsonConvert.DeserializeObject<User>(response2.Content.ReadAsStringAsync().Result);
                        User = res2;

                        Preferences.Set("username", Consts.Username);
                        Preferences.Set("password", Consts.Password);
                        Preferences.Set("user", JsonConvert.SerializeObject(res2));
                        Preferences.Set("token", Consts.Token);

                        return "ok";
                    }

                    return JsonConvert.DeserializeObject<ErrorMessages>(response.Content.ReadAsStringAsync().Result).message;
                }
                else
                {
                    return JsonConvert.DeserializeObject<ErrorMessages>(response.Content.ReadAsStringAsync().Result).message;
                }

            }
            else return "لطفا نام کاربری و رمز عبور را وارد کنید.";
        }


        public static bool tokencheck()
        {
            client = new HttpClient(handler);
            try
            {
                if (Token != null)
                {
                    client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", Token);
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception)
            {

                return false;
            }
        }
    }
}
