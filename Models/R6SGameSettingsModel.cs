using Microsoft.EntityFrameworkCore.Internal;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace WPF_HackersList.Models
{
    public class R6SGameSettingsModel
    {
        public string R6SAccountsDirectory { get { return String.Format("{0}\\{1}\\{2}", GamesDocumentsDirectory, "My Games", "Rainbow Six - Siege"); } }

        public string GamesDocumentsDirectory { get { return Environment.GetFolderPath(Environment.SpecialFolder.Personal); } }

        public string R6SSelectedRegion { get; set; }

        public int R6SIndexOfDataCenterHintInRow { get; private set; }

        private string _r6saccountgamesettingsfilepath;
        public string R6SAccountGameSettingsFilePath 
        {
            get
            {
                return _r6saccountgamesettingsfilepath;
            }
            set
            {
                _r6saccountgamesettingsfilepath = value;

                using (StreamReader streamReader = new StreamReader(value))
                    R6SGameSettingsFileText = streamReader.ReadToEnd();
            }
        }

        private string _r6sgamesettingsfiletext;
        public string R6SGameSettingsFileText 
        { 
            get 
            { 
                return _r6sgamesettingsfiletext; 
            } 
            private set 
            { 
                _r6sgamesettingsfiletext = value;
                R6SGameSettingsFileTextSplited = _r6sgamesettingsfiletext.Split("\n");
            } 
        }

        private string _r6sdatacenterhinttextinrow;
        public string R6SDataCenterHintTextInRow
        {
            get 
            { 
                return _r6sdatacenterhinttextinrow; 
            }
            private set 
            {
                _r6sdatacenterhinttextinrow = value;
                R6SSelectedRegion = R6SDataCenterHintTextInRow.Split("=")[1].Trim();
            }
        }

        private string[] _r6sgamesettingsfiletextsplited;
        public string[] R6SGameSettingsFileTextSplited 
        { 
            get 
            {
                return _r6sgamesettingsfiletextsplited;
            } 
            private set 
            {
                _r6sgamesettingsfiletextsplited = value;
                R6SDataCenterHintTextInRow = value.Where(x => x.Contains("DataCenterHint=") == true).SingleOrDefault();
                R6SIndexOfDataCenterHintInRow = value.IndexOf(R6SDataCenterHintTextInRow);
            }
        }
    }
}
