using System;
using System.Collections;
using System.Collections.Generic;
using PythonConnection;
using UnityEngine;

public class ConnectionTest : MonoBehaviour
{
    //Pythonへ送信するデータ形式
    [Serializable]
    private class SendingData
    {
        public SendingData(int testValue0, List<float> testValue1)
        {
            this.testValue0 = testValue0;
            this.testValue1 = testValue1;
        }

        public int testValue0;

        [SerializeField]
        private List<float> testValue1;
    }

    void Start()
    {
        //データ受信時時のコールバックを登録
        PythonConnector.instance.RegisterAction(typeof(TestDataClass), OnDataReceived);

        //Pythonへの接続を開始
        if (PythonConnector.instance.StartConnection())
        {
            Debug.Log("Connected");
        }
        else
        {
            Debug.Log("Connection Failed");
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            PythonConnector.instance.StopConnection();
            Debug.Log("Stop");
        }
    }

    public void OnTimeout()
    {
        Debug.Log("Timeout");
    }

    public void OnStop()
    {
        Debug.Log("Stopped");
    }

    //データ受信時のコールバック
    public void OnDataReceived(DataClass data)
    {
        //DataClass型で渡されてしまうため、明示的に型変換
        TestDataClass testData = data as TestDataClass;

        //受け取り結果表示
        Debug.Log("testValue0: " + testData.testValue0);
        foreach (float v in testData.v1)
        {
            Debug.Log("testValue1: " + v);
        }

        //Python側へ送るデータを生成
        int v1 = UnityEngine.Random.Range(0, 100);
        List<float> v2 = new List<float>()
        {
            UnityEngine.Random.Range(0.1f, 0.9f),
            UnityEngine.Random.Range(0.1f, 0.9f)
        };
        SendingData sendingData = new SendingData(v1, v2);

        Debug.Log("Sending Data: " + v1 + ", " + v2[0] + ", " + v2[1]);

        //Python側へ送信
        PythonConnector.instance.Send("test", sendingData);
    }

    // DataClassはPythonConnectorが使用する基底クラスまたはインターフェースだと仮定
    public class TestDataClass : DataClass
    {
        public int testValue0; // 受信する整数値に対応
        public List<float> v1; // 受信する浮動小数点リストに対応

        // データを初期化するためのコンストラクタ
        public TestDataClass(int testValue0, List<float> v1)
        {
            this.testValue0 = testValue0;
            this.v1 = v1;
        }
    }

}
