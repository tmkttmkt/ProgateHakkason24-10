using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour
{
    public int number;
    public string suit; // �J�[�h�̃X�[�g (e.g. "Hearts", "Diamonds", etc.)
    public bool isFaceUp = true;
    public Sprite faceUpSprite;  // 表向きの画像
    public Sprite faceDownSprite; // 裏向きの画像

    void Start()
    {
        // 裏向きの画像をResourcesからロードする
        faceDownSprite = Resources.Load<Sprite>("Cards/card_back");
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
