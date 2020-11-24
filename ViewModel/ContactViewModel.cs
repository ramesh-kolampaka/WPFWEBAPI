using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WPFContacts.Services;
using WPFContacts.Commands;
using System.Windows;
using System.Text.RegularExpressions;

namespace WPFContacts.ViewModel
{
    public class ContactViewModel : ViewModelBase, IDisposable
    {
        private readonly IWPFContactService _service;
        private ObservableCollection<ContactModel> _contactCollection = null;
        
        private ContactModel _selectedContact=null;
        private CurrentContactModel _currentContact = null;

        private const string CreateSuccessMsg = "Record Created Successfully.";
        private const string FailRecordMsg = "Fail to Create Record.";
        private const string UpdateSuccessMsg = "Record Updated Successfully.";
        private const string UpdateFailMsg = "Unable to  Update record, please try again.";

        private string _addEmail;

        public string AddEmail
        {
            get { return _addEmail; }
            set { _addEmail = value;
                OnPropertyChanged();
            }
        }

        private string _addPhoneNumber;

        public string AddPhoneNumber
        {
            get { return _addPhoneNumber; }
            set { _addPhoneNumber = value;
                OnPropertyChanged();
            }
        }



        public CurrentContactModel CurrentContact
        {
            get { 
                    return _currentContact; 
            }
            set { 
                _currentContact = value;
                OnPropertyChanged();
            }
        }


        public ContactModel SelectedContact
        {
            get { return _selectedContact; }
            set
            {
                if (value != null)
                {
                    _selectedContact = value;

                    if (value != null)
                    {
                        if (CurrentContact == null)
                            this.CurrentContact = new CurrentContactModel();


                        CurrentContact.UpdateWith(value);
                    }


                    OnPropertyChanged();
                }
            }
        }

        public ICommand SaveCommand { get;  set; }
        public ICommand AddNewCommand { get; set; }

        public ICommand DeleteEmailCommand { get; set; }
        public ICommand DeletePhoneNoCommand { get; set; }

        public ICommand ClearCommand { get; set; }

        public ICommand SearchCommand { get; set; }

        public ICommand AddEmailCommand { get; set; }

        public ICommand AddPhoneNumberCommand { get; set; }

       

        //public ContactViewModel(IWPFContactService service)
        //{
        //    _service = service;
        //}

        public ContactViewModel()
        {
            _service = new WPFContactService();
            Initialize();
        }

        private void Initialize()
        {
            AddNewCommand = new DelegateCommand(AddContactExecute, CanAddContact);
            SaveCommand = new DelegateCommand(UpdateConatactMethod, CanExecuteContactUpdate);

            DeleteEmailCommand = new DelegateCommand(DeleteEmailExecute, CanDeleteEmail);
            DeletePhoneNoCommand = new DelegateCommand(DeletePhoneCommand, CanDeletePhoneCommand);

            AddEmailCommand = new DelegateCommand(AddNewEmailCommand, CanAddNewEmailCommand);

            AddPhoneNumberCommand = new DelegateCommand(AddNewPhoneNumber, CanAddNewPhoneNumber);

            ClearCommand = new DelegateCommand(ClearCurrentContact, CanClearCurrentContact);
            LoadContacts();
            //SelectedContact = new ContactModel();

        }

        private bool CanClearCurrentContact(object arg)
        {
            return true;
        }

        private void ClearCurrentContact(object obj)
        {
            if (CurrentContact != null)
            {
                CurrentContact.Emails.Clear();
                CurrentContact.PhoneNumbers.Clear();
                CurrentContact.FirstName = string.Empty;
                CurrentContact.LastName = string.Empty;
                CurrentContact.DateofBirth = DateTime.Now;
                CurrentContact.IsDirty = false;
                CurrentContact.IsNew = false;

                AddEmail = string.Empty;
                AddPhoneNumber = string.Empty;

            }
        }

        private bool CanAddNewPhoneNumber(object arg)
        {
            return true;
        }

        private void AddNewPhoneNumber(object obj)
        {
            if (obj != null)
            {
                PhoneModel pm = new PhoneModel() { PhoneNumber = obj.ToString() };
                CurrentContact.PhoneNumbers.Add(pm);
                AddPhoneNumber = string.Empty;
            }
        }

