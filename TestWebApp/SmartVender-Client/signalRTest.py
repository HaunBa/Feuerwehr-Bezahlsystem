# create a connection
import sys

from requests import Session
from signalr import Connection

url = "http://localhost:5066/"

sys.setrecursionlimit(1000)

session = Session()

connection = Connection(url, session)

# start a connection
connection.start()

# ^ ssl options

# add a handler to process notifications to the connection
connection.handlers += lambda data: print('Connection: new notification.', data)

# get chat hub
VendingHub = connection.hub('VendingHub')


# create new chat message handler
def message_received(message):
    print('Hub: New message.', message)


def eject_item():
    print("Eject Item")


# receive new chat messages from the hub
VendingHub.client.on('EjectItem', eject_item)

# send a new message to the hub
VendingHub.server.invoke('RegisterVendingmachine', 1)

# do not receive new messages
VendingHub.client.off('EjectItem', eject_item)

# close the connection
connection.close()
