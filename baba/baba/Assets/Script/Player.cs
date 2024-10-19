using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public NPC npc; // NPCのスクリプトを参照
    public List<GameObject> hands = new List<GameObject>(); // プレイヤーの手札

    // プレイヤーがカードを取る
    public void TakeTurn()
    {
        // プレイヤーのターンでの処理
        // ここでUIやカード選択の処理を追加する (例: カードをクリックしてNPCから引く)
        Debug.Log("Player is taking a turn");

        if(npc == null)
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
            hands.Add(pickedCard);//カード以外がアタッチする可能性がある
            npc.hands.RemoveAt(randomIndex);
        }
        else
        {
            Debug.Log("助けて");
        }
        // ペアがあれば削除
        CheckForPairs();
        GameManager.Instance.EndTurn(); // ターン終了
    }

    public void CheckForPairs()
    {
        // 逆順でループすることでインデックスの問題を回避
        for ( int i = 0;  i <  hands.Count; i++)
        {
            Card card1 = hands[i].GetComponent<Card>();
            for (int j = 0; j <  hands.Count; j++)
            {
                if (i == j)
                {
                    continue;
                }
                Card card2 = hands[j].GetComponent<Card>();
                if(card1 == null)
                {
                    Debug.Log("card1 == NUll" + hands[i].name);
                    continue;
                }
                else
                if(card2 == null)
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
                    i = -1; // 次の外側ループが i = 0 から始まるようにする
                    break;
                }
            }
        }
    }
}
