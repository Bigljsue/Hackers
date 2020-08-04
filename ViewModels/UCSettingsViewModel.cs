using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using WPF_HackersList.DataBaseClasses;
using WPF_HackersList.DataBaseClasses.DataBaseMethods;

namespace WPF_HackersList.ViewModels
{
    public class UCSettingsViewModel : Screen
    {
        private string _secondDataBaseFullPath;

        public string SecondDataBaseFullPath
        {
            get { return _secondDataBaseFullPath; }
            set { _secondDataBaseFullPath = value; NotifyOfPropertyChange(() => SecondDataBaseFullPath);  }
        }

        public void GetSecondDataBase()
        {
            MessageBox.Show(SecondDataBaseFullPath);
            if (String.IsNullOrWhiteSpace(SecondDataBaseFullPath) && SecondDataBaseFullPath.Contains(".db",StringComparison.OrdinalIgnoreCase))
            {
                MessageBox.Show("Поле не заполнено корректно");
                return;
            }

            IDataBaseGetMethods DataBaseGetMethods = DependencyResolver.Resolve<IDataBaseGetMethods>();

            DataBaseGetMethods.ConnectToSecondDataBase(SecondDataBaseFullPath);
        }
    }
}
