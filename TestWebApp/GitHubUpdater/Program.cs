using GitHubUpdater;
using System.Diagnostics;

internal class Program
{
    private static void Main(string[] args)
    {
        GitUpdater GitUpdater = new GitUpdater();
        while (true)
        {
            Thread.Sleep(1000);
            GitUpdater.Start();
        }
    }
}