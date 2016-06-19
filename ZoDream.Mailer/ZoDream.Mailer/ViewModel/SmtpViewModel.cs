using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System.Collections.ObjectModel;
using System.Windows;
using ZoDream.Mailer.Model;

namespace ZoDream.Mailer.ViewModel
{
    /// <summary>
    /// This class contains properties that a View can data bind to.
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class SmtpViewModel : ViewModelBase
    {

        private int _index = -1;

        /// <summary>
        /// Initializes a new instance of the SmtpViewModel class.
        /// </summary>
        public SmtpViewModel()
        {
            _init();
        }

        /// <summary>
        /// The <see cref="SmtpList" /> property's name.
        /// </summary>
        public const string SmtpListPropertyName = "SmtpList";

        private ObservableCollection<ServerItem> _smtpList = new ObservableCollection<ServerItem>();

        /// <summary>
        /// Sets and gets the SmtpList property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public ObservableCollection<ServerItem> SmtpList
        {
            get
            {
                return _smtpList;
            }
            set
            {
                Set(SmtpListPropertyName, ref _smtpList, value);
            }
        }

        /// <summary>
        /// The <see cref="Server" /> property's name.
        /// </summary>
        public const string ServerPropertyName = "Server";

        private string _server = string.Empty;

        /// <summary>
        /// Sets and gets the Server property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string Server
        {
            get
            {
                return _server;
            }
            set
            {
                Set(ServerPropertyName, ref _server, value);
            }
        }

        /// <summary>
        /// The <see cref="Port" /> property's name.
        /// </summary>
        public const string PortPropertyName = "Port";

        private int _port = 25;

        /// <summary>
        /// Sets and gets the Port property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public int Port
        {
            get
            {
                return _port;
            }
            set
            {
                Set(PortPropertyName, ref _port, value);
            }
        }

        /// <summary>
        /// The <see cref="User" /> property's name.
        /// </summary>
        public const string UserPropertyName = "User";

        private string _user = string.Empty;

        /// <summary>
        /// Sets and gets the User property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string User
        {
            get
            {
                return _user;
            }
            set
            {
                Set(UserPropertyName, ref _user, value);
                if (string.IsNullOrEmpty(Email) && value.IndexOf('@') > 0)
                {
                    Email = value;
                }

                if (string.IsNullOrEmpty(Name))
                {
                    Name = value;
                }
            }
        }

        /// <summary>
        /// The <see cref="Password" /> property's name.
        /// </summary>
        public const string PasswordPropertyName = "Password";

        private string _password = string.Empty;

        /// <summary>
        /// Sets and gets the Password property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string Password
        {
            get
            {
                return _password;
            }
            set
            {
                Set(PasswordPropertyName, ref _password, value);
            }
        }

        /// <summary>
        /// The <see cref="Email" /> property's name.
        /// </summary>
        public const string EmailPropertyName = "Email";

        private string _email = string.Empty;

        /// <summary>
        /// Sets and gets the Email property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string Email
        {
            get
            {
                return _email;
            }
            set
            {
                Set(EmailPropertyName, ref _email, value);
            }
        }

        /// <summary>
        /// The <see cref="IsSSL" /> property's name.
        /// </summary>
        public const string IsSSLPropertyName = "IsSSL";

        private bool _isSSL = false;

        /// <summary>
        /// Sets and gets the IsSSL property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public bool IsSSL
        {
            get
            {
                return _isSSL;
            }
            set
            {
                Set(IsSSLPropertyName, ref _isSSL, value);
            }
        }

        /// <summary>
        /// The <see cref="Name" /> property's name.
        /// </summary>
        public const string NamePropertyName = "Name";

        private string _name = string.Empty;

        /// <summary>
        /// Sets and gets the Name property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                Set(NamePropertyName, ref _name, value);
            }
        }

        /// <summary>
        /// The <see cref="AddVisibility" /> property's name.
        /// </summary>
        public const string AddVisibilityPropertyName = "AddVisibility";

        private Visibility _addVisibility = Visibility.Collapsed;

        /// <summary>
        /// Sets and gets the AddVisibility property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public Visibility AddVisibility
        {
            get
            {
                return _addVisibility;
            }
            set
            {
                Set(AddVisibilityPropertyName, ref _addVisibility, value);
            }
        }

        private RelayCommand _saveCommand;

        /// <summary>
        /// Gets the SaveCommand.
        /// </summary>
        public RelayCommand SaveCommand
        {
            get
            {
                return _saveCommand
                    ?? (_saveCommand = new RelayCommand(ExecuteSaveCommand));
            }
        }

        private void ExecuteSaveCommand()
        {
            if (string.IsNullOrEmpty(Server) || string.IsNullOrEmpty(User) || string.IsNullOrEmpty(Password)) return;

            var item = new ServerItem(Server, Port, IsSSL, User, Password, Email, Name);
            if (_index < 0)
            {
                SmtpList.Add(item);
            } else
            {
                SmtpList[_index] = item;
            }
            AddVisibility = Visibility.Collapsed;
            _init();
        }

        private RelayCommand<int> _editCommand;

        /// <summary>
        /// Gets the EditCommand.
        /// </summary>
        public RelayCommand<int> EditCommand
        {
            get
            {
                return _editCommand
                    ?? (_editCommand = new RelayCommand<int>(ExecuteEditCommand));
            }
        }

        private void ExecuteEditCommand(int index)
        {
            if (index < 0 || index >= SmtpList.Count) return;
            AddVisibility = Visibility.Visible;
            _index = index;
            Server = SmtpList[index].Server;
            User = SmtpList[index].User;
            Password = SmtpList[index].Password;
            Email = SmtpList[index].Email;
            Name = SmtpList[index].Name;
            Port = SmtpList[index].Port;
            IsSSL = SmtpList[index].IsSSL;
        }

        private RelayCommand _addCommand;

        /// <summary>
        /// Gets the AddCommand.
        /// </summary>
        public RelayCommand AddCommand
        {
            get
            {
                return _addCommand
                    ?? (_addCommand = new RelayCommand(ExecuteAddCommand));
            }
        }

        private void ExecuteAddCommand()
        {
            AddVisibility = Visibility.Visible;
            _init();
        }

        private void _init()
        {
            _index = -1;
            Name = Email = Password = User = Server = string.Empty;
            Port = 25;
            IsSSL = false;
        }

        private RelayCommand<int> _deleteCommand;

        /// <summary>
        /// Gets the DeleteCommand.
        /// </summary>
        public RelayCommand<int> DeleteCommand
        {
            get
            {
                return _deleteCommand
                    ?? (_deleteCommand = new RelayCommand<int>(ExecuteDeleteCommand));
            }
        }

        private void ExecuteDeleteCommand(int index)
        {
            if (index < 0 || index >= SmtpList.Count) return;
            SmtpList.RemoveAt(index);
        }
    }
}