        private bool CanAddNewEmailCommand(object arg)
        {
            return true;
        }

        private void AddNewEmailCommand(object obj)
        {
            if (obj != null)
            {
                EmailModel pm = new EmailModel() { Email = obj.ToString() };
                CurrentContact.Emails.Add(pm);
                AddEmail = string.Empty;
            }
        }

        private bool CanDeleteEmail(object arg)
        {
            return true;
        }

        private void DeleteEmailExecute(object obj)
        {
            if (obj == null) return;

            if (CurrentContact != null)
            {
                CurrentContact.Emails.Remove(CurrentContact.Emails.Where(x=>x.Email == obj.ToString()).FirstOrDefault());
            }
        }

        private bool CanDeletePhoneCommand(object arg)
        {
            return true;
        }

        private void DeletePhoneCommand(object obj)
        {
            if (obj == null) return;

            if (CurrentContact != null)
            {
                CurrentContact.PhoneNumbers.Remove(CurrentContact.PhoneNumbers.Where(x => x.PhoneNumber== obj.ToString()).FirstOrDefault());
            }
        }
        
        private void AddContactExecute(object obj)
        {
            try
            {
                CurrentContact = new CurrentContactModel();
            }
            catch (Exception)
            {

                throw;
            }
        }

        private bool CanAddContact(object arg)
        {
            return true;
        }

        private bool CanExecuteContactUpdate(object arg)
        {
            return true;
        }

