using AutomaticWifiConnector.Classes;
using Microsoft.Extensions.Configuration;
using System.Diagnostics;
using System.Globalization;
using System.Net;

// read config values
var configuration = new ConfigurationBuilder()
                        .SetBasePath(Directory.GetCurrentDirectory())
                        .AddJsonFile($"WifiConnectionSettings.json");

var config = configuration.Build();

var version = config.GetRequiredSection("Settings")
                    .Get<Settings>();

if (!CheckForInternetConnection())
{
    // start shell to connect to wifi
    Process setupProcess = new Process();
    ProcessStartInfo setupstartInfo = new()
    {
        WindowStyle = ProcessWindowStyle.Hidden,
        FileName = "cmd.exe",
        Arguments = $"netsh wlan set hostednetwork mode=allow ssid={version.WifiName} key={version.WifiPassword}"
    };
    setupProcess.StartInfo = setupstartInfo;
    setupProcess.Start();

    setupProcess.WaitForExit(10000);

    // connect to wifi
    Process process = new();
    ProcessStartInfo startInfo = new ();
    startInfo.WindowStyle = ProcessWindowStyle.Hidden;
    startInfo.FileName = "cmd.exe";
    startInfo.Arguments = $"netsh wlan start hostednetwork";
    process.StartInfo = startInfo;
    process.Start();

    setupProcess.WaitForExit(10000);
}
var processes = Process.GetProcessesByName(version.ProcessName);

if (processes.Length != 0)
{
    foreach (var item in processes)
    {
        item.Kill();
    }
}

// start SmartVender and restart after exiting
while (true)
{
    Process smartVenderProcess = new();
    ProcessStartInfo smartVenderStartInfo = new()
    {
        FileName = version.ProcessName
    };
    smartVenderProcess.StartInfo = smartVenderStartInfo;
    smartVenderProcess.Start();

    smartVenderProcess.WaitForExit();
}

static bool CheckForInternetConnection(int timeoutMs = 10000)
{
    try
    {        
        string url = "https://www.google.com";

        var timeOut = TimeSpan.FromSeconds(timeoutMs);

        var client = new HttpClient
        {
            BaseAddress = new Uri(url),            
            Timeout = timeOut
        };
        using var response = client.Send(new HttpRequestMessage());
        return true;
    }
    catch
    {
        return false;
    }
}