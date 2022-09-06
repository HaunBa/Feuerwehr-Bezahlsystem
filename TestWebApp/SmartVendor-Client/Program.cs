using Microsoft.AspNetCore.SignalR;
using SmartVendor.Classes;
using TestingApp.ViewModels;

var builder = WebApplication.CreateBuilder(args);

int pinSlot1;
int pinSlot2;
int pinSlot3;
int pinSlot4;
int pinSlot5;
int pinSlot6;

// Add services to the container.
builder.Services.AddRazorPages();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.Run();

Task.Delay(3000);

var configuration = new ConfigurationBuilder()
.SetBasePath(Directory.GetCurrentDirectory())
.AddJsonFile($"appsettings.json");

#region Config
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

connection.On<List<VendingItems>>("EjectItem", (vendingItems) =>
{
    Console.WriteLine("Recieved Vendingitems");
    EjectItem(vendingItems);
});

connection.StartAsync().GetAwaiter().GetResult();

connection.InvokeAsync("RegisterVendingmachine", machineNumber);

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
    

    Task EjectItem(List<VendingItems> vendingItems)
{
    foreach (var item in vendingItems)
    {
        int selectedPinSlot;
        switch (item.Slot)
        {
            case 1:
                selectedPinSlot = pinSlot1;
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
                return Task.CompletedTask;
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
    return Task.CompletedTask;
}