import math
import time
import random
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
        flg=True
        for pas in self.kyasy:
            if(pas==self.down):flg=False
        if flg:
            if self.down:
                self.down=False
            elif not self.down:
                self.down=True
    def getnow(self):
        return self.down
        
def calculate_distance(x1, y1, x2, y2,wh):
    """2点間の距離を計算"""
    return math.sqrt((x2 - x1) ** 2 + (y2*wh - y1*wh) ** 2)
def timeout():
    global eflg
    print("timeout")
    eflg=False
#インスタンス
connector = UnityConnector(
    on_timeout=timeout,
    on_stopped=lambda:print("stopped")
)
sflg=True
eflg=True
#データが飛んできたときのコールバック
def on_data_received(data_type, data):
    global sflg,eflg
    if(data_type=="req"):
        sflg=data["sflg"]
    elif(data_type=="end"):
        if data["sflg"]:
            eflg=False

print("connecting...")
#Unity側の接続を待つ
connector.start_listening(
    on_data_received
)

print("connected")



flg=True
while eflg:
    if sflg:
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
    # 結果を表示


