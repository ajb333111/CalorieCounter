﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CalorieCounter
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SignUpPage : ContentPage
    {
        public SignUpPage()
        {
            InitializeComponent();
        }

        private void SignUpButton_Clicked(object sender, EventArgs e)
        {
            // to do: verify Miami email and add user to our DB through stored procedure
            //Navigation.PushModalAsync(new MainPage());
        }
    }
}