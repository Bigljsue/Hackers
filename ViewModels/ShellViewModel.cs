using Caliburn.Micro;
using System;
using System.IO;
using System.Linq;
using System.Windows;
using WPF_HackersList.DataBaseClasses;

namespace WPF_HackersList.ViewModels
{
    public class ShellViewModel : Conductor<object>
    {
        public ShellViewModel()
        {
            CheckForDataBase();           
        }

        public void LoadHackersListPage()
        {
            CheckForDataBase();
            ActivateItem(new UCHackersListViewModel());            
        }

        public void LoadSettingsPage()
        {
            ActivateItem(new UCSettingsViewModel());
        }

        public void CheckForDataBase()
        {
            DataBaseDebug dataBaseDebug = new DataBaseDebug();

            if (dataBaseDebug.IsDataBaseExist())
            {
                dataBaseDebug.DebugDataBase();
                return;
            }

            dataBaseDebug.RestoreDataBase();
        }

    }
}
