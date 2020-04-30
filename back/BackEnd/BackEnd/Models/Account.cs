using System.Collections.Generic;

namespace BackEnd.Models
{
    public class Account
    {
        public int Id { get; private set; }
        public string Login { get; private set; }
        public string Email { get; private set; }
        public string Password { get; private set; }

        public string FirstName { get; private set; }
        public string LastName { get; private set; }

        public IEnumerable<AccountExtension> AccountExtensions { get; private set; }
    }
}