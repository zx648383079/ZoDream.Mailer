using GalaSoft.MvvmLight;

namespace ZoDream.Mailer.Model
{
    public class EmailItem: ObservableObject
    {
        public string Email { get; set; }

        /// <summary>
        /// 发送模板的值 用|分割
        /// </summary>
        public string Value { get; set; }

        private EmailStatus _status;

        public EmailStatus Status
        {
            get { return _status; }
            set {
                if (_status == value)
                {
                    return;
                }

                _status = value;
                RaisePropertyChanged("Status");
            }
        }

        public string Message { get; set; }

        public EmailItem()
        {

        }

        public EmailItem(string email)
        {
            Email = email;
        }

        public EmailItem(string email, string value)
        {
            Value = value;
            Email = email;
        }
    }
}
