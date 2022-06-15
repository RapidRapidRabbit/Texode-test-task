using Microsoft.Win32;

namespace WpfTestApp.Services
{
    internal class UserDialogService : IUserDialog
    {
        public bool OpenFile(string title, string filter, out string selectedFile)
        {
            var fileDialog = new OpenFileDialog()
            {
                Title = title,
                Filter = filter,
            };

            if (fileDialog.ShowDialog() != true)
            {
                selectedFile = null;
                return false;
            }

            selectedFile = fileDialog.FileName;
            return true;
        }
    }
}
