@echo off
REM 仮想環境を作成する
python -m venv  MediapipeHakkason

REM 仮想環境を有効化する
call MediapipeHakkason\Scripts\activate

REM 必要なパッケージをインストールする
python -m pip install --upgrade pip
pip install mediapipe opencv-python numpy keyboard pyautogui
pip install "git+https://github.com/konbraphat51/UnityPythonConnectionModules.git#egg=UnityConnector&subdirectory=PythonSocket"


REM 仮想環境を無効化する
deactivate

echo 仮想環境が作成され、必要なパッケージがインストールされました。
pause
