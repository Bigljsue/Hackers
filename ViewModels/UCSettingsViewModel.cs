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
        private readonly R6SGameSettingsModel R6SGameSettings = new R6SGameSettingsModel();

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

        public UCSettingsViewModel()
        {
            GetR6SData();
            GetR6SRegion();
        }

        //////// Program settings start
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

            string gameSettingsNewFileText = null;

            R6SGameSettings.R6SGameSettingsFileTextSplited[R6SGameSettings.R6SIndexOfDataCenterHintInRow] = String.Format("DataCenterHint={0}\r", SelectedR6SRegion);

            foreach (var text in R6SGameSettings.R6SGameSettingsFileTextSplited)
                gameSettingsNewFileText += text + "\n";

            using (StreamWriter streamWriter = new StreamWriter(R6SGameSettings.R6SAccountGameSettingsFilePath))            
                await streamWriter.WriteAsync(gameSettingsNewFileText);

            MessageBox.Show($"Замена региона прошла успешно на {SelectedR6SRegion}", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);            
        }

        public void GetR6SData()
        {
            var rainbowSixSiegeAccounts = Directory.GetDirectories(R6SGameSettings.R6SAccountsDirectory);

            if (rainbowSixSiegeAccounts.Count() < 1)
            {
                MessageBox.Show($"Не найдены аккаунты Rainbow Sixe Siege в документах", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            List<string> itemCollection = new List<string>();
            SelectedR6SAccount = rainbowSixSiegeAccounts.First().Split("\\").Last();

            foreach (var accountName in rainbowSixSiegeAccounts)
                itemCollection.Add(accountName.Split("\\").Last());

            R6SAccountsCollection = itemCollection;
        }

        public void GetR6SRegion()
        {
            if (String.IsNullOrWhiteSpace(SelectedR6SAccount))
                return;

            R6SGameSettings.R6SAccountGameSettingsFilePath = String.Format("{0}\\{1}\\{2}", R6SGameSettings.R6SAccountsDirectory, SelectedR6SAccount, "GameSettings.ini");

            SelectedR6SRegion = R6SGameSettings.R6SSelectedRegion;
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
