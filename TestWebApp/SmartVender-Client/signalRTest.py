#create a connection
from requests import Session
from signalr-client import Connection

url = "https://localhost:7066/VendingHub"

session = Session()

connection = Connection(url, session)

#start a connection
connection.start()

#add a handler to process notifications to the connection
connection.handlers += lambda data: print ('Connection: new notification.', data)

#get chat hub
chat_hub = connection.hub('chat')

#create new chat message handler
def message_received(message):
    print ('Hub: New message.', message)

#receive new chat messages from the hub
chat_hub.client.on('message_received', message_received)

#send a new message to the hub
chat_hub.server.invoke('send_message', 'Hello!')

#do not receive new messages
chat_hub.client.off('message_received', message_received)

#close the connection
connection.close()