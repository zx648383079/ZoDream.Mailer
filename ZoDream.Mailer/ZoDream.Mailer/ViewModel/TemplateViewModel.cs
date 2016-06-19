using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System.Collections.ObjectModel;
using ZoDream.Helper.Local;
using ZoDream.Mailer.Model;

namespace ZoDream.Mailer.ViewModel
{
    /// <summary>
    /// This class contains properties that a View can data bind to.
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class TemplateViewModel : ViewModelBase
    {
        private int _index = -1;

        /// <summary>
        /// Initializes a new instance of the TemplateViewModel class.
        /// </summary>
        public TemplateViewModel()
        {
            _init();
        }

        /// <summary>
        /// The <see cref="TemlpateList" /> property's name.
        /// </summary>
        public const string TemlpateListPropertyName = "TemlpateList";

        private ObservableCollection<TemplateItem> _templateList = new ObservableCollection<TemplateItem>();

        /// <summary>
        /// Sets and gets the TemlpateList property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public ObservableCollection<TemplateItem> TemlpateList
        {
            get
            {
                return _templateList;
            }
            set
            {
                Set(TemlpateListPropertyName, ref _templateList, value);
            }
        }

        /// <summary>
        /// The <see cref="Title" /> property's name.
        /// </summary>
        public const string TitlePropertyName = "Title";

        private string _title = string.Empty;

        /// <summary>
        /// Sets and gets the Title property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string Title
        {
            get
            {
                return _title;
            }
            set
            {
                Set(TitlePropertyName, ref _title, value);
            }
        }

        /// <summary>
        /// The <see cref="IsHtml" /> property's name.
        /// </summary>
        public const string IsHtmlPropertyName = "IsHtml";

        private bool _isHtml = false;

        /// <summary>
        /// Sets and gets the IsHtml property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public bool IsHtml
        {
            get
            {
                return _isHtml;
            }
            set
            {
                Set(IsHtmlPropertyName, ref _isHtml, value);
            }
        }

        /// <summary>
        /// The <see cref="Content" /> property's name.
        /// </summary>
        public const string ContentPropertyName = "Content";

        private string _content = string.Empty;

        /// <summary>
        /// Sets and gets the Content property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string Content
        {
            get
            {
                return _content;
            }
            set
            {
                Set(ContentPropertyName, ref _content, value);
            }
        }

        /// <summary>
        /// The <see cref="AttachmentList" /> property's name.
        /// </summary>
        public const string AttachmentListPropertyName = "AttachmentList";

        private ObservableCollection<string> _attachmentList = new ObservableCollection<string>();

        /// <summary>
        /// Sets and gets the AttachmentList property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public ObservableCollection<string> AttachmentList
        {
            get
            {
                return _attachmentList;
            }
            set
            {
                Set(AttachmentListPropertyName, ref _attachmentList, value);
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
            if (string.IsNullOrEmpty(Title)) return;
            var item = new TemplateItem(Title, Content, IsHtml);
            if (_index < 0)
            {
                TemlpateList.Add(item);
            }
            else
            {
                TemlpateList[_index] = item;
            }
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
            if (index < 0 || index >= TemlpateList.Count) return;
            _index = index;
            Title = TemlpateList[index].Title;
            IsHtml = TemlpateList[index].IsHtml;
            Content = TemlpateList[index].Content;
            foreach (var item in TemlpateList[index].Attachment)
            {
                AttachmentList.Add(item);
            }
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
            _init();
        }

        private void _init()
        {
            _index = -1;
            Title = Content = string.Empty;
            IsHtml = false;
            AttachmentList.Clear();
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
            if (index < 0 || index >= TemlpateList.Count) return;
            TemlpateList.RemoveAt(index);
        }

        private RelayCommand _addAttachmentCommand;

        /// <summary>
        /// Gets the AddAttachmentCommand.
        /// </summary>
        public RelayCommand AddAttachmentCommand
        {
            get
            {
                return _addAttachmentCommand
                    ?? (_addAttachmentCommand = new RelayCommand(ExecuteAddAttachmentCommand));
            }
        }

        private void ExecuteAddAttachmentCommand()
        {
            var files = Open.ChooseFiles("");
            foreach (var item in files)
            {
                AttachmentList.Add(item);
            }
        }

        private RelayCommand<int> _deleteAttachmentCommand;

        /// <summary>
        /// Gets the DeleteAttachmentCommand.
        /// </summary>
        public RelayCommand<int> DeleteAttachmentCommand
        {
            get
            {
                return _deleteAttachmentCommand
                    ?? (_deleteAttachmentCommand = new RelayCommand<int>(ExecuteDeleteAttachmentCommand));
            }
        }

        private void ExecuteDeleteAttachmentCommand(int index)
        {
            if (index < 0 || index >= AttachmentList.Count) return;
            AttachmentList.RemoveAt(index);
        }

        private RelayCommand _clearAttachmentCommand;

        /// <summary>
        /// Gets the ClearAttachmentCommand.
        /// </summary>
        public RelayCommand ClearAttachmentCommand
        {
            get
            {
                return _clearAttachmentCommand
                    ?? (_clearAttachmentCommand = new RelayCommand(ExecuteClearAttachmentCommand));
            }
        }

        private void ExecuteClearAttachmentCommand()
        {
            AttachmentList.Clear();
        }
        
    }
}