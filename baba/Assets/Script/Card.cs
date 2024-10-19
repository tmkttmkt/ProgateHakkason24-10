using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour
{
    public int number; // �J�[�h�̐��� (1�`13: �X�[�g, 14: �W���[�J�[)
    public string suit; // �J�[�h�̃X�[�g (e.g. "Hearts", "Diamonds", etc.)

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    // �J�[�h�̃y�A����
    public bool IsPair(Card other) // public��ǉ�
    {
        return this.number == other.number;
    }
}
