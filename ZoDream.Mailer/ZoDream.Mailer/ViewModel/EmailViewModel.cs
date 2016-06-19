using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using ZoDream.Mailer.Model;

namespace ZoDream.Mailer.ViewModel
{
    /// <summary>
    /// This class contains properties that a View can data bind to.
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class EmailViewModel : ViewModelBase
    {
        private NotificationMessageAction<EmailItem> _callBack;

        private NotificationMessageAction _close;

        /// <summary>
        /// Initializes a new instance of the EmailViewModel class.
        /// </summary>
        public EmailViewModel()
        {
            Messenger.Default.Register<NotificationMessageAction<EmailItem>>(this, "rule", m =>
            {
                _callBack = m;
                if (m.Sender == null)
                {
                    return;
                }
                var item = m.Sender as EmailItem;
                Email = item.Email;
                Params = item.Value;
            });

            Messenger.Default.Register<NotificationMessageAction>(this, "close", m =>
            {
                _close = m;
            });
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
        /// The <see cref="Params" /> property's name.
        /// </summary>
        public const string ParamsPropertyName = "Params";

        private string _params = string.Empty;

        /// <summary>
        /// Sets and gets the Params property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string Params
        {
            get
            {
                return _params;
            }
            set
            {
                Set(ParamsPropertyName, ref _params, value);
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
            if (string.IsNullOrWhiteSpace(Email)) return;
            _callBack.Execute(new EmailItem(Email, Params));
            Email = string.Empty;
            Params = string.Empty;
            _close.Execute();
        }
    }
}