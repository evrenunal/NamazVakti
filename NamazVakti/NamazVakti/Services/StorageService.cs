using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Xamarin.Essentials;

namespace NamazVakti.Services
{
    public class StorageService
    {
        public StorageService()
        {

        }

        internal (bool,string) GetFile(string thisMonthId)
        {
            var mainDir = FileSystem.AppDataDirectory;


            var filePath = Path.Combine(mainDir, thisMonthId);

            if (!File.Exists(filePath))
                return (false, "");

            var fileContent = File.ReadAllText(filePath);

            return (true, fileContent);
        }
    }
}
