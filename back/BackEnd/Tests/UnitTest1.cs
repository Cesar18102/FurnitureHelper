using System.IO;

using NUnit.Framework;

using ServicesContract.Dto;
using BackEnd.Controllers;

namespace Tests
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        private string GetRandomString()
        {
            return Path.GetRandomFileName().Replace(".", "");
        }

        [Test]
        public void Test1()
        {
            AccountController accountController = new AccountController();

            SignUpDto signUpDto = new SignUpDto()
            {
                Login = GetRandomString(),
                Password = GetRandomString(),
                FirstName = GetRandomString(),
                LastName = GetRandomString(),
                Email = GetRandomString() + "@gmail.com"
            };

            accountController.SignUp(signUpDto);
        }
    }
}