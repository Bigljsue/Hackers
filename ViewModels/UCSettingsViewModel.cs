using Caliburn.Micro;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Reflection.Emit;
using System.Text;
using System.Windows;
using System.Windows.Controls;
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

        private string _selectedFontSize = Properties.Settings.Default.FontSize.ToString();

        public string SelectedFontSize
        {
            get { return _selectedFontSize; }
            set { _selectedFontSize = value; NotifyOfPropertyChange(() => SelectedFontSize); SetNewFontSize(); }
        }

        public void SetNewFontSize()
        {
            Properties.Settings.Default.FontSize = Convert.ToDouble(SelectedFontSize);
            Properties.Settings.Default.Save();
            NotifyOfPropertyChange(() => Properties.Settings.Default.FontSize);
        }

        public void GetSecondDataBasepPath()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.ShowDialog();

            SecondDataBaseFullPath = openFileDialog.FileName;
        }

        public void GetSecondDataBase()
        {
            if (String.IsNullOrWhiteSpace(SecondDataBaseFullPath) && SecondDataBaseFullPath.Contains(".db", StringComparison.OrdinalIgnoreCase))
            {
                MessageBox.Show("Поле не заполнено корректно", "Внимание", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }

            IDataBaseGetMethods DataBaseGetMethods = DependencyResolver.Resolve<IDataBaseGetMethods>();

            DataBaseGetMethods.GetDataFromOtherDataBase(SecondDataBaseFullPath);
        }
    }
}
