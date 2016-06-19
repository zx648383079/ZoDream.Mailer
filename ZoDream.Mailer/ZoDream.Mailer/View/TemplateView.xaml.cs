using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace ZoDream.Mailer.View
{
    /// <summary>
    /// Description for TemplateView.
    /// </summary>
    public partial class TemplateView : Window
    {
        private Task _timer;

        /// <summary>
        /// Initializes a new instance of the TemplateView class.
        /// </summary>
        public TemplateView()
        {
            InitializeComponent();
            //_laodHtml();
            //Closing += CloseWindow;
            HtmlView.Navigating += HtmlView_Navigating;
        }

        private void HtmlView_Navigating(object sender, System.Windows.Navigation.NavigatingCancelEventArgs e)
        {
            SetWebBrowserSilent(sender as WebBrowser, true);
        }

        /// <summary>  
        /// 设置浏览器静默，不弹错误提示框  
        /// </summary>  
        /// <param name="webBrowser">要设置的WebBrowser控件浏览器</param>  
        /// <param name="silent">是否静默</param>  
        private void SetWebBrowserSilent(WebBrowser webBrowser, bool silent)
        {
            FieldInfo fi = typeof(WebBrowser).GetField("_axIWebBrowser2", BindingFlags.Instance | BindingFlags.NonPublic);
            if (fi != null)
            {
                object browser = fi.GetValue(webBrowser);
                if (browser != null)
                    browser.GetType().InvokeMember("Silent", BindingFlags.SetProperty, null, browser, new object[] { silent });
            }
        }

        private void CloseWindow(object sender, System.ComponentModel.CancelEventArgs e)
        {
            _timer.Dispose();
        }


        private void _laodHtml()
        {
            _timer = Task.Factory.StartNew(()=> {
                while(true)
                {
                    Thread.Sleep(200);
                    if (!string.IsNullOrEmpty(HtmlEditor.Text))
                    {
                        this.Dispatcher.Invoke(() =>
                        {
                            HtmlView.NavigateToString(HtmlEditor.Text);
                        });
                    }
                }
            });
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            HtmlView.NavigateToString(HtmlEditor.Text);
        }
    }
}