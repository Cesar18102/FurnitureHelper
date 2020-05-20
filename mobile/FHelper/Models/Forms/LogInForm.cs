using System.ComponentModel.DataAnnotations;

using Newtonsoft.Json;

namespace Models.Forms
{
    public class LogInForm
    {
        [Required(ErrorMessage = "login is required")]
        public string Login { get; set; }

        [Required(ErrorMessage = "password is required")]
        public string Password { get; set; }

        public LogInForm() { }

        public LogInForm(SignUpForm signUpForm)
        {
            Login = signUpForm.Login;
            Password = signUpForm.Password;
        }
    }
}
