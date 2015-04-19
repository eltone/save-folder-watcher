using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;

namespace SaveFolderWatcher
{
    public class Watcher
    {
        public static FileSystemWatcher watcher = new FileSystemWatcher();
        private static string path = ConfigurationManager.AppSettings["saveDirectory"];
        private static string destination = ConfigurationManager.AppSettings["backupDirectory"];
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        public Watcher()
        {
            watcher.Path = path;
            watcher.Changed += OnChange;
            watcher.Created += OnChange;
            watcher.Renamed += OnChange;
            watcher.Deleted += OnChange;
            watcher.Error += (o, e) => logger.Error("Error thrown by FileSystemWatcher", e.GetException());
            watcher.IncludeSubdirectories = true;
        }

        public void Start()
        {
            watcher.EnableRaisingEvents = true;
            logger.Info("Watching: " + path);
        }

        public void Stop()
        {
            watcher.EnableRaisingEvents = false;
        }

        private void OnChange(object sender, FileSystemEventArgs e)
        {
            if (e.FullPath.Contains(".sims3.backup"))
            {
                watcher.EnableRaisingEvents = false;
                logger.Info("Change detected, disabling listening and copying files");

                // wait 30 seconds for files to finish saving
                System.Threading.Thread.Sleep(10 * 1000);
                CopyFiles(e.FullPath);

                logger.Info("All files copied, re-enabling listener");

                // done copying so enable event watching again
                watcher.EnableRaisingEvents = true;
            }
        }

        private void CopyFiles(string filePath)
        {
            var containingDirPath = Path.GetDirectoryName(filePath);
            var containingDirInfo = new DirectoryInfo(containingDirPath);
            var containingDirName = containingDirInfo.Name;

            var destinationDir = Directory.CreateDirectory(Path.Combine(destination, containingDirName + "." + DateTime.Now.ToString("ddMMyy-HH-mm-ss")));

            foreach (var file in containingDirInfo.EnumerateFiles())
            {
                file.CopyTo(Path.Combine(destinationDir.FullName, file.Name), false);
                logger.Info("Copied " + file.Name);
            }
        }
    }
}
