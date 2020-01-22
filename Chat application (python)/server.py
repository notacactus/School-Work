# ============================================================================================
# Name: server.py
# Made for COIS-4310H Assignment 3
# Author: Calen Irwin [0630330] & Ryland Whillans [0618437]
# Last Modification Date: 2019-11-07
# Purpose: Server portion of Client/Server chat application
# Instructions for running => "python server.py" or "python server.py&"
# References:   https://www.binarytides.com/code-chat-application-server-client-sockets-python/
#               https://docs.python.org/3/library/struct.html
# =============================================================================================

from socket import *                # import all from socket library
from struct import *                # import all from struct library
from hashlib import *               # import all from hashlib library
from select import select           # import select from select library

VERSION = "3"                       # version of application/rfc
SERVER_ADDRESS = "192.197.151.116"  # address of server
SERVER_PORT = 50330                 # port of server

# constants for accessing packet elements as indices
H_VERSION = 0
H_PACKETNUM = 1
H_SOURCE = 2
H_DEST = 3
H_VERB = 4
H_ENC = 5
CHECKSUM = 6
BODY = 7

# wrapper function to create and send a packet and then iterate the packet number
def send_packet(socket, struct, version, packetNum, src, dest, verb, enc, checksum, body):
    packet = struct.pack(version, packetNum, src, dest, verb, enc, checksum, body)
    socket.send(packet)
    return packetNum + 1

# function to generate and return md5 hash
def get_sha256(body):
    hash = sha256()                             # sha256 hashing algorithm
    hash.update(body.encode("utf-8"))           # hash the given argument
    return hash.hexdigest()                     # 64 character hash string

