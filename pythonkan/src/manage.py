import cv2
import mediapipe as mp
import keyboard
import pyautogui
import math

def calculate_distance(x1, y1, x2, y2,wh):
    """2点間の距離を計算"""
    return math.sqrt((x2 - x1) ** 2 + (y2*wh - y1*wh) ** 2)
# MediaPipeの設定
mp_hands = mp.solutions.hands
mp_drawing = mp.solutions.drawing_utils
mp_drawing_styles = mp.solutions.drawing_styles

# カメラの起動
cap = cv2.VideoCapture(0)

with mp_hands.Hands(
    model_complexity=1,
    min_detection_confidence=0.5,
    min_tracking_confidence=0.5) as hands:
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
            for hand_landmarks in results.multi_hand_landmarks:
                # 手のランドマークを描画
                mp_drawing.draw_landmarks(
                    image, 
                    hand_landmarks, 
                    mp_hands.HAND_CONNECTIONS,
                    mp_drawing_styles.get_default_hand_landmarks_style(),
                    mp_drawing_styles.get_default_hand_connections_style())
            index_finger_tip = hand_landmarks.landmark[8]
            oyayubi_finger_tip = hand_landmarks.landmark[4]
            h, w, _ = image.shape
            dis=calculate_distance(index_finger_tip.x,index_finger_tip.y,oyayubi_finger_tip.x,oyayubi_finger_tip.y,w/h)
            print(dis<0.06,dis)
            screen_width, screen_height = pyautogui.size()
            
            hito_x = int(index_finger_tip.x * screen_width)
            hito_y = int(index_finger_tip.y * screen_height)
            oya_x = int(oyayubi_finger_tip.x * screen_width)
            oya_y = int(oyayubi_finger_tip.y * screen_height)
            pyautogui.moveTo((hito_x+oya_x)/2,(hito_y+oya_y)/2)  
        if keyboard.is_pressed('esc') or keyboard.is_pressed('q'):
            print("Exit key detected. Exiting...")
            break
        # 結果を表示
        cv2.imshow('Hand Detection', image)

        if cv2.waitKey(5) & 0xFF == 27:  # Escキーで終了
            break

cap.release()
cv2.destroyAllWindows()
