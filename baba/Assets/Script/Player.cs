using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public NPC npc; // NPCスクリプトへの参照
    public List<GameObject> hands = new List<GameObject>(); // プレイヤーの手札

    // プレイヤーのターンを進行
    bool IsOverlapping(RectTransform rectTransform, Vector2 point)
    {
        // カメラがUIを正しく表示するための設定（通常CanvasのRender Modeに依存）
        Camera cam = null;

        // 指定した座標が矩形内に含まれるかチェック
        return RectTransformUtility.RectangleContainsScreenPoint(rectTransform, point, cam);
    }

    public void TakeTurn(Vector2 xy)
    {
        // プレイヤーがターンを開始
        Debug.Log("Player is taking a turn");

        // NPCの手札から1枚カードを選ぶ
        if (npc.hands.Count > 0)
        {
            GameObject obj = null;
            foreach (GameObject cardobj in npc.hands)
            {
                RectTransform rect = cardobj.GetComponent<RectTransform>();
                if (rect == null) continue;
                Debug.Log(rect.position);
                Debug.Log(xy);
                if (IsOverlapping(rect, xy))
                {
                    obj = cardobj;
                    break;
                }
            }
            if (obj != null)
            {
                obj.transform.SetParent(transform);
                hands.Add(obj);
                npc.hands.Remove(obj);
                CheckForPairs();
                SetCardFaceUp(obj);
                GameManager.Instance.EndTurn(); // ターン終了
            }
            else
            {
                GameManager.Instance.StartTurn(); // 次のターン開始
            }
        }
        else
        {
            GameManager.Instance.StartTurn();
        }
    }

    // カードを表にする処理
    public void SetCardFaceUp(GameObject card)
    {
        Debug.Log("SetCardFaceUp");
        Card cardComponent = card.GetComponent<Card>();
        if (cardComponent != null)
        {
            cardComponent.isFaceUp = true; // 表にする
            UpdateCardAppearance(card);
        }
    }

    // カードの見た目を更新
    private void UpdateCardAppearance(GameObject card)
    {
        Card cardComponent = card.GetComponent<Card>();
        Image imageComponent = card.GetComponent<Image>();

        if (cardComponent != null && imageComponent != null)
        {
            imageComponent.sprite = cardComponent.faceUpSprite; // 表面のスプライトを設定
            Debug.Log(card.name + " のカードが表になりました。");
        }
    }

    // ペアがあるか確認して削除
    public void CheckForPairs()
    {
        for (int i = 0; i < hands.Count; i++)
        {
            Card card1 = hands[i].GetComponent<Card>();
            for (int j = 0; j < hands.Count; j++)
            {
                if (i == j) continue;

                Card card2 = hands[j].GetComponent<Card>();
                if (card1 == null || card2 == null) continue;

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
