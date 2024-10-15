from UnityConnector import UnityConnector

#インスタンス
connector = UnityConnector(
    on_timeout=lambda:print("timeout"),
    on_stopped=lambda:print("stopped")
)

#データが飛んできたときのコールバック
def on_data_received(data_type, data):
    print(data_type, data)

print("connecting...")

#Unity側の接続を待つ
connector.start_listening(
    on_data_received
)

print("connected")

#デモ用のループ
while(True):
    #Enterで送信を開始（入力内容は送信内容と関係ない）
    input_data = input()

    #Unityへ停止命令
    if input_data == "q":
        connector.stop_connection()
        break

    #送るデータをdictionary形式で
    data = {
        "testValue0": 334,
        "testValue1": [0.54,0.23,0.12,],
    }
    
    print(data)

    #Unityへ送る
    connector.send(
        "test",
        data
    )
