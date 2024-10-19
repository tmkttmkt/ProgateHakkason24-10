import cv2
import mediapipe as mp
import keyboard
import pyautogui
import math
import time
from UnityConnector import UnityConnector

#インスタンス
connector = UnityConnector(
    on_timeout=lambda:print("timeout"),
    on_stopped=lambda:print("stopped")
)
class clickobj:
    kyasy:list=[]
    down:bool=False
    def add(self,now):
        self.kyasy.append(now)
        if(len(self.kyasy)>3):
            self.kyasy=self.kyasy[1:]
        self.hantei()
    def hantei(self):
        flg=True
        for pas in self.kyasy:
            if(pas==self.down):flg=False
        if flg:
            if self.down:
                self.down=False
                pyautogui.mouseUp()
            elif not self.down:
                self.down=True
                pyautogui.mouseDown()
    def getnow(self):
        return self.down
        
def calculate_distance(x1, y1, x2, y2,wh):
    """2点間の距離を計算"""
    return math.sqrt((x2 - x1) ** 2 + (y2*wh - y1*wh) ** 2)
# MediaPipeの設定
mp_hands = mp.solutions.hands


# カメラの起動
cap = cv2.VideoCapture(0)

with mp_hands.Hands(
    model_complexity=1,
    min_detection_confidence=0.5,
    min_tracking_confidence=0.5) as hands:
    cli=clickobj()
    while cap.isOpened():
        success, image = cap.read()
        if not success:
            print("カメラから画像を取得できませんでした。")
            break

        # 画像を反転し、BGRからRGBに変換
        image = cv2.flip(image, 1)
        image_rgb = cv2.cvtColor(image, cv2.COLOR_BGR2RGB)

        # 手のランドマークを検出
        results = hands.process(image_rgb)

        # 検出された手のランドマークを描画
        if results.multi_hand_landmarks:
            hand_landmarks=results.multi_hand_landmarks[-1]
            index_finger_tip = hand_landmarks.landmark[8]
            oyayubi_finger_tip = hand_landmarks.landmark[4]
            h, w, _ = image.shape
            dis=calculate_distance(index_finger_tip.x,index_finger_tip.y,oyayubi_finger_tip.x,oyayubi_finger_tip.y,w/h)
            print(dis<0.06,dis)
            cli.add(dis<0.06)
            connector.send(
                "test",
                {
                    "x":(index_finger_tip.x+oyayubi_finger_tip.x)/2,
                    "y":(index_finger_tip.y+oyayubi_finger_tip.y)/2,
                    "mousedown":cli.getnow(),
                }
            )
        if keyboard.is_pressed('esc') or keyboard.is_pressed('q'):
            print("Exit key detected. Exiting...")
            break
        # 結果を表示
        time.sleep(1/1000)

cap.release()
cv2.destroyAllWindows()
