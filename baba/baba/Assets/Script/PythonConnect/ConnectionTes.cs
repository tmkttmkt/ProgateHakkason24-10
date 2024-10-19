using System;
using System.Collections;
using System.Collections.Generic;
using PythonConnection;
using UnityEngine;

public class ConnectionTest : MonoBehaviour
{
    //Python�֑��M����f�[�^�`��
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
        //�f�[�^��M�����̃R�[���o�b�N��o�^
        PythonConnector.instance.RegisterAction(typeof(TestDataClass), OnDataReceived);

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

    //�f�[�^��M���̃R�[���o�b�N
    public void OnDataReceived(DataClass data)
    {
        //DataClass�^�œn����Ă��܂����߁A�����I�Ɍ^�ϊ�
        TestDataClass testData = data as TestDataClass;

        //�󂯎�茋�ʕ\��
        Debug.Log("testValue0: " + testData.testValue0);
        foreach (float v in testData.v1)
        {
            Debug.Log("testValue1: " + v);
        }

        //Python���֑���f�[�^�𐶐�
        int v1 = UnityEngine.Random.Range(0, 100);
        List<float> v2 = new List<float>()
        {
            UnityEngine.Random.Range(0.1f, 0.9f),
            UnityEngine.Random.Range(0.1f, 0.9f)
        };
        SendingData sendingData = new SendingData(v1, v2);

        Debug.Log("Sending Data: " + v1 + ", " + v2[0] + ", " + v2[1]);

        //Python���֑��M
        PythonConnector.instance.Send("test", sendingData);
    }

    // DataClass��PythonConnector���g�p������N���X�܂��̓C���^�[�t�F�[�X���Ɖ���
    public class TestDataClass : DataClass
    {
        public int testValue0; // ��M���鐮���l�ɑΉ�
        public List<float> v1; // ��M���镂�������_���X�g�ɑΉ�

        // �f�[�^�����������邽�߂̃R���X�g���N�^
        public TestDataClass(int testValue0, List<float> v1)
        {
            this.testValue0 = testValue0;
            this.v1 = v1;
        }
    }

}
