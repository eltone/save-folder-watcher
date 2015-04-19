using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Topshelf;

namespace SaveFolderWatcher
{
    class Program
    {
        static void Main(string[] args)
        {
            HostFactory.Run(x =>
            {
                x.Service<Watcher>(s =>
                {
                    s.ConstructUsing(name => new Watcher());
                    s.WhenStarted(tc => tc.Start());
                    s.WhenStopped(tc => tc.Stop());
                });

                x.RunAsLocalSystem();

                x.SetDescription("Sims 3 save game backup service");
                x.SetDisplayName("Sims3Archiver");
                x.SetServiceName("sim3archiver");

                x.UseNLog();
            });
        }
    }
}
