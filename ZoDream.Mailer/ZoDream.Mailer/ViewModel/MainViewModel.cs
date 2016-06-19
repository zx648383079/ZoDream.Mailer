using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using ZoDream.Helper.Local;
using ZoDream.Mailer.Model;
using ZoDream.Mailer.View;

namespace ZoDream.Mailer.ViewModel
{
    /// <summary>
    /// This class contains properties that the main View can data bind to.
    /// <para>
    /// See http://www.mvvmlight.net
    /// </para>
    /// </summary>
    public class MainViewModel : ViewModelBase
    {

        private string _file;

        private Task _timer;

        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public MainViewModel()
        {
            _timer = Task.Factory.StartNew(() =>
            {
                while (true)
                {
                    Time = DateTime.Now;
                    Thread.Sleep(500);
                }
            });
        }

        /// <summary>
        /// The <see cref="Message" /> property's name.
        /// </summary>
        public const string MessagePropertyName = "Message";

        private string _message = string.Empty;

        /// <summary>
        /// Sets and gets the Message property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string Message
        {
            get
            {
                return _message;
            }
            set
            {
                Set(MessagePropertyName, ref _message, value);
            }
        }

        /// <summary>
        /// The <see cref="Time" /> property's name.
        /// </summary>
        public const string TimePropertyName = "Time";

        private DateTime _time = new DateTime();

        /// <summary>
        /// Sets and gets the Time property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public DateTime Time
        {
            get
            {
                return _time;
            }
            set
            {
                Set(TimePropertyName, ref _time, value);
            }
        }

        /// <summary>
            /// The <see cref="EmailList" /> property's name.
            /// </summary>
        public const string EmailListPropertyName = "EmailList";

        private ObservableCollection<EmailItem> _emailList = new ObservableCollection<EmailItem>();

        /// <summary>
        /// Sets and gets the EmailList property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public ObservableCollection<EmailItem> EmailList
        {
            get
            {
                return _emailList;
            }
            set
            {
                Set(EmailListPropertyName, ref _emailList, value);
            }
        }

        private RelayCommand _newCommand;

        /// <summary>
        /// Gets the NewCommand.
        /// </summary>
        public RelayCommand NewCommand
        {
            get
            {
                return _newCommand
                    ?? (_newCommand = new RelayCommand(ExecuteNewCommand));
            }
        }

        private void ExecuteNewCommand()
        {
            new EmailView().Show();
            Messenger.Default.Send(new NotificationMessageAction<EmailItem>(null, item => {
                EmailList.Add(item);
            }), "email");
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
            if (index < 0 || index >= EmailList.Count) return;
            new EmailView().Show();
            Messenger.Default.Send(new NotificationMessageAction<EmailItem>(EmailList[index], null, item => {
                EmailList[index] = item;
            }), "email");
        }

        private RelayCommand _importCommand;

        /// <summary>
        /// Gets the OpenCommand.
        /// </summary>
        public RelayCommand ImportCommand
        {
            get
            {
                return _importCommand
                    ?? (_importCommand = new RelayCommand(ExecuteImportCommand));
            }
        }

        private void ExecuteImportCommand()
        {
            _import(Open.ChooseFile());
        }

        const string Pattern = @"([^@\s]+@[^\s^@]+)\s+(.+)";

        private void _import(string file)
        {
            if (string.IsNullOrEmpty(file) || !File.Exists(file))
            {
                return;
            }

            var encoder = new TxtEncoder();
            StreamReader sr = new StreamReader(file, encoder.GetEncoding(file));
            string line;
            while ((line = sr.ReadLine()) != null)
            {
                if (!Regex.IsMatch(line, Pattern))
                {
                    continue;
                }
                var match = Regex.Match(line, Pattern);
                EmailList.Add(new EmailItem(match.Groups[1].Value, match.Groups[2].Value));
            }
            sr.Close();

        }

        private RelayCommand<DragEventArgs> _fileDrogCommand;

        /// <summary>
        /// Gets the FileDrogCommand.
        /// </summary>
        public RelayCommand<DragEventArgs> FileDrogCommand
        {
            get
            {
                return _fileDrogCommand
                    ?? (_fileDrogCommand = new RelayCommand<DragEventArgs>(ExecuteFileDrogCommand));
            }
        }

        private void ExecuteFileDrogCommand(DragEventArgs parameter)
        {
            if (parameter == null)
            {
                return;
            }
            var files = (System.Array)parameter.Data.GetData(DataFormats.FileDrop);
            //        as FileInfo[];

            foreach (string item in files)
            {
                _import(item);
            }
        }

        private RelayCommand _exportCommand;

        /// <summary>
        /// Gets the SaveCommand.
        /// </summary>
        public RelayCommand ExportCommand
        {
            get
            {
                return _exportCommand
                    ?? (_exportCommand = new RelayCommand(ExecuteExportCommand));
            }
        }

        private void ExecuteExportCommand()
        {
            if (string.IsNullOrEmpty(_file))
            {
                _saveAs();
                return;
            }
            _save();
        }

        private void _saveAs()
        {
            _file = Open.ChooseSaveFile();
            _save();
        }

        private void _save()
        {
            if (string.IsNullOrEmpty(_file))
            {
                return;
            }
            StreamWriter sw = new StreamWriter(_file, false, Encoding.UTF8);
            foreach (var item in EmailList)
            {
                sw.WriteLine($"{item.Email} {item.Value}");
            }
            sw.Close();
        }

