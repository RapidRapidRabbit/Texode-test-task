using System.IO;
using System.Windows.Input;
using WpfTestApp.Infrastructure.Commands;
using WpfTestApp.Services;

namespace WpfTestApp.ViewModels
{
    internal class AddViewModel : ViewModelBase
    {
        private FileInfo _selectedFile;
        private readonly IUserDialog _dialogService;
        public ICommand SelectFileCommand { get; }

        public FileInfo SelectedFile
        {
            get => _selectedFile;
            set
            {
                _selectedFile = value;
                OnPropertyChanged(nameof(SelectedFile));
            }
        }

        public AddViewModel()
        {
            SelectFileCommand = new ActionCommand(OnSelectFileCommandExecuted);

            _dialogService = new UserDialogService();
        }

        private void OnSelectFileCommandExecuted(object p)
        {
            if (!_dialogService.OpenFile("Выберите картинку", "Image Files|*.jpg;*.jpeg;*.png", out string path))
                return;

            SelectedFile = new FileInfo(path);
        }
    }

   
}
