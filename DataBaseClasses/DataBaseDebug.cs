using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace WPF_HackersList.DataBaseClasses
{
    public class DataBaseDebug
    {
        public delegate void MethodsOnCopying(string message, bool state);
        public event MethodsOnCopying onComplete = delegate { };

        public string FileName = "DataBase";

        public string FileToDebug = "DataBase.db";

        /// <summary>
        /// Влючает в конце путя \
        /// </summary>
        public string AppTempFolder = String.Format("{0}{1}", Path.GetTempPath(), "Hackers\\");

        private void DeleteLastDataBaseDebug()
        {
            var lastFile = AppTempFolder + GetDataBaseDebugsSorted().LastOrDefault();

            if (!File.Exists(lastFile))
                return;

            File.Delete(lastFile);
        }

        public bool IsDebugExist()
        {           
            if (!Directory.Exists(AppTempFolder))
                return false;

            var dataBaseDebugFiles = Directory.GetFiles(AppTempFolder);

            if (dataBaseDebugFiles.Count() < 1)
                return false;

            return true;
        }

        public void RestoreDataBase(string dataBaseToRestore)
        {
            try
            {
                var dataBaseLastDebug = AppTempFolder + GetDataBaseDebugsSorted().FirstOrDefault();

                if (!File.Exists(dataBaseLastDebug))
                {
                    onComplete("Дубликатов баз данных нету.", false);
                    throw new Exception("Дубликатов баз данных нету.");
                }

                if (String.IsNullOrWhiteSpace(dataBaseToRestore))
                {
                    dataBaseToRestore = String.Format("{0}\\{1}", Directory.GetCurrentDirectory(),"DataBase.db");

                    File.Copy(dataBaseLastDebug, dataBaseToRestore);
                    return;
                }

                File.Copy(dataBaseLastDebug, dataBaseToRestore);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async void DebugDataBase()
        {
            try
            {
                if (!File.Exists(FileToDebug))
                    throw new Exception("Файла базы данных нету");

                if (GetDataBaseDebugsSorted() != null)
                    if (GetDataBaseDebugsSorted().Count() > 19)
                        DeleteLastDataBaseDebug();

                var newFileSavePath = AppTempFolder + GetRenamedDataBase();

                var bufferLength = 10 * 1024 * 8;
                var fileBuffer = new byte[bufferLength];


                await using (var fileStreamReader = new FileStream(FileToDebug, FileMode.Open, FileAccess.Read))
                {
                    long fileToDebugSize = fileStreamReader.Length;

                    using (var fileStreamWriter = new FileStream(newFileSavePath, FileMode.Create, FileAccess.Write))
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
                onComplete("База дынных скопирована успешно", true);
            }
            catch (Exception e)
            {
                onComplete(String.Format("Ошибка копирования: {0}", e.Message), false);
            }
        }

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

        private string GetRenamedDataBase()
        {
            return String.Format("{0}_DataBase_{1}_{2}_{3}_{4}.db", GetLastSavedDataBaseId() + 1, DateTime.Now.Day, DateTime.Now.Month, DateTime.Now.Year, DateTime.Now.Hour);
        }
    }
}
