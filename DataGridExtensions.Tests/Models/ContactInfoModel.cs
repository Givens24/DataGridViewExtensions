using System;
using System.ComponentModel;

namespace DataGridExtensions.Tests.Models
{
    public class ContactInfoModel
    {
        public Guid Id { get; set; }
        [Description("First Name")]
        public string FirstName { get; set; }
        [Description("Last Name")]
        public string LastName { get; set; }
        [Description("Phone Number")]
        public string PhoneNumber { get; set; }
        [Description("Date of Birth")]
        public DateTime DateOfBirth { get; set; }
        public int Age { get; set; }
        public string Email { get; set; }

    }
}
