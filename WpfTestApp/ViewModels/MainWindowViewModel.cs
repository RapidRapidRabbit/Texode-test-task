using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using WpfTestApp.Infrastructure.Commands;
using WpfTestApp.Models;
using WpfTestApp.Services;
using WpfTestApp.Views.Windows;

namespace WpfTestApp.ViewModels
{
    internal class MainWindowViewModel : ViewModelBase
    {
        private readonly IDataService _dataService;
        private Animal _selectedAnimal;
        public ObservableCollection<Animal> Animals { get; set; }
       

        public Animal SelectedAnimal
        {
            get => _selectedAnimal;
            set
            {
                _selectedAnimal = value;
                OnPropertyChanged(nameof(SelectedAnimal));
            }
        }
        public ICommand AddCommand { get; }
        public ICommand DeleteCommand { get; }
        public ICommand EditCommand { get; }

        public MainWindowViewModel()
        {
            _dataService = new DataService();

            
            
            AddCommand = new ActionCommand(OnAddCommandExecuted);
            EditCommand = new ActionCommand(OnEditCommandExecuted, EditCommandCanExecuted);
            DeleteCommand = new ActionCommand(OnDeleteCommandExecuted, DeleteCommandCanExecute);

            try
            {
                Animals = new ObservableCollection<Animal>(_dataService.GetAll());
            }
            catch
            {
                Animals = new ObservableCollection<Animal>();
            }
        }

        #region Add animal command
        private async void OnAddCommandExecuted(object p)
        {
            var dlg = new EditWindow();

            var model = new EditViewModel();

            dlg.DataContext = model;


            if (dlg.ShowDialog() != true)
                return;

            if (string.IsNullOrWhiteSpace(model.EditAnimal.Name))
            {
                MessageBox.Show("Name should not be empty");
                return;
            }

            if (model.SelectedFile == null)
            {
                MessageBox.Show("Please, select a file");
                return;
            }

            try
            {
                var result = await _dataService.Add(model.EditAnimal.Name, model.SelectedFile);
                Animals.Add(result);
            }
            catch
            {
                ShowErrorMessage();
            }
        }


        #endregion

        #region Delete animal command

        private async void OnDeleteCommandExecuted(object p)
        {
            var animal = p as Animal;

            try
            {
                bool result = await _dataService.Delete(animal.Id);

                if (result)
                {
                    Animals.Remove(p as Animal);
                }
                else
                {
                    ShowErrorMessage();
                }
            }
            catch
            {
                ShowErrorMessage();
            }
            
        }

        private bool DeleteCommandCanExecute(object p) => SelectedAnimal != null;

        #endregion

        #region EditCommand

        private async void OnEditCommandExecuted(object p)
        {
            var animal = p as Animal;

            var dlg = new EditWindow();
            var model = new EditViewModel
            {
                EditAnimal = animal
            };

            dlg.DataContext = model;


            if (dlg.ShowDialog() != true)
                return;

            try
            {
                var result = await _dataService.Update(model.EditAnimal.Id, model.EditAnimal.Name, model.SelectedFile);
                Animals[Animals.IndexOf(animal)] = result;
            }
            catch
            {
                ShowErrorMessage();
            }
        }

        private bool EditCommandCanExecuted(object p) => SelectedAnimal != null;

        #endregion

        private static void ShowErrorMessage() => MessageBox.Show("Something went wrong", "Error");
    }
}
