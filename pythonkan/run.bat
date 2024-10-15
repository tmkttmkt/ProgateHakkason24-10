@echo off

REM 仮想環境を有効化する
call MediapipeHakkason\Scripts\activate

REM 必要なパッケージをインストールする
python %1


REM 仮想環境を無効化する
deactivate

pause
