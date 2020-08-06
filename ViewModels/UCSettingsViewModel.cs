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

        //////// Rainbow Six Siege start
        
        private string R6SAccountsDirectory;
        private int IndexOfDataCenterHintInRow;
        private string[] R6SGameSettingsSplited;
        private string R6SGameSettingsPath;

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

        public async void SetR6SRegion()
        {
            R6SGameSettingsSplited[IndexOfDataCenterHintInRow] = String.Format("{0}={1}\r", "DataCenterHint", SelectedR6SRegion);
            string gameSettingsFileText = null;

            foreach (var text in R6SGameSettingsSplited)
                gameSettingsFileText += text;

            using (StreamWriter streamWriter = new StreamWriter(R6SGameSettingsPath))            
                await streamWriter.WriteAsync(gameSettingsFileText);
            

            MessageBox.Show($"Замена региона прошла успешно на {SelectedR6SRegion}", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        public void GetR6SData()
        {
            var documents = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            R6SAccountsDirectory = String.Format("{0}\\{1}\\{2}", documents, "My Games", "Rainbow Six - Siege");
            var rainbowSixSiegeAccounts = Directory.GetDirectories(R6SAccountsDirectory);
            List<string> itemCollection = new List<string>();
            SelectedR6SAccount = rainbowSixSiegeAccounts.First().Split("\\").Last();

            foreach (var accountName in rainbowSixSiegeAccounts)
                itemCollection.Add(accountName.Split("\\").Last());

            R6SAccountsCollection = itemCollection;
        }

        public async void GetR6SRegion()
        {
            R6SGameSettingsPath = String.Format("{0}\\{1}\\{2}", R6SAccountsDirectory, SelectedR6SAccount, "GameSettings.ini");
            string gameSettingsContent = null;

            if (String.IsNullOrWhiteSpace(SelectedR6SAccount))
                return;
            using(StreamReader streamReader = new StreamReader(R6SGameSettingsPath))            
                gameSettingsContent = await streamReader.ReadToEndAsync();

            R6SGameSettingsSplited = gameSettingsContent.Split("\n");
            var gameSettingsNeededRow = R6SGameSettingsSplited.Where(x => x.Contains("DataCenterHint=") == true).SingleOrDefault();

            IndexOfDataCenterHintInRow = R6SGameSettingsSplited.IndexOf(gameSettingsNeededRow);
            SelectedR6SRegion = gameSettingsNeededRow.Split("=")[1].Trim();
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