        private RelayCommand _saveAsCommand;

        /// <summary>
        /// Gets the SaveAsCommand.
        /// </summary>
        public RelayCommand SaveAsCommand
        {
            get
            {
                return _saveAsCommand
                    ?? (_saveAsCommand = new RelayCommand(ExecuteSaveAsCommand));
            }
        }

        private void ExecuteSaveAsCommand()
        {
            _saveAs();
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
            if (index < 0 || index >= EmailList.Count) return;
            EmailList.RemoveAt(index);
        }

        private RelayCommand _deleteCompleteCommand;

        /// <summary>
        /// Gets the DeleteCompleteCommand.
        /// </summary>
        public RelayCommand DeleteCompleteCommand
        {
            get
            {
                return _deleteCompleteCommand
                    ?? (_deleteCompleteCommand = new RelayCommand(ExecuteDeleteCompleteCommand));
            }
        }

        private void ExecuteDeleteCompleteCommand()
        {
            for (int i = EmailList.Count; i > 0; i--)
            {
                if (EmailList[i].Status == EmailStatus.Success)
                {
                    EmailList.RemoveAt(i);
                }
            }
        }

        private RelayCommand _clearCommand;

        /// <summary>
        /// Gets the ClearCommand.
        /// </summary>
        public RelayCommand ClearCommand
        {
            get
            {
                return _clearCommand
                    ?? (_clearCommand = new RelayCommand(ExecuteClearCommand));
            }
        }

        private void ExecuteClearCommand()
        {
            EmailList.Clear();
        }

        private RelayCommand<int> _viewCommand;

        /// <summary>
        /// Gets the ViewCommand.
        /// </summary>
        public RelayCommand<int> ViewCommand
        {
            get
            {
                return _viewCommand
                    ?? (_viewCommand = new RelayCommand<int>(ExecuteViewCommand));
            }
        }

        private void ExecuteViewCommand(int index)
        {
            if (index < 0 || index >= EmailList.Count)
            {
                Message = string.Empty;
                return;
            }
            Message = EmailList[index].Status.ToString();
            if (EmailList[index].Status == EmailStatus.Failure)
            {
                Message += "  错误提示：" + EmailList[index].Message;
            }
        }

        private Task _task;

        private RelayCommand _startCommand;

        /// <summary>
        /// Gets the StartCommand.
        /// </summary>
        public RelayCommand StartCommand
        {
            get
            {
                return _startCommand
                    ?? (_startCommand = new RelayCommand(ExecuteStartCommand));
            }
        }

        private void ExecuteStartCommand()
        {
            _start();
        }

        private void _start()
        {
            foreach (var item in EmailList)
            {
                item.Status = EmailStatus.Waiting;
            }
            _task = Task.Factory.StartNew(() =>
            {
                foreach (var item in EmailList)
                {

                }
            });
        }

        private RelayCommand _stopCommand;

        /// <summary>
        /// Gets the StopCommand.
        /// </summary>
        public RelayCommand StopCommand
        {
            get
            {
                return _stopCommand
                    ?? (_stopCommand = new RelayCommand(ExecuteStopCommand));
            }
        }

        private void ExecuteStopCommand()
        {
            _stop();
        }

        private void _stop()
        {
            _task.Dispose();
            foreach (var item in EmailList)
            {
                item.Status = EmailStatus.None;
            }
        }

        private RelayCommand _pauseCommand;

        /// <summary>
        /// Gets the PauseCommand.
        /// </summary>
        public RelayCommand PauseCommand
        {
            get
            {
                return _pauseCommand
                    ?? (_pauseCommand = new RelayCommand(ExecutePauseCommand));
            }
        }

        private void ExecutePauseCommand()
        {
            _task.Dispose();
        }

        private RelayCommand _restartCommand;

        /// <summary>
        /// Gets the RestartCommand.
        /// </summary>
        public RelayCommand RestartCommand
        {
            get
            {
                return _restartCommand
                    ?? (_restartCommand = new RelayCommand(ExecuteRestartCommand));
            }
        }

        private void ExecuteRestartCommand()
        {
            _stop();
            _start();
        }

        private RelayCommand _templateCommand;

        /// <summary>
        /// Gets the TemplateCommand.
        /// </summary>
        public RelayCommand TemplateCommand
        {
            get
            {
                return _templateCommand
                    ?? (_templateCommand = new RelayCommand(ExecuteTemplateCommand));
            }
        }

        private void ExecuteTemplateCommand()
        {
            new TemplateView().Show();
        }


        private RelayCommand _smtpCommand;

        /// <summary>
        /// Gets the SmtpCommand.
        /// </summary>
        public RelayCommand SmtpCommand
        {
            get
            {
                return _smtpCommand
                    ?? (_smtpCommand = new RelayCommand(ExecuteSmtpCommand));
            }
        }

        private void ExecuteSmtpCommand()
        {
            new SmtpView().Show();
        }

        private void _showMessage(string message)
        {
            Message = message;
            Task.Factory.StartNew(() =>
            {
                Thread.Sleep(3000);
                Message = string.Empty;
            });
        }

        public override void Cleanup()
        {
            // Clean up if needed
            _timer.Dispose();
            base.Cleanup();
        }
    }
}