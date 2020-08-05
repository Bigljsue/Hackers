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
            try
            {
                string programDirectory = Directory.GetCurrentDirectory();
                var dataBaseFile = Directory.GetFiles(programDirectory).ToList().Where(x => x.Contains(".db", StringComparison.OrdinalIgnoreCase) == true).FirstOrDefault();

                DataBaseDebug dataBaseDebug = new DataBaseDebug();

                if (!String.IsNullOrWhiteSpace(dataBaseFile))
                {
                    dataBaseDebug.DebugDataBase();
                    return;
                }
                else if (dataBaseDebug.IsDebugExist())
                {
                    MessageBox.Show("Файл базы данных отсутсвует, попытка восстановления.", "Внимание", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    dataBaseDebug.RestoreDataBase(dataBaseFile);
                }
                else
                {
                    var directoryFiles = Directory.GetFiles(programDirectory);

                    var dataBaseFiles = directoryFiles.Where(x => x.Contains(".db", StringComparison.OrdinalIgnoreCase) == true).Select(x => x);
                    if (dataBaseFiles.Count() == 0)
                        throw new Exception("Копии файла базы даных нету, файл должен поступать вместе с программой и называтся \"DataBase.db\"." +
                            "Для работы программы файл необходимо скачать и переместить в папку с программой.");

                    string dataBaseFilePath = dataBaseFiles.FirstOrDefault();

                    var dataBaseNewFilePath = String.Format("{0}\\{1}", Directory.GetCurrentDirectory(), "DataBase.db");

                    File.Move(dataBaseFilePath, dataBaseNewFilePath, true);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка проверки базы данных.\n{ex.Message}","Ошибка",MessageBoxButton.OK,MessageBoxImage.Error);
                Application.Current.Shutdown();
            }
        }

    }
}
