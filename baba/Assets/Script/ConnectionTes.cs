using System;
using System.Collections;
using System.Collections.Generic;
using PythonConnection;
using UnityEngine;

public class ConnectionTest : MonoBehaviour
{
    //Pythonへ送信するデータ形式
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
    private class flg
    {
        public flg(bool testValue0)
        {
            this.sflg= testValue0;
        }

        public bool sflg;
    }

    void Start()
    {
        //データ受信時時のコールバックを登録
        PythonConnector.instance.RegisterAction(typeof(HandDataClass), OnDataReceived);
        PythonConnector.instance.RegisterAction(typeof(TestDataClass), OnTestDataReceived);

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
        if (Input.GetKeyDown(KeyCode.T))
        {
            Debug.Log("T");
            PythonConnector.instance.Send("req",new flg(true));
        }
        if (Input.GetKeyDown(KeyCode.F))
        {
            Debug.Log("F");
            PythonConnector.instance.Send("req",new flg(false));
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
    // xとyが0~1の範囲で、ワールド座標を取得する関数
    public Vector3 GetWorldPosition(float x, float y)
    {
        Camera mainCamera = Camera.main;

        // 画面のスクリーン座標に変換（xとyを0~1の範囲で掛ける）
        float screenX = x * Screen.width;
        float screenY = y * Screen.height;

        // スクリーン座標をワールド座標に変換
        Vector3 screenPosition = new Vector3(screenX, screenY, mainCamera.nearClipPlane);
        Vector3 worldPosition = mainCamera.ScreenToWorldPoint(screenPosition);

        return worldPosition;
    }
    public void OnDataReceived(DataClass data)
    {
        HandDataClass mainmata = data as HandDataClass;
        transform.position = GetWorldPosition(mainmata.x, mainmata.y);
    }
    //データ受信時のコールバック
    public void OnTestDataReceived(DataClass data)
    {
        TestDataClass testData = data as TestDataClass;

        Debug.Log("testValue0: " + testData.testValue0);
        foreach (float v in testData.v1)
        {
            Debug.Log("testValue1: " + v);
        }

        int v1 = UnityEngine.Random.Range(0, 100);
        List<float> v2 = new List<float>()
        {
            UnityEngine.Random.Range(0.1f, 0.9f),
            UnityEngine.Random.Range(0.1f, 0.9f)
        };
        SendingData sendingData = new SendingData(v1, v2);

        Debug.Log("Sending Data: " + v1 + ", " + v2[0] + ", " + v2[1]);

        PythonConnector.instance.Send("test", sendingData);
    }
    // DataClassはPythonConnectorが使用する基底クラスまたはインターフェースだと仮定
    public class TestDataClass : DataClass
    {
        public double testValue0; // 受信する整数値に対応
        public List<float> v1; // 受信する浮動小数点リストに対応

        // データを初期化するためのコンストラクタ
        public TestDataClass(float testValue0, List<float> v1)
        {
            this.testValue0 = testValue0;
            this.v1 = v1;
        }
    }
    public class HandDataClass : DataClass
    {
        public float x;
        public float y;
        public bool mousedown;
        // データを初期化するためのコンストラクタ
        public HandDataClass(float x, float y, bool mousedown)
        {
            this.x = x;
            this.y = y;
            this.mousedown = mousedown;
        }
    }

}
