using AutomaticWifiConnector.Classes;
using Microsoft.Extensions.Configuration;

// read config values
var configuration = new ConfigurationBuilder()
                        .SetBasePath(Directory.GetCurrentDirectory())
                        .AddJsonFile($"WifiConnectionSettings.json");

var config = configuration.Build();

var version = config.GetRequiredSection("Settings")
                    .Get<Settings>();

// start shell to connect to wifi
System.Diagnostics.Process setupProcess = new System.Diagnostics.Process();
System.Diagnostics.ProcessStartInfo setupstartInfo = new()
{
    WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden,
    FileName = "cmd.exe",
    Arguments = $"netsh wlan set hostednetwork mode=allow ssid={version.WifiName} key={version.WifiPassword}"
};
setupProcess.StartInfo = setupstartInfo;
setupProcess.Start();

setupProcess.WaitForExit(10000);

// connect to wifi
System.Diagnostics.Process process = new System.Diagnostics.Process();
System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
startInfo.FileName = "cmd.exe";
startInfo.Arguments = $"netsh wlan start hostednetwork";
process.StartInfo = startInfo;
process.Start();

setupProcess.WaitForExit(10000);

// start SmartVender and restart after exiting
while (true)
{
    System.Diagnostics.Process smartVenderProcess = new ();
    System.Diagnostics.ProcessStartInfo smartVenderStartInfo = new()
    {
        FileName = "SmartVender.exe"
    };
    smartVenderProcess.StartInfo = smartVenderStartInfo;
    smartVenderProcess.Start();

    smartVenderProcess.WaitForExit();
}