using SimpleRestApi.Views;
using System;

using Xamarin.Forms;

namespace SimpleRestApi
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            if (Device.RuntimePlatform == Device.iOS)
                MainPage = new ExchangeRate();
            else
                MainPage = new NavigationPage(new ExchangeRate());
        }
    }
}