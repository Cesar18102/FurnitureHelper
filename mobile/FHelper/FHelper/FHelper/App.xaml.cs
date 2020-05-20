using Xamarin.Forms;

using FHelper.Pages;

namespace FHelper
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            MainPage = new AuthorizePage();
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
