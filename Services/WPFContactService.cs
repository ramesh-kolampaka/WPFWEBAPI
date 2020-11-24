using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Windows;

namespace WPFContacts.Services
{
    public class WPFContactService : IWPFContactService, IDisposable
    {
        private HttpClient client;
        private ObservableCollection<ContactModel> _contactsList;

        public ObservableCollection<ContactModel> ContactsList { get => _contactsList; set => _contactsList = value; }

        public WPFContactService()
        {
            
        }
        public bool CreateContact(ContactModel contactModel)
        {
            bool successflag = false;
            try
            {
                GetInstanceofHttpClient();

            HttpResponseMessage response = client.PostAsJsonAsync("Api/Contacts/CreateContact", contactModel).Result;

            if (response.IsSuccessStatusCode)
            {
                successflag = true;
            }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return successflag;
        }
        private void GetInstanceofHttpClient()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri("http://localhost:52007/");
            
            // Add an Accept header for JSON format.
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));

        }


        public ObservableCollection<ContactModel> GetAllContacts()
        {
            try
            {
                GetInstanceofHttpClient();

                HttpResponseMessage response = client.GetAsync("Api/Contacts/GetAllContacts").Result;

                if (response.IsSuccessStatusCode)
                {
                    //var contacts =  response.Content.ReadAsAsync<IEnumerable<ContactModel>>().Result;

                    // response.EnsureSuccessStatusCode(); // throws if not 200-299


                    var stringData = response.Content.ReadAsStringAsync().Result;

                    //string stringData = response.Content.
                    //ReadAsStringAsync().Result;
                    var options = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    };
                    var contacts = System.Text.Json.JsonSerializer.Deserialize<IEnumerable<ContactModel>>
                        (stringData, options);

                    ContactsList = new ObservableCollection<ContactModel>(contacts);

                }
                else
                {
                    MessageBox.Show("Error Code" + response.StatusCode + " : Message - " + response.ReasonPhrase);
                }

            }

            catch (Exception ex)
            {
                throw ex;
            }
            return ContactsList;
        }

        public ContactModel GetContactById(int id)
        {
            throw new NotImplementedException();
        }

        public bool RemoveContact(int id)
        {
            bool deleteStatus = false;
            try
            {
                GetInstanceofHttpClient();

                HttpResponseMessage response = client.DeleteAsync("Api/Contacts/Contact/id="+id).Result;
                
                if (response.IsSuccessStatusCode)
                {
                    deleteStatus = true;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            
            return deleteStatus;
        }

        public bool UpdateContact(ContactModel contactModel)
        {
            bool updateStatus = false;
            try
            {
                GetInstanceofHttpClient();

                HttpResponseMessage response = client.PutAsJsonAsync("Api/Contacts/UpdateContact", contactModel).Result;

                if (response.IsSuccessStatusCode)
                {
                    updateStatus = true;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return updateStatus;
        }


        public void Dispose()
        {
            
        }
    }
}
