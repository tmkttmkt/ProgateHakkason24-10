using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public NPC npc; // NPCのスクリプトを参照
    public List<GameObject> hands = new List<GameObject>(); // プレイヤーの手札

    // プレイヤーがカードを取る
    public void TakeTurn()
    {
        // プレイヤーのターンでの処理
        Debug.Log("Player is taking a turn");

        if (npc == null)
        {
            Debug.Log("NPC == null");
        }
        else
        {
            Debug.Log("NPC != null");
        }

        // NPCの手札からランダムに1枚取る処理
        if (npc.hands.Count > 0)
        {
            int randomIndex = Random.Range(0, npc.hands.Count);
            GameObject pickedCard = npc.hands[randomIndex];
            hands.Add(pickedCard);
            npc.hands.RemoveAt(randomIndex);

            // カードを表向きにする処理
            SetCardFaceUp(pickedCard);
        }
        else
        {
            Debug.Log("助けて");
        }

        // ペアがあれば削除
        CheckForPairs();
        GameManager.Instance.EndTurn(); // ターン終了
    }

    // カードを表向きにする処理
    public void SetCardFaceUp(GameObject card)
    {
        Debug.Log("SetCardFaceUp");
        Card cardComponent = card.GetComponent<Card>();
        if (cardComponent != null)
        {
            cardComponent.isFaceUp = true; // 表向きに設定
            UpdateCardAppearance(card);
        }
    }

    // カードの見た目を更新する処理
    private void UpdateCardAppearance(GameObject card)
    {
        Card cardComponent = card.GetComponent<Card>();
        Image imageComponent = card.GetComponent<Image>();

        if (cardComponent != null && imageComponent != null)
        {

                imageComponent.sprite = cardComponent.faceUpSprite; // 表向きの画像
                Debug.Log(card.name + " playerを表向きに更新しました。");
        }
    }

    public void CheckForPairs()
    {
        for (int i = 0; i < hands.Count; i++)
        {
            Card card1 = hands[i].GetComponent<Card>();
            for (int j = 0; j < hands.Count; j++)
            {
                if (i == j)
                {
                    continue;
                }
                Card card2 = hands[j].GetComponent<Card>();
                if (card1 == null)
                {
                    Debug.Log("card1 == NUll" + hands[i].name);
                    continue;
                }
                else if (card2 == null)
                {
                    Debug.Log("card 2 == NUll" + hands[j].name);
                    continue;
                }
                if (card1.number == card2.number)
                {
                    Destroy(hands[i]);
                    Destroy(hands[j]);
                    if (i > j)
                    {
                        hands.RemoveAt(i);
                        hands.RemoveAt(j);
                    }
                    else
                    {
                        hands.RemoveAt(j);
                        hands.RemoveAt(i);
                    }
                    i = -1;
                    break;
                }
            }
        }
    }
}
