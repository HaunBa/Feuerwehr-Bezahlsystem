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

        public async Task Start()
        {
            //while (true)
            //{
            //    Thread.Sleep(60000);
            //}
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
            Process process = new Process();
            process.StartInfo.FileName = "cmd.exe";
            process.StartInfo.Arguments = "git rev-parse origin";
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.RedirectStandardOutput = true;
            process.Start();

            var res = process.StandardOutput.ReadToEnd();
            return res;
        }

        private string GetLocalVersion()
        {
            Process process = new Process();
            process.StartInfo.FileName = "cmd.exe";
            process.StartInfo.Arguments = "git rev-parse @";
            process.StartInfo.RedirectStandardOutput = true;
            process.Start();

            var res = process.StandardOutput.ReadToEnd();
            return res;
        }
    }
}
