import cv2
from cvzone.HandTrackingModule import HandDetector
import socket
import numpy as np

# Parameters
width, height = 1280, 720
UDP_IP = "127.0.0.1"
UDP_PORT = 4141

# Camera
cap = cv2.VideoCapture(0)
if not cap.isOpened():
    print("Lỗi: Không tìm thấy camera.")
    exit()
cap.set(3, width)
cap.set(4, height)

# Hand Detector
detector = HandDetector(maxHands=2, detectionCon=0.8)

# Communication
sock = socket.socket(socket.AF_INET, socket.SOCK_DGRAM)
serverAddressPort = (UDP_IP, UDP_PORT)


while True:
    # Get the Frame from the Camera
    success, img = cap.read()

    # Hands
    hands, img = detector.findHands(img)


    data = []
    # Landmark values - (x, y, z) * 21

    if hands:
        for hand in hands:
            lmList = hand["lmList"]
            for lm in lmList:
                data.extend([lm[0], height - lm[1], lm[2]])

        # CHÚ Ý: Chuyển list số thành chuỗi, không cần dấu ngoặc vuông[]
        data_string = ",".join(map(str, data))

        print(data)
        sock.sendto(str.encode(str(data)), serverAddressPort)

    img = cv2.resize(img, (0, 0), None, 0.25, 0.25)
    cv2.imshow('Image', img[:, ::-1, :])
    cv2.waitKey(1)