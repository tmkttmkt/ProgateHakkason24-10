import cv2
import mediapipe as mp
import math
import asyncio
from UnityConnector import UnityConnector
class clickobj:
    kyasy:list=[]
    down:bool=False
    def add(self,now):
        self.kyasy.append(now)
        if(len(self.kyasy)>5):
            self.kyasy=self.kyasy[1:]
        self.hantei()
    def hantei(self):
        if all(pas != self.down for pas in self.kyasy):
            self.down = not self.down
    def getnow(self):
        return self.down
        
def calculate_distance(x1, y1, x2, y2,aspect_ratio):
    return math.sqrt((x2 - x1) ** 2 + (y2*aspect_ratio - y1*aspect_ratio) ** 2)
def timeout():
    global eflg
    print("timeout")
    eflg=False
connector = UnityConnector(
    on_timeout=timeout,
    on_stopped=lambda:print("stopped")
)
sflg=True
eflg=True
def on_data_received(data_type, data):
    global sflg,eflg
    if(data_type=="req"):
        sflg=data["sflg"]
    elif(data_type=="end"):
        if data["sflg"]:
            eflg=False
print("connecting...")
connector.start_listening(on_data_received)
print("connected")
async def send_data(x, y, mousedown):
    await asyncio.to_thread(connector.send, "main", {"x": x, "y": y, "mousedown": mousedown})
                    # 座標のスムージング
def smooth_update(past_value, new_value, threshold=0.005):
    return past_value if abs(past_value - new_value) < threshold else new_value

past_x=0
past_y=0
async def main_loop():
    global sflg, eflg, past_x, past_y

    # MediaPipeの設定
    mp_hands = mp.solutions.hands

    # カメラの起動
    cap = cv2.VideoCapture(0)
    cap.set(cv2.CAP_PROP_FPS, cap.get(cv2.CAP_PROP_FPS))
    cap.set(cv2.CAP_PROP_FRAME_WIDTH, 320)
    cap.set(cv2.CAP_PROP_FRAME_HEIGHT, 240)

    cli = clickobj()

    with mp_hands.Hands(
        model_complexity=1,
        min_detection_confidence=0.5,
        min_tracking_confidence=0.5) as hands:

        while cap.isOpened() and eflg:
            success, image = cap.read()
            if not success:
                print("カメラから画像を取得できませんでした。")
                break

            if sflg:
                image = cv2.flip(image, 1)
                image_rgb = cv2.cvtColor(image, cv2.COLOR_BGR2RGB)
                results = hands.process(image_rgb)

                if results.multi_hand_landmarks:
                    hand_landmarks = results.multi_hand_landmarks[-1]
                    index_finger_tip = hand_landmarks.landmark[8]
                    oyayubi_finger_tip = hand_landmarks.landmark[4]
                    h, w, _ = image.shape
                    dis = calculate_distance(index_finger_tip.x, index_finger_tip.y, oyayubi_finger_tip.x, oyayubi_finger_tip.y, w/h)
                    
                    cli.add(dis < 0.06)

                    x = (index_finger_tip.x + oyayubi_finger_tip.x) / 2
                    y = (index_finger_tip.y + oyayubi_finger_tip.y) / 2


                    x = smooth_update(past_x, x)
                    y = smooth_update(past_y, y)
                    past_x, past_y = x, y

                    # 非同期でデータを送信
                    await send_data(x, y, cli.getnow())

            await asyncio.sleep(1/15)  # 短い休止を挟むことでCPU負荷を軽減

    cap.release()
    cv2.destroyAllWindows()

# イベントループを開始
asyncio.run(main_loop())