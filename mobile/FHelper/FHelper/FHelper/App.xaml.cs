using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using ServerAccess;

namespace FHelper
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            IServer server = DependencyService.Get<IServer>();
            MainPage = new MainPage();
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
