using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFContacts.Services
{
    public interface IWPFContactService
    {
        bool CreateContact(ContactModel contactModel);
        ContactModel GetContactById(int id);
        ObservableCollection<ContactModel> GetAllContacts();
        bool RemoveContact(int id);
        bool UpdateContact(ContactModel contactModel);
    }
}
