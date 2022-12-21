using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Portugal_2023
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            MainPage = new PortugalTab() { BarBackgroundColor = Color.Red,};
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
