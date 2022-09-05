using System.Collections.Generic;
using TestingApp.ViewModels;

internal class Program
{
    public static int pinSlot1;
    public static int pinSlot2;
    public static int pinSlot3;
    public static int pinSlot4;
    public static int pinSlot5;
    public static int pinSlot6;

    //public static GpioController Controller { get; set; }

    private static void Main(string[] args)
    {
        #region Configuration

        Task.Delay(1000);

        var configuration = new ConfigurationBuilder()
        .SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile($"appsettings.json");

        var config = configuration.Build();

        var version = config.GetRequiredSection("Version").Get<VersionSettings>();
        
        var configVersion = version.ConfigVersion;

        var settings = config.GetRequiredSection("Settings").Get<Settings>();
        var machineNumber = settings.VendingMachineNumber;
        var serverUrl = settings.ServerUrl ?? "https://localhost:7066";
        var ReconnectInterval = settings.ReconnectInterval;
        pinSlot1 = settings.PinSlot1;
        pinSlot2 = settings.PinSlot2;
        pinSlot3 = settings.PinSlot3;
        pinSlot4 = settings.PinSlot4;
        pinSlot5 = settings.PinSlot5;
        pinSlot6 = settings.PinSlot6;

        #endregion

        #region Setup SignalR

        var connection = new HubConnectionBuilder()
            .WithAutomaticReconnect()
            .WithUrl($"{serverUrl}/VendingHub")
            .Build();

        

        connection.Closed += async (error) =>
        {
            await Task.Delay(ReconnectInterval);
            Console.WriteLine("Reconnecting...");
            await connection.StartAsync();
        };

        connection.On<List<VendingItems>> ("EjectItem", (vendingItems) =>
        {
            Console.WriteLine("Recieved Vendingitems");
            EjectItem(vendingItems);
        });
        
        connection.StartAsync().GetAwaiter().GetResult();

        connection.InvokeAsync("RegisterVendingmachine", machineNumber).GetAwaiter().GetResult();

        Console.WriteLine(connection.State);

        #endregion

        #region Setup GPIO

        //Controller = new GpioController();

        //Controller.OpenPin(pinSlot1, PinMode.Output);
        //Controller.OpenPin(pinSlot2, PinMode.Output);
        //Controller.OpenPin(pinSlot3, PinMode.Output);
        //Controller.OpenPin(pinSlot4, PinMode.Output);
        //Controller.OpenPin(pinSlot5, PinMode.Output);
        //Controller.OpenPin(pinSlot6, PinMode.Output);

        Console.WriteLine($"Vendingmachine number: {machineNumber}");

        #endregion

        Console.ReadKey();
    }

    private static void EjectItem(List<VendingItems> vendingItems)
    {
        foreach(var item in vendingItems)
        {
            int selectedPinSlot;
            switch (item.Slot)
            {
                case 1:
                    selectedPinSlot = pinSlot2;
                    break;
                case 2:
                    selectedPinSlot = pinSlot2;
                    break;
                case 3:
                    selectedPinSlot = pinSlot3;
                    break;
                case 4:
                    selectedPinSlot = pinSlot4;
                    break;
                case 5:
                    selectedPinSlot = pinSlot5;
                    break;
                case 6:
                    selectedPinSlot = pinSlot6;
                    break;

                default:
                    return;
            }

            for (int i = 0; i < item.Amount; i++)
            {
                //Controller.Write(selectedPinSlot, PinValue.High);
                Console.WriteLine($"{selectedPinSlot} set to High.");

                Task.Delay(1000);

                //Controller.Write(selectedPinSlot, PinValue.Low);
                Console.WriteLine($"{selectedPinSlot} set to Low.");
                Task.Delay(5000);
            }
        }
    }
}