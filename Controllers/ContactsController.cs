using ContactService.Models;
using ContactService.Repositorys;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ContactService.Controllers
{   
    
    [RoutePrefix("api/Contacts")]
    public class ContactsController : ApiController
    {
        private readonly IContactRepository _contactRepository;

        public ContactsController(IContactRepository contactRepository)
        {
            _contactRepository = contactRepository;
        }
        // GET api/values
        
        [Route("GetAllContacts")]
        public IEnumerable<ContactModel> GetAllContacts()
        {
           return _contactRepository.GetAll();
        }


       

     //   [HttpGet, Route("{id:int}")]

        [Route("GetContact")]
        // GET api/values/5
        public ContactModel GetContact(int id)
        {
            ContactModel contactitem = _contactRepository.Get(id);
            if (contactitem == null)
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.NotFound));
            }
            return contactitem;
        }


        
        [Route("GetContactByName")]
        public IEnumerable<ContactModel> GetContactByName(string name)
        {
            return _contactRepository.GetAll().Where(
                p => string.Equals(p.FirstName, name, StringComparison.OrdinalIgnoreCase));
                
        }

        [HttpPost]
        [Route("CreateContact")]
        public HttpResponseMessage CreateContact(ContactModel item)
        {
            HttpResponseMessage response=null;

            if (_contactRepository.Add(item))
            {
                 response = Request.CreateResponse<ContactModel>(HttpStatusCode.Created, item);
                //string uri = Url.Link("DefaultApi", new { id = item.Id });
                //response.Headers.Location = new Uri(uri);
               
            }
            return response;
        }

        [HttpPut]
        [Route("UpdateContact")]
        public void UpdateContact(ContactModel contactModel)
        {
            //contactModel.Id = id;
            if(contactModel==null) throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.ExpectationFailed));

            if (!_contactRepository.Update(contactModel))
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.NotFound));
            }
        }

        [HttpDelete]
        [Route("DeleteContact")]
        public HttpResponseMessage DeleteContact(int id)
        {
            _contactRepository.Remove(id);
            return new HttpResponseMessage(HttpStatusCode.NoContent);
        }

    }

 
}
