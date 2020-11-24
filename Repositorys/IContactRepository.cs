using ContactService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactService.Repositorys
{
    public interface IContactRepository
    {
        bool Add(ContactModel contactModel);
        ContactModel Get(int id);
        List<ContactModel> GetAll();
        void Remove(int id);
        bool Update(ContactModel contactModel);
    }
}
