using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PhotoSync.Models;

namespace PhotoSync.Commands
{
    internal class NewCommand : RelayCommand
    {
        public NewCommand()
            : base(ExecuteMethod, CanExecuteMethod)
        {
        }

        private static void ExecuteMethod(object parameter)
        {

        }

        private static bool CanExecuteMethod(object parameter) => string.IsNullOrWhiteSpace(AppState.Instance.SelectedLibrary);
    }
}
