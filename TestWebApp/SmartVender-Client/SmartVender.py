#imports

import json
from typing import Any
from dataclasses import dataclass
import websockets
import requests
import asyncio

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

        return Root(_ConfigVersion, _VendingMachineNumber, _ReconnectInterval, _PinSlot1, _PinSlot2, _PinSlot3, _PinSlot4, _PinSlot5, _PinSlot6, _ServerUrl, _HubName)


# load config

jsonstring = json.load(open("appsettings.json"))
root = Root.from_dict(jsonstring)

# connect to signalR

negotiation = requests.post('https://localhost:7066/VendingHub/negotiateVersion=0').json()
def toSignalRMessage(data):
    return f'{json.dumps(data)}\u001e'

async def connectToHub(connectionId):
    uri = f"wss://localhost:7066/{root.HubName}"
    async with websockets.connect(uri) as websocket:

        async def start_pinging():
            while _running:
                await asyncio.sleep(10)
                await websocket.send(toSignalRMessage({"type": 6}))

        async def handshake():
            await websocket.send(toSignalRMessage({"protocol": "json", "version": 1}))
            handshake_response = await websocket.recv()
            print(f"handshake_response: {handshake_response}")

        async def listen():
            while _running:
                get_response = await websocket.recv()
                print(f"get_response: {get_response}")

        await handshake()

        _running = True
        listen_task = asyncio.create_task(listen())
        ping_task = asyncio.create_task(start_pinging())

        # start
        start_message = {
            "type": 1,
            "invocationId": "invocation_id",
            "target": "RegisterVendingmachine",
            "arguments": [
                root.VendingMachineNumber
            ],
            "streamIds": [
                "stream_id"
            ]
        }

        await websocket.send(toSignalRMessage(start_message))
        # send

        message = {
            "type": 2,
            "invocationId": "stream_id",
            "item": f'{root.VendingMachineNumber}'
        }
        await websocket.send(toSignalRMessage(message))
        await asyncio.sleep(2)

        # end

        completion_message = {
            "type": 3,
            "invocationId": "stream_id"
        }

        await websocket.send(toSignalRMessage(completion_message))
        await asyncio.sleep(2)

        _running = False
        await ping_task

        print(f"connectionId: {negotiation['connectionId']}")
        asyncio.run(connectToHub(negotiation['connectionId']))

        while (True):
            await listen_task
