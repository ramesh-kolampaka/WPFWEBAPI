using ContactService.Data;
using ContactService.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;

namespace ContactService.Repositorys
{
    public class ContactsRepository : IContactRepository
    {

        private List<ContactModel> _lstContacts = new List<ContactModel>();

        private CONTACTSDATAEntities db = new CONTACTSDATAEntities();
        private int _nextId = 3;
        public ContactsRepository()
        {
           
        }



        private void GenerateContactClassData()
        {
            try
            {
                this.Add(new ContactModel()
                {
                    Id = 1,
                    FirstName = "Trem",
                    LastName = "Bridge",
                    DateofBirth = new DateTime(1985, 1, 20),
                    Emails = new List<EmailModel>() { new EmailModel() { Email = "bc@bc.com" } },
                    PhoneNumbers = new List<PhoneModel>() { new PhoneModel(){ PhoneNumber= "7987897871" }
                }
                }
                    );
                this.Add(new ContactModel()
                { Id=2,
                    FirstName = "Wiser",
                    LastName = "Blome",
                    DateofBirth = new DateTime(1989, 12, 20),
                    Emails = new List<EmailModel>() { new EmailModel() { Email = "ole@uma.com" },
                                                       new EmailModel() { Email="oe@oe.com" }},
                    PhoneNumbers = new List<PhoneModel>() { new PhoneModel() { PhoneNumber = "6789711211" },
                                                             new PhoneModel(){ PhoneNumber="6798754515" } }
                });

                this.Add(new ContactModel()
                {   Id=3,
                    FirstName = "Kameli",
                    LastName = "Gordon",
                    DateofBirth = new DateTime(1972, 8, 9),
                    Emails = new List<EmailModel>() { new EmailModel() { Email = "list@lis.com" },
                                                       new EmailModel() { Email="klk@kli.com" }},
                    PhoneNumbers = new List<PhoneModel>() { new PhoneModel() { PhoneNumber = "5555552252" },
                                                             new PhoneModel(){ PhoneNumber="9754456552" } }
                });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
       
        public bool Add(ContactModel contactModel)
        {
            bool addflag = false;
            try
            {
                CONTACT objContact = new CONTACT();
                objContact.FirstName = contactModel.FirstName;
                objContact.LastName = contactModel.LastName;

                objContact.DateofBirth = contactModel.DateofBirth;
                objContact.EmailIds = contactModel.Emails != null ? string.Join(",", contactModel.Emails.Select(x => x.Email).ToArray()) : string.Empty;
                objContact.PhoneNumbers = contactModel.PhoneNumbers != null ? string.Join(",", contactModel.PhoneNumbers.Select(x => x.PhoneNumber).ToArray()) : string.Empty;

                db.CONTACTS.Add(objContact);
                db.SaveChanges();
            }
            catch (Exception ex) 
            {
                addflag = false;
                throw ex; 
            }
            return addflag=true;
        }

        public ContactModel Get(int id) 
        {
            return _lstContacts.Find(c => c.Id == id);
        }

        public List<ContactModel> GetAll()
        {
            //return _lstContacts.AsQueryable();
            List<ContactModel> listitems = new List<ContactModel>();
            try
            {

                var listitems2 = db.CONTACTS.ToList();

                //listitems = (from cont in db.CONTACTS
                //             select new ContactModel
                //             {
                //                 Id = cont.ContactId,
                //                 FirstName = cont.FirstName,
                //                 LastName = cont.LastName,
                //                 DateofBirth = cont.DateofBirth,
                //                 Emails = (!string.IsNullOrEmpty(cont.EmailIds) ? ((from m in cont.EmailIds.Split(',').ToList() select new EmailModel() { Email = m }).ToList()) : null),
                //                 PhoneNumbers = (!string.IsNullOrEmpty(cont.PhoneNumbers) ? ((from m in cont.PhoneNumbers.Split(',').ToList() select new PhoneModel() { PhoneNumber = m }).ToList()) : null)
                //             }).ToList();

                
                foreach (var item in listitems2)
                {
                    var emails = string.IsNullOrEmpty(item.EmailIds)? null:item.EmailIds.Split(',').ToList();

                    var phoneNumbers = string.IsNullOrEmpty(item.PhoneNumbers) ? null : item.PhoneNumbers.Split(',').ToList();

                    var emailsList = (from m in emails
                                     select new EmailModel() { Email=m }).ToList();

                    var phoneNumbersList = (from m in phoneNumbers
                                            select new PhoneModel() { PhoneNumber=m }).ToList();


                    ContactModel cmodel = new ContactModel() {Id=item.ContactId, FirstName=item.FirstName,LastName=item.LastName, 
                        DateofBirth=item.DateofBirth, Emails=emailsList, PhoneNumbers=phoneNumbersList };

                    listitems.Add(cmodel);
                }
           
            }
            catch (Exception ex)
            {
                throw ex;
            } 
            
            return listitems;
        }

        public void Remove(int id)
        {
           
        }

        public bool Update(ContactModel contactModel)
        {

            var existingcontact = db.CONTACTS.Where(x => x.ContactId == contactModel.Id).FirstOrDefault();

            if (existingcontact != null)
            {
                existingcontact.DateofBirth = contactModel.DateofBirth;
                existingcontact.FirstName = contactModel.FirstName;
                existingcontact.LastName = contactModel.LastName;
                existingcontact.EmailIds = contactModel.Emails!=null? string.Join(",",contactModel.Emails.Select(x=>x.Email).ToArray()):string.Empty;
                existingcontact.PhoneNumbers = contactModel.PhoneNumbers!=null? string.Join(",", contactModel.PhoneNumbers.Select(x=>x.PhoneNumber).ToArray()):string.Empty;

                db.SaveChanges();
            }


                  return true;
        }
    }
}