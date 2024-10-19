using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour
{
    public List<GameObject> hands = new List<GameObject>(); // NPCの手札
    public List<GameObject> field = new List<GameObject>(); // 捨てる場所 (フィールド)

    // カードを引く
    public void PickCard(List<GameObject> playerHand)
    {
        if (playerHand.Count > 0)
        {
            int randomIndex = Random.Range(0, playerHand.Count);
            GameObject pickedCard = playerHand[randomIndex];
            hands.Add(pickedCard);
            playerHand.RemoveAt(randomIndex);
        }

        // ペアがあれば消す処理
        CheckForPairs();
    }

    // ペアのチェックと削除
    public void CheckForPairs()
    {
        // 逆順でループすることでインデックスの問題を回避
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
                    Debug.Log("NPC_card1 == NUll" + hands[i].name);
                    continue;
                }
                else
                if (card2 == null)
                {
                    Debug.Log("NPC_card2 == NUll" + hands[j].name);
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
                    i = -1; // 次の外側ループが i = 0 から始まるようにする
                    break;
                }
            }
        }
    }
}
