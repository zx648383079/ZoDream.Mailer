using GalaSoft.MvvmLight.Messaging;
using System.Windows;

namespace ZoDream.Mailer.View
{
    /// <summary>
    /// Description for EmailView.
    /// </summary>
    public partial class EmailView : Window
    {
        /// <summary>
        /// Initializes a new instance of the EmailView class.
        /// </summary>
        public EmailView()
        {
            InitializeComponent();
            Messenger.Default.Send(new NotificationMessageAction(null, Close), "close");
        }
    }
}