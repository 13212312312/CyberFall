import socket
import threading
import json
import DQN
import numpy as np

HOST = "127.0.0.1"  # Standard loopback interface address (localhost)
PORT = 8053  # Port to listen on (non-privileged ports are > 1023)
def inputToArray(inp):
    list = []
    list += [inp["player_health"]]
    list += [inp["player_x"]]
    list += [inp["player_y"]]
    list += [inp["mapType"]]
    for enemy in inp["enemies"]:
        list += [enemy["poz"]["pozx"]]
        list += [enemy["poz"]["pozy"]]
        list += [enemy["type"]]
    return list


def outputToArray(outp):
    list = []
    for enemy in outp["enemies"]:
        list += [enemy["pozx"]]
        list += [enemy["pozy"]]
    return list

def getScore(input,output):
    score = 0
    for i in range(7):
        aux = (i * 3) + 4
        aux2 = i * 2
        if not (input[aux] == 0 and input[aux + 1] == 0):
            dist = (input[1] - output[aux2]) ** 2 + (input[2] - output[aux2 + 1]) ** 2
            score -= dist
    return score


def createThread(connection, address):
    with connection:
        print(f"Connected by {address}")
        while True:
            data = connection.recv(4096)
            parsedData = json.loads(data)
            inp = parsedData["message"]
            outp = parsedData["response"]

            input_array = np.array(inputToArray(inp))
            output_array = np.array(outputToArray(outp))

            score = getScore(input_array, output_array)

            input_array = np.array(inputToArray(inp))
            output_array = np.array(outputToArray(outp))

            print(input_array)
            print(output_array)
            # print(inp)
            # print(outp)

            # Apply network
            loss = DQN.train_step(input_array, output_array)
            generated_output = DQN.network.predict(input_array)
            print(generated_output)
            generated_score = getScore(input_array, generated_output[0])
            print(f"Initial score: {score}, Network score: {generated_score}")


with socket.socket(socket.AF_INET, socket.SOCK_STREAM) as s:
    s.bind((HOST, PORT))
    while True:
        s.listen()
        conn, addr = s.accept()
        thread = threading.Thread(target=createThread, args=(conn, addr))
        thread.start()
