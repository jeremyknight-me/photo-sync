using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhotoSyncManager.ViewModels
{
    public class ExcludedFoldersViewModel
    {
        private readonly MainViewModel mainViewModel;

        public ExcludedFoldersViewModel(MainViewModel main)
        {
            this.mainViewModel = main;
        }
    }
}
