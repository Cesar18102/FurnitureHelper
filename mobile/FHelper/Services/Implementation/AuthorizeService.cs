using System;
using System.Threading.Tasks;

using Autofac;

using Models.Dto;
using Models.Forms;

using ServerAccess;

using Services.Declaration;

namespace Services.Implementation
{
    internal class AuthorizeService : IAuthorizeService
    {
        private const string SIGN_UP_ENDPOINT = "Account/SignUp";
        private const string LOG_IN_ENDPOINT = "Account/LogIn";

        private static readonly IServer Server = ServerHolder.Dependencies.Resolve<IServer>();
        private static readonly HashingService Hasher = ServicesHolder.Dependencies.Resolve<HashingService>();

        public static SessionDto Session { get; private set; }

        public async Task<SessionDto> SignUp(SignUpForm signUpForm)
        {
            await Server.SendPost<SignUpForm, object>(
                ServerHolder.SERVER_URL + SIGN_UP_ENDPOINT,
                signUpForm
            );

            return await LogIn(new LogInForm(signUpForm));
        }

        public async Task<SessionDto> LogIn(LogInForm logInForm)
        {
            string salt = Guid.NewGuid().ToString();
            string passwordSalted = Hasher.GetHash(Hasher.GetHash(logInForm.Password) + salt);
            LogInDto dto = new LogInDto(logInForm.Login, passwordSalted, salt);

            Session = await Server.SendPost<LogInDto, SessionDto>(
                ServerHolder.SERVER_URL + LOG_IN_ENDPOINT, dto
            );

            return Session;
        }
    }
}
