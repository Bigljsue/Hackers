using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;

namespace WPF_HackersList.DataBaseClasses
{
    public class DataBaseDebug
    {
        /// <summary>
        /// Влючает в конце путя \
        /// </summary>
        private string AppTempFolder = String.Format("{0}{1}", Path.GetTempPath(), "Hackers\\");

        private string DataBaseFilePath = String.Format("{0}\\{1}",Directory.GetCurrentDirectory(),"DataBase.db");

        public void RestoreDataBase()
        {
            try
            {
                var programDirectory = Directory.GetCurrentDirectory();
                var dataBaseFiles = Directory.GetFiles(programDirectory).Where(x => x.Contains(".db", StringComparison.OrdinalIgnoreCase) == true);

                if (dataBaseFiles.Count() > 0)
                {
                    var dataBaseFilePath = dataBaseFiles.FirstOrDefault();
                    var dataBaseFileName = dataBaseFiles.Single().Split("\\").Last();

                    File.Move(dataBaseFilePath, DataBaseFilePath, true);

                    MessageBox.Show($"Новый файл ({dataBaseFileName}) базы данных, расположенный в папке с программой, был взят и переименован в (DataBase.db).",
                        "Внимание", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                }
                else if (IsDebugExist())
                {
                    MessageBox.Show("Файл базы данных отсутсвует, попытка восстановления из резервных копий.", "Внимание", MessageBoxButton.OK, MessageBoxImage.Exclamation);

                    var dataBaseLastDebug = AppTempFolder + GetDataBaseDebugsSorted().FirstOrDefault();

                    File.Copy(dataBaseLastDebug, DataBaseFilePath, true);
                }
                else
                    throw new Exception("Отсутсвует база данных. Отсутсвуют резервные копии базы данных");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при проверки на существование базы данных в папке с программой.Приложение закроется.\nОшибка: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                Application.Current.Shutdown();
            }
        }

        public async void DebugDataBase()
        {
            try
            {
                if (IsDebugExist())
                    if (GetDataBaseDebugsSorted().Count() > 19)
                        DeleteLastDataBaseDebug();

                var dataBaseDebuFilePath = AppTempFolder + GetRenamedDataBase();

                var bufferLength = 10 * 1024 * 8;
                var fileBuffer = new byte[bufferLength];


                await using (var fileStreamReader = new FileStream(DataBaseFilePath, FileMode.Open, FileAccess.Read))
                {
                    long fileToDebugSize = fileStreamReader.Length;

                    using (var fileStreamWriter = new FileStream(dataBaseDebuFilePath, FileMode.Create, FileAccess.Write))
                        while (true)
                        {
                            int bytesRead = fileStreamReader.Read(fileBuffer, 0, bufferLength);

                            if (bytesRead == 0)
                                break;

                            fileStreamWriter.Write(fileBuffer, 0, bytesRead);

                            if (bytesRead < bufferLength)
                                break;
                        }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show($"Ошибка копирования базы данных.\n{e.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public bool IsDataBaseExist() => File.Exists(DataBaseFilePath);

        private int GetLastSavedDataBaseId()
        {
            if (GetDataBaseDebugsSorted() == null)
                return 0;

            return Convert.ToInt32(GetDataBaseDebugsSorted().LastOrDefault().Split("_")[0]);
        }

        private IEnumerable<string> GetDataBaseDebugsSorted()
        {
            if (!Directory.Exists(AppTempFolder))
                Directory.CreateDirectory(AppTempFolder);

            var dataBaseDebugs = Directory.GetFiles(AppTempFolder, "*.db").ToList();

            if (dataBaseDebugs.Count() < 1)
                return null;

            var dataBaseDebugsNames = new List<string>();

            foreach(var dataBaseDebug in dataBaseDebugs)
                dataBaseDebugsNames.Add(dataBaseDebug.Split("\\").First(x => x.EndsWith(".db")));                

            var dataBaseDebugsNamesSorted = dataBaseDebugsNames.OrderBy(x => Convert.ToInt32(x.Split("_")[0])).Select(x => x);

            return dataBaseDebugsNamesSorted;
        }

        private bool IsDebugExist()
        {
            var isDebugDirectoryNotExist = !Directory.Exists(AppTempFolder);

            if (isDebugDirectoryNotExist)
                return false;

            var dataBaseDebugFiles = Directory.GetFiles(AppTempFolder);

            if (dataBaseDebugFiles.Count() < 1)
                return false;

            return true;
        }    

        private void DeleteLastDataBaseDebug()
        {
            var lastFile = AppTempFolder + GetDataBaseDebugsSorted().LastOrDefault();

            if (!File.Exists(lastFile))
                return;

            File.Delete(lastFile);
        }

        private string GetRenamedDataBase()
        {
            return String.Format("{0}_DataBase_{1}_{2}_{3}_{4}.db", GetLastSavedDataBaseId() + 1, DateTime.Now.Day, DateTime.Now.Month, DateTime.Now.Year, DateTime.Now.Hour);
        }
    }
}
