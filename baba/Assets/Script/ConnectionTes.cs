using System;
using System.Collections;
using System.Collections.Generic;
using PythonConnection;
using UnityEngine;

public class ConnectionTest : MonoBehaviour
{
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
        //�f�[�^��M�����̃R�[���o�b�N��o�^
        PythonConnector.instance.RegisterAction(typeof(HandDataClass), OnDataReceived);
        PythonConnector.instance.RegisterAction(typeof(TestDataClass), OnTestDataReceived);

        //Python�ւ̐ڑ����J�n
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
            PythonConnector.instance.Send("end",new flg(true));
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
    // x��y��0~1�͈̔͂ŁA���[���h���W���擾����֐�
    public Vector3 GetWorldPosition(float x, float y)
    {
        Camera mainCamera = Camera.main;

        // ��ʂ̃X�N���[�����W�ɕϊ��ix��y��0~1�͈̔͂Ŋ|����j
        float screenX = x * Screen.width;
        float screenY = y * Screen.height;

        // �X�N���[�����W�����[���h���W�ɕϊ�
        Vector3 screenPosition = new Vector3(screenX, screenY, mainCamera.nearClipPlane);
        Vector3 worldPosition = mainCamera.ScreenToWorldPoint(screenPosition);

        return worldPosition;
    }
    public void OnDataReceived(DataClass data)
    {
        HandDataClass mainmata = data as HandDataClass;
        transform.position = GetWorldPosition(mainmata.x, 1-mainmata.y);
    }
    //�f�[�^��M���̃R�[���o�b�N
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
    }
    // DataClass��PythonConnector���g�p������N���X�܂��̓C���^�[�t�F�[�X���Ɖ���
    public class TestDataClass : DataClass
    {
        public double testValue0; // ��M���鐮���l�ɑΉ�
        public List<float> v1; // ��M���镂�������_���X�g�ɑΉ�

        // �f�[�^�����������邽�߂̃R���X�g���N�^
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
        // �f�[�^�����������邽�߂̃R���X�g���N�^
        public HandDataClass(float x, float y, bool mousedown)
        {
            this.x = x;
            this.y = y;
            this.mousedown = mousedown;
        }
    }

}
