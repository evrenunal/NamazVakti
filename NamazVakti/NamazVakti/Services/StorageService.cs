using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Xamarin.Essentials;

namespace NamazVakti.Services
{
    public class StorageService
    {
        private string mainDir;

        public StorageService()
        {
            mainDir = FileSystem.AppDataDirectory;
        }

        internal (bool, string) GetFile(string thisMonthId)
        {
            var filePath = Path.Combine(mainDir, thisMonthId);

            if (!File.Exists(filePath))
                return (false, "");

            var fileContent = File.ReadAllText(filePath);

            return (true, fileContent);
        }

        internal void SaveFile(string fileToSave, string thisMonthId)
        {
            var filePath = Path.Combine(mainDir, thisMonthId);
            File.WriteAllText(filePath, fileToSave);
        }

        internal void DeleteFile(string lastMonthId)
        {
            var filePath = Path.Combine(mainDir, lastMonthId);
            File.Delete(filePath);
        }
    }
}
