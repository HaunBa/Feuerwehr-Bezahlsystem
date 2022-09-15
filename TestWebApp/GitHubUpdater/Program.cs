using GitHubUpdater;
using System.Diagnostics;

//GitUpdater GitUpdater = new GitUpdater();
//await GitUpdater.Start();

var thisProcess = Process.GetCurrentProcess();

var process = new Process{
        StartInfo = new ProcessStartInfo
        {
            FileName = typeof(Program).Assembly.GetName().Name + ".exe",
            UseShellExecute = false,
            RedirectStandardOutput = true,
            CreateNoWindow = false
        }
    };

var processName = process.StartInfo.FileName;

process.Start();

Console.WriteLine("other process started");

Thread.Sleep(1000);

Console.WriteLine("Killing this Process...");
thisProcess.Kill();
thisProcess.Close();

Console.WriteLine("Killed?");
