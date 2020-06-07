using System.Windows.Input;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FHelper.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ValidateView : ContentView
    {
        public static readonly BindableProperty ValideeProperty = BindableProperty.Create(
            "Validee", typeof(object), typeof(object)
        );

        public object Validee
        {
            get => GetValue(ValideeProperty);
            set => SetValue(ValideeProperty, value);
        }

        private Command validateCommand;
        public ICommand ValidateCommand => validateCommand;

        public ICollection<ValidationResult> Errors = new List<ValidationResult>();

        private void Validate()
        {
            if (Validee == null)
                return;

            ValidationContext context = new ValidationContext(Validee);
            Validator.TryValidateObject(Validee, context, Errors, true);
        }

        public ValidateView()
        {
            validateCommand = new Command(Validate);
            InitializeComponent();
        }
    }
}