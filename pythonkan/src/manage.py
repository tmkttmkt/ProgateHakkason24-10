# main.py
import cv2
import mediapipe as mp

mp_face_mesh = mp.solutions.face_mesh
mp_drawing = mp.solutions.drawing_utils

# カメラの初期化
cap = cv2.VideoCapture(0)

# 出力ファイルの設定
fourcc = cv2.VideoWriter_fourcc(*'mp4v')
out = cv2.VideoWriter('output.mp4', fourcc, 20.0, (640, 480))

with mp_face_mesh.FaceMesh(max_num_faces=1) as face_mesh:
    while cap.isOpened():
        ret, frame = cap.read()
        if not ret:
            break

        # フレームの処理
        frame_rgb = cv2.cvtColor(frame, cv2.COLOR_BGR2RGB)
        results = face_mesh.process(frame_rgb)

        # アイトラッキング用の描画
        if results.multi_face_landmarks:
            for face_landmarks in results.multi_face_landmarks:
                mp_drawing.draw_landmarks(
                    frame,
                    face_landmarks,
                    mp_face_mesh.FACE_CONNECTIONS)

        # フレームを出力
        out.write(frame)

        # フレームの表示
        cv2.imshow('Eye Tracking', frame)
        if cv2.waitKey(1) & 0xFF == ord('q'):
            break

# リソースの解放
cap.release()
out.release()
cv2.destroyAllWindows()
