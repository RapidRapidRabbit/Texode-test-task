using System.Windows;

namespace WpfTestApp.Infrastructure.Commands
{
    public class CloseWindowCommand : BaseCommand
    {
        public override bool CanExecute(object parameter) => parameter is Window;

        public override void Execute(object parameter)
        {
           if (!CanExecute(parameter))
               return;

           var window = (Window)parameter;
           window.Close();
        }
    }
    public class CloseDialogCommand : BaseCommand
    {
        public bool DialogResult { get; set; }

        public override bool CanExecute(object parameter) => parameter is Window;

        public override void Execute(object parameter)
        {
            if (!CanExecute(parameter))
                return;

            var window = (Window)parameter;
            window.DialogResult = DialogResult;
            window.Close();
        }
    }
}
