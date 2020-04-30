using System;

namespace BackEnd.Models
{
    public class AccountExtension
    {
        public int Id { get; private set; }
        public string Phone { get; private set; }
        public string Address { get; private set; }
        public DateTime LastUsedDate { get; private set; }
    }
}