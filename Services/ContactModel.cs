using System;
using System.Collections.Generic;
using System.Linq;
using WPFContacts.ViewModel;

namespace WPFContacts.Services
{
    public class ContactModel
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public DateTime DateofBirth { get; set; }

        public List<EmailModel> Emails { get; set; } 

        public List<PhoneModel> PhoneNumbers { get; set; }

        public ContactModel()
        {
            Emails = new List<EmailModel>();
            PhoneNumbers = new List<PhoneModel>();
        }
        
        public void UpdateWith(CurrentContactModel ccm)
        {
            Id = ccm.Id;
            FirstName = ccm.FirstName;
            LastName = ccm.LastName;
            DateofBirth = ccm.DateofBirth;
            Emails = ccm.Emails.ToList();
            PhoneNumbers = ccm.PhoneNumbers.ToList();
        }
    }

    public class EmailModel : ViewModelBase
    {
        private int id;
        private string email;

        public string Email { get => email; set { email = value;OnPropertyChanged(); } }

        public int Id { get => id; set { id = value; OnPropertyChanged(); } }
    }

    public class PhoneModel : ViewModelBase
    {
        private string phoneNumber;
        private int id;

        public int Id { get => id; set { id = value; OnPropertyChanged(); } }
        public string PhoneNumber { get => phoneNumber; set { phoneNumber = value; OnPropertyChanged(); } }
    }
}