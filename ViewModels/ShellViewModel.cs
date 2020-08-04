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
        private bool FirstLoad = true;

        public ShellViewModel()
        {
            ActivateItem(new UCHackersListViewModel());
            CheckForDataBase();
        }

        public void LoadHackersListPage()
        {
            ActivateItem(new UCHackersListViewModel());
            CheckForDataBase();
        }

        public void LoadSettingsPage()
        {
            ActivateItem(new UCSettingsViewModel());
        }

        public void CheckForDataBase()
        {
            if (FirstLoad == false)
                return;
            FirstLoad = false;

            try
            {
                string programDirectory = Directory.GetCurrentDirectory();
                var dataBaseFile = String.Format("{0}\\{1}", Directory.GetCurrentDirectory(), "DataBase.db");

                DataBaseDebug dataBaseDebug = new DataBaseDebug();

                if (File.Exists(dataBaseFile))
                {
                    dataBaseDebug.DebugDataBase();
                    return;
                }
                else if (dataBaseDebug.IsDebugExist())
                {
                    MessageBox.Show("Файл базы данных отсутсвует, попытка восстановления.");
                    dataBaseDebug.RestoreDataBase();
                }
                else
                {
                    var directoryFiles = Directory.GetFiles(programDirectory);

                    var dataBaseFiles = directoryFiles.Where(x => x.Contains(".db", StringComparison.OrdinalIgnoreCase) == true).Select(x => x);
                    if (dataBaseFiles.Count() == 0)
                        throw new Exception("Копии файла базы даных нету, файл должен поступать вместе с программой и называтся \"DataBase.db\"." +
                            "Для работы программы файл необходимо скачать и переместить в папку с программой");

                    string dataBaseFilePath = dataBaseFiles.FirstOrDefault();

                    var newFilePath = String.Format("{0}\\{1}", Directory.GetCurrentDirectory(), "DataBase.db");

                    File.Move(dataBaseFilePath, newFilePath, true);
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

    }
}
