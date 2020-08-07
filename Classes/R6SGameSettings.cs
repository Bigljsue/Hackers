using Microsoft.EntityFrameworkCore.Internal;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;

namespace WPF_HackersList.Classes
{
    public class R6SGameSettings
    {
        public bool SuccesefulReplaced = false;

        private string R6SAccountsDirectory 
        { 
            get 
            { 
                return String.Format("{0}\\{1}\\{2}", Environment.GetFolderPath(Environment.SpecialFolder.Personal), "My Games", "Rainbow Six - Siege"); 
            } 
        }

        public string GetGameSettingsText(string gameSettingsFilePath)
        {
            string fileText = null;

            using (StreamReader streamReader = new StreamReader(gameSettingsFilePath))
                fileText = streamReader.ReadToEnd();

            return fileText;
        }

        public List<string> GetAccountsNames()
        {
            var accountsNames = new List<string>();
            var accountsPaths = GetAccountsPaths();

            foreach (string accountName in accountsPaths)
                accountsNames.Add(accountName.Split("\\").Last());

            return accountsNames;
        }

        public List<string> GetAccountsPaths()
        {
            var accountsPaths = Directory.GetDirectories(R6SAccountsDirectory).ToList();
            return accountsPaths;
        }

        public string GetAccountPath(string accountName)
        {
            var accountsPaths = GetAccountsPaths();
            var accountPath = accountsPaths.Where(x => x.Contains(accountName) == true).SingleOrDefault();
            return accountPath;
        }

        public string GetAccountGameSettingsFilePath(string accountName)
        {
            var accountPath = GetAccountPath(accountName);
            var accountGameSettingsPath = string.Format("{0}\\{1}", accountPath, "GameSettings.ini");
            return accountGameSettingsPath;
        }

        private string GetRowWithRegion(string gameSettingsText)
        {
            var gameSettingTextSplited = gameSettingsText.Split("\n");
            var rowWithRegion = gameSettingTextSplited.Where(x => x.Contains("DataCenterHint=") == true).SingleOrDefault();

            return rowWithRegion;
        }

        public string GetAccountRegion(string accountName)
        {
            var accountGameSettingsPath = GetAccountGameSettingsFilePath(accountName);
            var gameSettingsTextSplited = GetGameSettingsText(accountGameSettingsPath).Split("\n");
            var accountSelectedRegion = gameSettingsTextSplited.Where(x => x.Contains("DataCenterHint=") == true).SingleOrDefault().Split("=")[1].Trim();

            return accountSelectedRegion;
        }

        public void SetNewRegion(string accountName, string regionName)
        {
            try
            {
                var accountGameSettingsPath = GetAccountGameSettingsFilePath(accountName);
                var gameSettingsTextOld = GetGameSettingsText(accountGameSettingsPath);
                var gameSettingsTextNew = gameSettingsTextOld.Replace(GetRowWithRegion(gameSettingsTextOld), "DataCenterHint=" + regionName + "\r");

                using (StreamWriter streamWriter = new StreamWriter(accountGameSettingsPath))
                    streamWriter.Write(gameSettingsTextNew);

                SuccesefulReplaced = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка изменения региона для аккаунта. Текст ошибки:\n{ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

    }
}
