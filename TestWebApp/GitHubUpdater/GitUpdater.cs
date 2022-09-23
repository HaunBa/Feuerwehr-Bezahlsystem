using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GitHubUpdater
{
    public class GitUpdater
    {
        public Process process = new Process();

        public void Start()
        {
            CheckForUpdate();
        }

        public void CheckForUpdate()
        {
            var remoteVersion = GetRemoteVersion();
            Console.WriteLine("remoteVersion: " + remoteVersion);

            var localVersion = GetLocalVersion();
            Console.WriteLine("localVersion: "+ localVersion);
        }

        private string GetRemoteVersion()
        {
            var p = new Process();
            var startInfo = new ProcessStartInfo("cmd.exe")
            {
                RedirectStandardInput = true,
                RedirectStandardOutput = true,
                UseShellExecute = false
            };
            p.StartInfo = startInfo;
            p.Start();
            StreamReader stdInputReader = p.StandardOutput;
            StreamWriter stdInputWriter = p.StandardInput;

            stdInputWriter.WriteLine("git rev-parse origin");
            Thread.Sleep(100);
            var res = stdInputReader.ReadToEnd();
            //ProcessStartInfo info = new("cmd", "/K git rev-parse origin");            
            
            //Process.Start(info);

            //var res = process.StandardOutput.ReadToEnd();

            return res;
        }

        private string GetLocalVersion()
        {
            ProcessStartInfo info = new("cmd", "/K git rev-parse @");
            Process.Start(info);

            var res = process.StandardOutput.ReadToEnd();

            return res;
        }
    }
}
