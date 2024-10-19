from UnityConnector import UnityConnector
import time
import random
#インスタンス
connector = UnityConnector(
    on_timeout=lambda:print("timeout"),
    on_stopped=lambda:print("stopped")
)
sflg=True
#データが飛んできたときのコールバック
def on_data_received(data_type, data):
    global sflg
    if(data_type=="req"):
        sflg=data["sflg"]

print("connecting...")

#Unity側の接続を待つ
connector.start_listening(
    on_data_received
)

print("connected")
flg=False
#デモ用のループ
while(True):
    if(sflg):
        time.sleep(1/1000)
        x=random.random()
        y=random.random()
        han=random.random()
        if(han>0.95):flg=not flg
        data={
                "x":x,
                "y":y,
                "mousedown":flg,
            }
        

        #Unityへ送る
        connector.send(
            "main",
            data
        )
