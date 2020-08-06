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
                DataBaseDebug dataBaseDebug = new DataBaseDebug();
                var programDirectory = Directory.GetCurrentDirectory();
                var dataBaseFiles = Directory.GetFiles(programDirectory).Where(x => x.Contains(".db", StringComparison.OrdinalIgnoreCase) == true);
                var dataBaseNewFilePath = String.Format("{0}\\{1}", programDirectory, "DataBase.db");
                var isDataBaseFileNotExist = !File.Exists(dataBaseNewFilePath);

                if (isDataBaseFileNotExist)
                {
                    if (dataBaseFiles.Count() > 0)
                    {
                        var dataBaseFilePath = dataBaseFiles.FirstOrDefault();
                        var dataBaseFileName = dataBaseFiles.Single().Split("\\").Last();

                        File.Move(dataBaseFilePath, dataBaseNewFilePath, true);

                        MessageBox.Show($"Новый файл ({dataBaseFileName}) базы данных, расположенный в папке с программой, был взят и переименован в (DataBase.db).",
                            "Внимание", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    }
                    else if (dataBaseDebug.IsDebugExist())
                    {
                        MessageBox.Show("Файл базы данных отсутсвует, попытка восстановления из резервных копий.", "Внимание", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                        dataBaseDebug.RestoreDataBase(dataBaseNewFilePath);
                    }
                    else
                        throw new Exception("Отсутсвует база данных. Отсутсвуют резервные копии базы данных");
                }
                else
                    dataBaseDebug.DebugDataBase();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при проверки на существование базы данных в папке с программой.\nОшибка: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                Application.Current.Shutdown();
            }
        }

    }
}