def main():

    # Struct Description
    # Structs are C structs represented as Python byte objects
    # Our packet struct is defined using the format string passed to the Struct() function
    # The format string is a compact description of the layout of the C struct
    # We use the following functions contained within the Struct module:
    # pack() - Returns a bytes object according to the format string
    # unpack() - Unpack the buffer according to the format string into a tuple
    # -------------------------------------------------------------------------------------
    # Format String Description
    # ! = byte-ordering
    # H = unsigned short
    # c = character
    # p = varaible length string where the maximum length is specified by the number
    #     proceeding it minus 1 (e.g. 21p is a string of maximum 20 characters)
    #--------------------------------------------------------------------------------------
    packetStruct = Struct("!cH21p21p3s8p64s256p")

    packetNum = 0               # counter for total number of packets sent by server

    connectionList = []         # list of all connected socket descriptors
    connectedClientList = []    # list of all connected clients

    serverSocket = socket(AF_INET, SOCK_STREAM)         # master socket to handle new TCP connections
    serverSocket.bind((SERVER_ADDRESS, SERVER_PORT))    # bind the socket to the host and port
    serverSocket.listen(5)                              # listen with max of 5

    connectionList.append(serverSocket)                 # add master socket to connection list

    print("Now listening on " + str(SERVER_ADDRESS) + " port " + str(SERVER_PORT))  # console message to notify that server is actively listening

    # main loop to handle new connections and client requests
    while True:
        readSockets, writeSockets, errorSockets = select(connectionList, [], [])    # returns readable sockets

        # loop through readable sockets and handle their traffic
        for sock in readSockets:
            # handle a new client connection
            if sock == serverSocket:
                sd, clientAddr = sock.accept()  # sd - new client socket descriptor
                clientPacket = packetStruct.unpack(sd.recv(packetStruct.size))
                # validate packet checksum
                if not (clientPacket[CHECKSUM] == get_sha256(clientPacket[BODY])):
                    # request message rebroadcast
                    msg = str(clientPacket[H_PACKETNUM])
                    packetNum = send_packet(sd, packetStruct, VERSION, packetNum, "", clientPacket[H_SOURCE], "reb", "none", get_sha256(msg), msg)
                    sd.close()  # close new client socket
                    print("Packet " + msg + " from user \"" + clientPacket[H_SOURCE] + "\" corrupt. Requesting rebroadcast.")

                # if there are already atleast 5 connected clients
                elif len(connectedClientList) >= 5:
                    # notify client attempting to connect that the server has reached its capacity
                    capacityErr = "Error: Server capacity is full. Please try again later."
                    packetNum = send_packet(sd, packetStruct, VERSION, packetNum, "", clientPacket[H_SOURCE], "err", "none", get_sha256(capacityErr), capacityErr)
                    sd.close()  # close new client socket
                # if there is already a client connected using the requested username
                elif clientPacket[2] in connectedClientList:
                    # notify client attempting to connect that the username is already in use
                    dupNameErr = "Error: That name already exists. Please try connecting using a different name."
                    packetNum = send_packet(sd, packetStruct, VERSION, packetNum, "", clientPacket[H_SOURCE], "err", "none", get_sha256(dupNameErr), dupNameErr)
                    sd.close()  # close new client socket
                # if there were no errors, proceed to handle the new client connection
                else:
                    connectionList.append(sd)                                                           # add new client socket descriptor to connection list
                    connectedClientList.append(clientPacket[H_SOURCE])                                  # add client's name to list of connected clients
                    connectionNotice = "New User \"" + clientPacket[H_SOURCE] + "\" has connected"
                    print(connectionNotice)
                    connectionMsg = "Connected!\nConnected Users: " + ", ".join(connectedClientList)    # create a string of all connected clients
                    # send connection confirmation message
                    packetNum = send_packet(sd, packetStruct, VERSION, packetNum, "", clientPacket[H_SOURCE], "srv", "none", get_sha256(connectionMsg), connectionMsg)
                    # notify all connected clients of the new connection
                    index = 1
                    for client in connectedClientList:
                        if client != clientPacket[H_SOURCE]:
                            packetNum = send_packet(connectionList[index], packetStruct, VERSION, packetNum, clientPacket[H_SOURCE], client, "srv", "none", get_sha256(connectionNotice), connectionNotice)
                        index += 1

            # handle incoming packet from existing client
            else:
                rawPacket = sock.recv(packetStruct.size)
                # handles an abrupt manual disconnect (socket closed)
                if(len(rawPacket) == 0):
                    socketIndex = connectionList.index(sock)                                                        # find the index of the client's socket
                    clientName = connectedClientList.pop(socketIndex-1)                                             # remove client from client list
                    disconnectionNotice = "\"" + clientName + "\" has disconnected"
                    print(disconnectionNotice)
                    connectionList.pop(socketIndex).close()                                                         # remove socket from connection list and close socket

                    # notify other connected clients of the client disconnection
                    index = 1
                    for client in connectedClientList:
                        packetNum = send_packet(connectionList[index], packetStruct, VERSION, packetNum, "", client, "srv", "none", get_sha256(disconnectionNotice), disconnectionNotice)
                        index += 1
                else:
                    clientPacket = packetStruct.unpack(rawPacket)
                    verb = clientPacket[H_VERB]
                    # validate packet checksum
                    if not clientPacket[CHECKSUM] == get_sha256(clientPacket[BODY]):
                        # request message rebroadcast
                        msg = str(clientPacket[H_PACKETNUM])
                        packetNum = send_packet(sd, packetStruct, VERSION, packetNum, "", clientPacket[H_SOURCE], "reb", "none", get_sha256(msg), msg)
                        # display message content
                        print("Packet " + msg + " from user \"" + clientPacket[H_SOURCE] + "\" corrupt. Requesting rebroadcast.")
                    elif verb == 'msg':
                        # if the destination of a message is not connected to the server
                        if clientPacket[H_DEST] not in connectedClientList:
                            # send an error message back to the sender
                            destNotFoundErr = "Error: The recipient of your message is not connected."
                            packetNum = send_packet(sock, packetStruct, VERSION, packetNum, "", clientPacket[H_SOURCE], "err", "none", get_sha256(destNotFoundErr), destNotFoundErr)
                        # otherwise send the message to the destination
                        else:
                            socketIndex = connectedClientList.index(clientPacket[H_DEST]) + 1
                            packetNum = send_packet(connectionList[socketIndex], packetStruct, VERSION, packetNum, clientPacket[H_SOURCE], clientPacket[H_DEST], "msg", clientPacket[H_ENC], get_sha256(clientPacket[BODY]), clientPacket[BODY])
                            # display message content
                            print(clientPacket[H_SOURCE] + " -> " + clientPacket[H_DEST] + ": " + clientPacket[BODY])
                    elif verb == 'all':
                        index = 1
                        # send message to all clients except for the messages sender
                        for client in connectedClientList:
                            if client != clientPacket[H_SOURCE]:
                                packetNum = send_packet(connectionList[index], packetStruct, VERSION, packetNum, clientPacket[H_SOURCE], client, "all", clientPacket[H_ENC], get_sha256(clientPacket[BODY]), clientPacket[BODY])
                            index += 1
                        # display message content
                        print(clientPacket[H_SOURCE] + " -> All: " + clientPacket[BODY])
                    elif verb == 'who':
                        # send a list of all connected clients (inclusive) to the packet sender
                        clients = "Connected Users: " + ", ".join(connectedClientList)
                        packetNum = send_packet(sock, packetStruct, VERSION, packetNum, "", clientPacket[H_SOURCE], "who", "none", get_sha256(clients), clients)
                    # disconnect the clients connection and remove their information from the client lists
                    elif verb == 'bye':
                        clientIndex = connectedClientList.index(clientPacket[H_SOURCE]) # find the index of the client
                        connectedClientList.pop(clientIndex)                            # remove client from client list
                        connectionList.pop(clientIndex+1).close()                       # remove the client socket and close the connection
                        disconnectionNotice = "\"" + clientPacket[H_SOURCE] + "\" has disconnected"
                        print(disconnectionNotice)
                        # notify all connected clients of the client disconnection
                        index = 1
                        for client in connectedClientList:
                            packetNum = send_packet(connectionList[index], packetStruct, VERSION, packetNum, clientPacket[H_SOURCE], client, "srv", "none", get_sha256(disconnectionNotice), disconnectionNotice)
                            index += 1

    serverSocket.close()    # close server socket

if __name__ == "__main__":
    main()
