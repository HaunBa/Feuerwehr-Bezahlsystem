import time
import json
from signalrcore.hub.base_hub_connection import BaseHubConnection
from signalrcore.hub_connection_builder import HubConnectionBuilder
from typing import Any
from dataclasses import dataclass
# import RPi-GPIO as GPIO
# from mfrc522 import SimpleMFRC522

# reader = SimpleMFRC522()
GPIO = ""

@dataclass
class Root:
    ConfigVersion: str
    VendingMachineNumber: int
    ReconnectInterval: int
    PinSlot1: int
    PinSlot2: int
    PinSlot3: int
    PinSlot4: int
    PinSlot5: int
    PinSlot6: int
    ServerUrl: str
    HubName: str

    @staticmethod
    def from_dict(obj: Any) -> 'Root':
        _ConfigVersion = str(obj.get("ConfigVersion"))
        _VendingMachineNumber = int(obj.get("VendingMachineNumber"))
        _ReconnectInterval = int(obj.get("ReconnectInterval"))
        _PinSlot1 = int(obj.get("PinSlot1"))
        _PinSlot2 = int(obj.get("PinSlot2"))
        _PinSlot3 = int(obj.get("PinSlot3"))
        _PinSlot4 = int(obj.get("PinSlot4"))
        _PinSlot5 = int(obj.get("PinSlot5"))
        _PinSlot6 = int(obj.get("PinSlot6"))
        _ServerUrl = str(obj.get("ServerUrl"))
        _HubName = str(obj.get("HubName"))

        return Root(_ConfigVersion, _VendingMachineNumber, _ReconnectInterval, _PinSlot1, _PinSlot2, _PinSlot3,
                    _PinSlot4, _PinSlot5, _PinSlot6, _ServerUrl, _HubName)


# load config

jsonstring = json.load(open("appsettings.json"))
root = Root.from_dict(jsonstring)

server_url = "wss://" + root.ServerUrl + "/" + root.HubName

mes = ""

# setup GPIO
GPIO.setmode(GPIO.BCM)

GPIO.setup(root.PinSlot1, GPIO.OUT)
GPIO.setup(root.PinSlot2, GPIO.OUT)
GPIO.setup(root.PinSlot3, GPIO.OUT)
GPIO.setup(root.PinSlot4, GPIO.OUT)
GPIO.setup(root.PinSlot5, GPIO.OUT)
GPIO.setup(root.PinSlot6, GPIO.OUT)


GPIO.setup(root.InputSlot1, GPIO.IN)
GPIO.setup(root.InputSlot3, GPIO.IN)
GPIO.setup(root.InputSlot4, GPIO.IN)
GPIO.setup(root.InputSlot5, GPIO.IN)
GPIO.setup(root.InputSlot6, GPIO.IN)
GPIO.setup(root.InputSlot2, GPIO.IN)

def _on_connect():
    hub_connection.send("RegisterVendingmachine", [root.VendingMachineNumber])


# def ReadRFID():
#    while True:
#        print("Hold a tag near the reader")
#        id, text = reader.read()
#        print("ID: %s\nText: %s" % (id, text))
#        time.sleep(5)


def take_message(IncomingVendingItems):
    vending_items = IncomingVendingItems[0]
    for val in vending_items["amount"]:
        for value in vending_items["slot"]:
            time.sleep(1000)
            GPIO.output(value, True)
            time.sleep(5000)
            GPIO.output(value, False)
            time.sleep(1000)


hub_connection: BaseHubConnection = HubConnectionBuilder() \
    .with_url(server_url, options={"verify_ssl": False}) \
    .with_automatic_reconnect({
    "type": "raw",
    "keep_alive_interval": 10,
    "reconnect_interval": 5,
    "max_attempts": 5
}).build()

hub_connection.start()
hub_connection.on_open(_on_connect)
hub_connection.on("EjectItem", take_message)

while True:
    try:
        time.sleep(100)
    except KeyboardInterrupt:
        hub_connection.stop()
        GPIO.cleanup()
        print("disconnected")
        raise
