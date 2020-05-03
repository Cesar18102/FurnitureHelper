using System;

namespace Models
{
    public class AccountExtensionModel : IModel
    {
        public int Id { get; private set; }
        public string Phone { get; private set; }
        public string Address { get; private set; }
        public DateTime LastUsedDate { get; private set; }
    }
}