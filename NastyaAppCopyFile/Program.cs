using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Threading;

namespace NastyaAppCopyFile
{
    class Program
    {
        private static string filePath { get; set; }
        private static string CopyPath { get; set; }

        static void Main(string[] args)
        {
            //Get Incoming File
            filePath = ConfigurationManager.AppSettings["TargetPath"];
            CopyPath = ConfigurationManager.AppSettings["CopyPath"];

            if (!Directory.Exists(filePath))
            {
                Console.WriteLine("Directory in config file does not exists");
                Console.WriteLine(filePath);
                return;
            }

            FileSystemWatcher watcher = new FileSystemWatcher();
            watcher.Path = filePath;
            watcher.NotifyFilter = NotifyFilters.LastWrite;
            watcher.Filter = "*.*";
            watcher.Changed += Watcher_Changed;
            watcher.EnableRaisingEvents = true;

            while(true)
            {
                Thread.Sleep(1000);
                Console.WriteLine("...");
            }
        }

        private static void Watcher_Changed(object sender, FileSystemEventArgs e)
        {
            if (!Directory.Exists(CopyPath))
            {
                Console.WriteLine("Directory in config file does not exists");
                Console.WriteLine(CopyPath);
                return;
            }

            if (File.Exists(e.FullPath))
            {
                var copyFilePath = Path.Combine(CopyPath, Path.GetFileName(e.FullPath));

                if (File.Exists(copyFilePath))
                {
                    File.Delete(copyFilePath);
                }

                File.Copy(e.FullPath, copyFilePath);
            }
            //throw new NotImplementedException();
        }
    }
}
