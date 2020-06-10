using System;
using System.Linq;

using Autofac;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using Models.Forms;
using Models.Exceptions;

using Services;
using Services.Declaration;

using FHelper.Resources;

namespace FHelper.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AuthorizePage : ContentPage
    {
        public static readonly BindableProperty SignUpFormProperty = BindableProperty.Create(
            "SignUpForm", typeof(SignUpForm), typeof(SignUpForm), new SignUpForm()
        );

        public SignUpForm SignUpForm
        {
            get => (SignUpForm)GetValue(SignUpFormProperty);
            set => SetValue(SignUpFormProperty, value);
        }

        public static readonly BindableProperty LogInFormProperty = BindableProperty.Create(
            "LogInForm", typeof(LogInForm), typeof(LogInForm), new LogInForm()
        );

        public LogInForm LogInForm
        {
            get => (LogInForm)GetValue(LogInFormProperty);
            set => SetValue(LogInFormProperty, value);
        }

        public static readonly BindableProperty IsRegistringProperty = BindableProperty.Create(
            "IsRegistring", typeof(bool), typeof(bool), false
        );

        public bool IsRegistring
        {
            get => (bool)GetValue(IsRegistringProperty);
            set => SetValue(IsRegistringProperty, value);
        }

        public static readonly BindableProperty IsLoginingProperty = BindableProperty.Create(
            "IsLogining", typeof(bool), typeof(bool), false
        );

        public bool IsLogining
        {
            get => (bool)GetValue(IsLoginingProperty);
            set => SetValue(IsLoginingProperty, value);
        }

        private static readonly IAuthorizeService AuthorizeService = ServicesHolder.Dependencies.Resolve<IAuthorizeService>();

        public AuthorizePage()
        {
            LogInForm.Login = "cesar";
            LogInForm.Password = "12345678";

            InitializeComponent();
        }

        private async void SignUpButton_Clicked(object sender, EventArgs e)
        {
            if (SignUpValidator.Errors.Count != 0)
            {
                await DisplayAlert(Signs.SignUpError, SignUpValidator.Errors.First().ErrorMessage, "OK");
                return;
            }

            if (Password.Text != PasswordConfirm.Text)
            {
                await DisplayAlert(Signs.SignUpError, Signs.PasswordMismatchError, "OK");
                return;
            }

            IsBusy = true;

            try
            {
                await AuthorizeService.SignUp(SignUpForm);
                App.Current.MainPage = new FurnitureHelperMenu();
            }
            catch (CustomException ex) { await DisplayAlert(Signs.SignUpError, ex.Message, "OK"); }

            IsBusy = false;
        }

        private async void LogInButton_Clicked(object sender, EventArgs e)
        {
            if (LogInValidator.Errors.Count != 0)
            {
                await DisplayAlert(Signs.LogInError, LogInValidator.Errors.First().ErrorMessage, "OK");
                return;
            }

            IsBusy = true;

            try
            {
                await AuthorizeService.LogIn(LogInForm);
                App.Current.MainPage = new FurnitureHelperMenu();
            }
            catch (CustomException ex) { await DisplayAlert(Signs.LogInError, ex.Message, "OK"); }

            IsBusy = false;
        }

        private void LogInPrepareButton_Clicked(object sender, EventArgs e) => IsLogining = true;
        private void SignUpPrepareButton_Clicked(object sender, EventArgs e) => IsRegistring = true;

        protected override bool OnBackButtonPressed()
        {
            if (!IsRegistring && !IsLogining && !IsBusy)
                return false;

            IsLogining = false;
            IsRegistring = false;

            return true;
        }
    }
}