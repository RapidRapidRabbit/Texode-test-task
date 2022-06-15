
namespace WpfTestApp.Services
{
    public interface IUserDialog
    {
        bool OpenFile(string title, string filter, out string selectedFile);
    }
}
