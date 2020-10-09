using DsGestionStock.Pages;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DsGestionStock
{
    public partial class App : Application
    {
        public static MainPage MasterDetail { get; internal set; }

        public App()
        {
            InitializeComponent();

            MainPage = new DsGestionStock.Pages.MainPage();
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
