﻿using Newtonsoft.Json;
using RestSharp;
using RestSharp.Authenticators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CalorieCounter
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class WelcomePage : ContentPage
    {

        RestService _restService;

        public WelcomePage()
        {
            InitializeComponent();
            _restService = new RestService();
        }

        private void LoginButton_Clicked(object sender, EventArgs e)
        {
            Login(email.Text, password.Text);
        }

        private void SignUpButton_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new SignUpPage());
        }

        private void AboutButton_Clicked(object sender, EventArgs e)
        {

        }

        

        public async void Login(string username, string password)
        {

            //var client = new RestClient("https://accounts.google.com/o/oauth2/v2/auth");
            //var request = new RestRequest("oauth/ro", Method.GET);

            //request.AddParameter("scope", "openid%20email%20profile");
            //request.AddParameter("response_type", "code");
            //request.AddParameter("redirect_uri", "com.googleusercontent.apps.1041253101002-dhan7880g5t577r7d6lc8cfcsvqfqqhf:/oauth2redirect/code");
            //request.AddParameter("client_id", "1041253101002-dhan7880g5t577r7d6lc8cfcsvqfqqhf.apps.googleusercontent.com");
            //request.AddParameter("hd", "miamioh.edu");

            //try
            //{
            //    IRestResponse response = client.Execute(request);
            //    var status = response.StatusCode;
            //    string code = response.Content;
            //    string code1 = JsonConvert.DeserializeObject<string>(response.Content);
            //}
            //catch (JsonReaderException e)
            //{
            //    Console.WriteLine(e.Message);
            //}



            string requestUri = "https://accounts.google.com/o/oauth2/v2/auth?";
            requestUri += $"scope=openid%20email%20profile";
            requestUri += $"&response_type=code";
            requestUri += "&redirect_uri=com.googleusercontent.apps.1041253101002-dhan7880g5t577r7d6lc8cfcsvqfqqhf:/oauth2redirect/code";
            requestUri += "&client_id=1041253101002-dhan7880g5t577r7d6lc8cfcsvqfqqhf.apps.googleusercontent.com";
            requestUri += "&hd=miamioh.edu";
            requestUri += "&prompt=select_account";
            requestUri += $"&login_hint={email.Text}";
            requestUri += "nonce=fdsafsadfdsagds";
            //await Browser.OpenAsync(requestUri);
            string c = await _restService.AuthenticateUser(requestUri);


        }

    }
}