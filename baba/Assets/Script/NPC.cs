using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // Image コンポーネントにアクセスするために必要

public class NPC : MonoBehaviour
{
    public List<GameObject> hands = new List<GameObject>(); // NPCの手札
    public List<GameObject> field = new List<GameObject>(); // 捨てる場所 (フィールド)

    void Start()
    {
        ShowCardsFaceDown(); // NPCの持つカードを裏向きにする
    }

    // NPCの持つカードをすべて裏向きにする
    public void ShowCardsFaceDown()
    {
        foreach (GameObject card in hands)
        {
            Card cardComponent = card.GetComponent<Card>();
            if (cardComponent != null)
            {
                cardComponent.isFaceUp = false; // 裏向きに設定
                UpdateCardAppearance(card); // 見た目を更新
            }
        }
    }

    // カードを引く
    public void PickCard(List<GameObject> playerHand)
    {
        if (playerHand.Count > 0)
        {
            int randomIndex = Random.Range(0, playerHand.Count);
            GameObject pickedCard = playerHand[randomIndex];
            pickedCard.transform.SetParent(transform);
            hands.Add(pickedCard);
            playerHand.RemoveAt(randomIndex);

            // 引いたカードを裏向きに設定
            SetCardFaceDown(pickedCard);
        }
        // カードを引いた後、少し時間を置いてからペアをチェックする
        StartCoroutine(WaitAndCheckForPairs(5.0f));
    }

    // カードを裏向きにする
    public void SetCardFaceDown(GameObject card)
    {
        Debug.Log("SetCardFaceDown");
        Card cardComponent = card.GetComponent<Card>();
        if (cardComponent != null)
        {
            cardComponent.isFaceUp = false; // 裏向きにする
            UpdateCardAppearance(card);
        }
    }

    // カードを表向き・裏向きに応じて見た目を更新
    private void UpdateCardAppearance(GameObject card)
    {
        Card cardComponent = card.GetComponent<Card>();
        Image imageComponent = card.GetComponent<Image>();

        if (cardComponent != null && imageComponent != null)
        {
                imageComponent.sprite = cardComponent.faceDownSprite; // 裏向きの画像
                Debug.Log(card.name + " を裏向きに更新しました。");
        }
    }

    // 一定時間待ってからペアのチェックと削除を行う
    private IEnumerator WaitAndCheckForPairs(float delay)
    {
        yield return new WaitForSeconds(delay);
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
                if (card1 == null || card2 == null)
                {
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