        private void UpdateConatactMethod(object obj)
        {
            if (CurrentContact != null)
            {
                if (SelectedContact == null) { SelectedContact = new ContactModel(); }

                SelectedContact.UpdateWith(CurrentContact);

                if (CurrentContact.IsNew)
                {
                    
                    
                    if (_service.CreateContact(SelectedContact))
                    {
                        LoadContacts();
                        MessageBox.Show(CreateSuccessMsg, "Create", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else
                    {
                        MessageBox.Show(FailRecordMsg, "Create", MessageBoxButton.OK, MessageBoxImage.Error);
                    }


                }
                else 
                {
                   if (!CurrentContact.IsDirty) return;

                   // SelectedContact.UpdateWith(CurrentContact);

                    if (_service.UpdateContact(SelectedContact))
                    {
                        LoadContacts();
                        MessageBox.Show(UpdateSuccessMsg, "Update", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else
                    {
                        MessageBox.Show(UpdateFailMsg, "Update", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }

            }
        }



        public ObservableCollection<ContactModel> ContactCollection { get => _contactCollection; set { _contactCollection = value; OnPropertyChanged(); } }

        private  void LoadContacts()
        {
            
            try
            {
                ContactCollection =   _service.GetAllContacts();

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public  void Dispose()
        {
            if (ContactCollection != null)
            {
                ContactCollection.Clear();
                ContactCollection = null;
            }

            SelectedContact = null;
        }
    }



    public class CurrentContactModel : ViewModelBase,IDataErrorInfo
    {
        private bool _issNew;
        private bool _IsDirty;
        private string firstName;
        private string lastName;
        private DateTime dateofBirth;
        private ObservableCollection<EmailModel> emails ;
        private ObservableCollection<PhoneModel> phoneNumbers ;


        public CurrentContactModel()
        {
            IsNew = true;
            Emails = new ObservableCollection<EmailModel>();
            PhoneNumbers = new ObservableCollection<PhoneModel>();
            Emails.CollectionChanged += Emails_CollectionChanged;
            PhoneNumbers.CollectionChanged += PhoneNumbers_CollectionChanged;
        }

        private void PhoneNumbers_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null)
            {
                foreach (var item in e.NewItems)
                {
                    ((INotifyPropertyChanged)item).PropertyChanged += PhoneNumbers_PropertyChanged;
                    this.IsDirty = true;
                }
            }

            if (e.OldItems != null)
            {
                foreach (var item in e.OldItems)
                {
                    ((INotifyPropertyChanged)item).PropertyChanged -= PhoneNumbers_PropertyChanged;
                    this.IsDirty = true;
                }
            
            }

        }

        private void PhoneNumbers_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "PhoneNumber")
                this.IsDirty = true;
        }

      

        private void Emails_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null)
            {
                foreach (var item in e.NewItems)
                {
                    ((INotifyPropertyChanged)item).PropertyChanged += CurrentContactModel_PropertyChanged;
                    this.IsDirty = true;
                }
            }

            if (e.OldItems != null)
            {
                foreach (var item in e.OldItems)
                {
                    ((INotifyPropertyChanged)item).PropertyChanged -= CurrentContactModel_PropertyChanged;
                    this.IsDirty = true;
                }

            }
        }

        private void CurrentContactModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Email")
                this.IsDirty = true;
        }
        public bool IsNew
        {
            get { return _issNew; }
            set
            {
                _issNew = value;
                OnPropertyChanged();
            }
        }

        public bool IsDirty
        {
            get { return _IsDirty; }
            set
            {
                _IsDirty = value;
                OnPropertyChanged();
            }
        }


        public int Id { get; set; }
        public string FirstName { 
            
            get => firstName; 
            
            set { firstName = value; IsDirty = true; OnPropertyChanged(); }
        }
        public string LastName { get => lastName;
            set { lastName = value; IsDirty = true; OnPropertyChanged(); }
        }

        public DateTime DateofBirth { get => dateofBirth;

            set { dateofBirth = value; IsDirty = true; OnPropertyChanged(); } }

        public ObservableCollection<EmailModel> Emails { get => emails;

            set { emails = value; IsDirty = true; OnPropertyChanged(); }
        }
        public ObservableCollection<PhoneModel> PhoneNumbers
        {
            get => phoneNumbers;

            set { phoneNumbers = value; IsDirty = true; OnPropertyChanged(); }
        }


        #region Idataerro members

        private string _error;

        public string Error
        {
            get => _error;
            set
            {
                if (_error != value)
                {
                    _error = value;
                    OnPropertyChanged();
                }
            }
        }

        public string this[string columnName]
        {
            get
            {
                return OnValidate(columnName);
            }
        }

        private string OnValidate(string columnName)
        {
            string result = string.Empty;
            if (columnName == "FirstName")
            {
                if (string.IsNullOrEmpty(FirstName))
                {
                    result = "FirstName is mandatory";
                }
                else if (!Regex.IsMatch(FirstName, @"^[a-zA-Z]+$"))
                {
                    result = "Should enter alphabets only!!!";
                }
            }

            if (columnName == "LastName")
            {
                if (string.IsNullOrEmpty(LastName))
                {
                    result = "LastName is mandatory";
                }
                else if (!Regex.IsMatch(LastName, @"^[a-zA-Z]+$"))
                {
                    result = "Should enter alphabets only!!!";
                }
            }

            //if (columnName == "Email")
            //{
            //    if (string.IsNullOrEmpty(Email))
            //    {
            //        result = "Email Required";
            //    }
            //    else if (!Regex.IsMatch(Email, "[a-zA-Z0-9._%+-]+@[a-zA-Z0-9-]+\.[a-zA-Z]{1,4}"))
            //    {
            //        result = "Invalid Email ID";
            //    }
            //}
            if (result == null)
            {
                Error = null;
            }
            else
            {
                Error = "Error";
            }
            return result;
        }


        #endregion



        public void UpdateWith(ContactModel cvm)
        {

            if (cvm != null)
            {
                IsNew = false;
                Id = cvm.Id;
                //if (phoneNumbers == null)
                //    phoneNumbers = new ObservableCollection<PhoneModel>();
                //else
                //    phoneNumbers.Clear();

                //if (Emails == null)
                //    Emails = new ObservableCollection<EmailModel>();
                //else
                //    Emails.Clear();

                if (phoneNumbers != null)
                {
                    PhoneNumbers.Clear();

                    foreach (var item in cvm.PhoneNumbers)
                    {
                        PhoneNumbers.Add(item);
                    }
                }

                if (Emails != null)
                {

                    Emails.Clear();

                    foreach (var item in cvm.Emails)
                    {
                        Emails.Add(item);
                    }
                }

               
                DateofBirth = cvm.DateofBirth;
                FirstName = cvm.FirstName;
                LastName = cvm.LastName;
                IsDirty = false;
            }

        }


    }

}
