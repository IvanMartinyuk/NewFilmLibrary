using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace LibraryOfNewFilm.Infrastructure
{
    public class RelayCommand : ICommand
    {
        readonly Action<object> action;
        readonly Predicate<object> predicate;

        public RelayCommand(Action<object> ac, Predicate<object> pr = null)
        {
            action = ac;
            predicate = pr;
        }
        public event EventHandler CanExecuteChanged
        {
            add
            {
                CommandManager.RequerySuggested += value;
            }
            remove
            {
                CommandManager.RequerySuggested -= value;
            }
        }

        public bool CanExecute(object parameter)
        {
            return predicate == null ? true : predicate(parameter);
        }

        public void Execute(object parameter)
        {
            action(parameter);
        }
    }
}
