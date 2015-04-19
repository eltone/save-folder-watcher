#Savegame Archiver
This is a simple windows service that watches a directory for new files and copies them to the configured directory. It was developed to guard against the fact that The Sims 3 had a habit of corrupting saves.

------

##Configuration

Set the directory to watch and the destination directory in the App.config.

##Setup

1. Restore NuGet packages.
2. Build.
3. Install:
 - `SaveFolderWatcher.exe install`