using Caliburn.Micro;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection.Emit;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using WPF_HackersList.Classes;
using WPF_HackersList.DataBaseClasses;
using WPF_HackersList.DataBaseClasses.DataBaseMethods;
using WPF_HackersList.Models;

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

        //////// Program settings start
        private string _selectedFontSize = Properties.Settings.Default.FontSize.ToString();
        public string SelectedFontSize
        {
            get { return _selectedFontSize; }
            set { _selectedFontSize = value; NotifyOfPropertyChange(() => SelectedFontSize); SetNewFontSize(); }
        }
        //////// Rainbow Six Siege end

        //////// Rainbow Six Siege start
        private readonly R6SGameSettings R6SGameSettings = new R6SGameSettings();

        private List<string> _r6sAccountsCollection;
        public List<string> R6SAccountsCollection
        {
            get { return _r6sAccountsCollection; }
            set { _r6sAccountsCollection = value; NotifyOfPropertyChange(() => R6SAccountsCollection); }
        }

        private string _selectedR6SAccount;
        public string SelectedR6SAccount
        {
            get { return _selectedR6SAccount; }
            set { _selectedR6SAccount = value; NotifyOfPropertyChange(() => SelectedR6SAccount);  }
        }

        private string _selectedR6SRegion;
        public string SelectedR6SRegion
        {
            get { return _selectedR6SRegion; }
            set { _selectedR6SRegion = value; NotifyOfPropertyChange(() => SelectedR6SRegion); }
        }
        ////////// Rainbow Six Siege end

        //////// Program settings start
        public UCSettingsViewModel()
        {
            GetR6SData();
        }

        public void SetNewFontSize()
        {
            Properties.Settings.Default.FontSize = Convert.ToDouble(SelectedFontSize);
            Properties.Settings.Default.Save();
            NotifyOfPropertyChange(() => Properties.Settings.Default.FontSize);
        }
        //////// Rainbow Six Siege end

        //////// Rainbow Six Siege start
        public async void SetR6SRegion()
        {
            if (String.IsNullOrWhiteSpace(SelectedR6SAccount))
            {
                MessageBox.Show($"Не найдены аккаунты Rainbow Sixe Siege в документах", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            R6SGameSettings.SetNewRegion(SelectedR6SAccount, SelectedR6SRegion);

            if(R6SGameSettings.SuccesefulReplaced)
                MessageBox.Show($"Замена региона прошла успешно на {SelectedR6SRegion}", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);            
        }

        public void GetR6SData()
        {
            var rainbowSixSiegeAccounts = R6SGameSettings.GetAccountsNames();

            if (rainbowSixSiegeAccounts.Count() < 1)
            {
                MessageBox.Show($"Не найдены аккаунты Rainbow Sixe Siege в документах", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            R6SAccountsCollection = rainbowSixSiegeAccounts;
            SelectedR6SAccount = rainbowSixSiegeAccounts.FirstOrDefault();
            SelectedR6SRegion = R6SGameSettings.GetAccountRegion(SelectedR6SAccount);
        }
        ////////// Rainbow Six Siege end

        public void GetSecondDataBasePath()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.ShowDialog();

            SecondDataBaseFullPath = openFileDialog.FileName;
        }

        public void GetSecondDataBasePeople()
        {
            if (String.IsNullOrWhiteSpace(SecondDataBaseFullPath) || !SecondDataBaseFullPath.Contains(".db", StringComparison.OrdinalIgnoreCase))
            {
                MessageBox.Show("Поле не заполнено корректно", "Внимание", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }            

            IDataBaseGetMethods DataBaseGetMethods = DependencyResolver.Resolve<IDataBaseGetMethods>();

            DataBaseGetMethods.GetDataFromOtherDataBase(SecondDataBaseFullPath);
        }
    }
}
