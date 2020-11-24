using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ContactService.Models
{
    public class ContactModel
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public DateTime? DateofBirth { get; set; }

        public List<EmailModel> Emails { get; set; }

        public List<PhoneModel> PhoneNumbers { get; set; } 
    }
}