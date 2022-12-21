#imports

import json;
from typing import Any
from dataclasses import dataclass
from requests import Session
from signalr import Connection

# Classes

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
        return Root(_ConfigVersion, _VendingMachineNumber, _ReconnectInterval, _PinSlot1, _PinSlot2, _PinSlot3, _PinSlot4, _PinSlot5, _PinSlot6, _ServerUrl)


# load config

jsonstring = json.load(open("appsettings.json"))
root = Root.from_dict(jsonstring)

# connect to signalR

with Session() as session:
    #create connection
    connection = Connection(root.ServerUrl, session)

    #get hub
    hub = connection.register_hub('vendingHub')

    connection.start()

    # ejectItem handler
    def eject_recievedItem(data):
        print('received: ', data)

    # error handler
    def print_error_to_console(error):
        print('error: ', error)

    # register methods
    hub.client.on("EjectItem", eject_recievedItem)

    connection.error += print_error_to_console

    # start connection
    with connection:

        # register vending machine
        hub.server.invoke("RegisterVendingmachine", root.VendingMachineNumber)

        # testcall
        hub.server.invoke("")