using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public NPC npc; // NPC�ｿｽﾌス�ｿｽN�ｿｽ�ｿｽ�ｿｽv�ｿｽg�ｿｽ�ｿｽ�ｿｽQ�ｿｽ�ｿｽ
    public List<GameObject> hands = new List<GameObject>(); // �ｿｽv�ｿｽ�ｿｽ�ｿｽC�ｿｽ�ｿｽ�ｿｽ[�ｿｽﾌ趣ｿｽD
                                                            // �ｿｽv�ｿｽ�ｿｽ�ｿｽC�ｿｽ�ｿｽ�ｿｽ[�ｿｽ�ｿｽ�ｿｽJ�ｿｽ[�ｿｽh�ｿｽ�ｿｽ�ｿｽ�ｿｽ�ｿｽ
    bool IsOverlapping(RectTransform rectTransform, Vector2 point)
    {
        // �ｿｽJ�ｿｽ�ｿｽ�ｿｽ�ｿｽ�ｿｽ�ｿｽUI�ｿｽ�ｿｽ\�ｿｽ�ｿｽ�ｿｽ�ｿｽ�ｿｽﾄゑｿｽ�ｿｽ驍ｽ�ｿｽﾟのカ�ｿｽ�ｿｽ�ｿｽ�ｿｽ�ｿｽ�ｿｽ�ｿｽ謫ｾ�ｿｽi�ｿｽﾊ擾ｿｽ�ｿｽCanvas�ｿｽ�ｿｽRender Mode�ｿｽﾉ会ｿｽ�ｿｽ�ｿｽ�ｿｽﾄ異なゑｿｽj
        Camera cam = null;

        // �ｿｽw�ｿｽ閧ｵ�ｿｽ�ｿｽ�ｿｽ�ｿｽ�ｿｽW�ｿｽ�ｿｽ�ｿｽl�ｿｽp�ｿｽ`�ｿｽ�ｿｽ�ｿｽﾉゑｿｽ�ｿｽ驍ｩ�ｿｽ�ｿｽ�ｿｽ`�ｿｽF�ｿｽb�ｿｽN
        return RectTransformUtility.RectangleContainsScreenPoint(rectTransform, point, cam);
    }
    public bool IsCircleCenterInsideRectangle(RectTransform  rectangleRectTransform,Vector2 circleCenter ){

        // 四角形の位置とサイズを取得
        Vector2 rectPosition = rectangleRectTransform.anchoredPosition;
        Vector2 rectSize = rectangleRectTransform.sizeDelta;

        // 四角形の境界を計算
        float rectMinX = rectPosition.x - rectSize.x / 2;
        float rectMaxX = rectPosition.x + rectSize.x / 2;
        float rectMinY = rectPosition.y - rectSize.y / 2;
        float rectMaxY = rectPosition.y + rectSize.y / 2;

        // 円の中心が四角形の境界内にあるかどうかを判定
        bool isInsideX = circleCenter.x >= rectMinX && circleCenter.x <= rectMaxX;
        bool isInsideY = circleCenter.y >= rectMinY && circleCenter.y <= rectMaxY;
        Debug.Log(isInsideX.ToString()+"|"+isInsideY.ToString());
        // XとY両方で範囲内にある場合は「当たっている」
        return isInsideX && isInsideY;
    }
    public void TakeTurn(Vector2 xy)
    {
        // �ｿｽv�ｿｽ�ｿｽ�ｿｽC�ｿｽ�ｿｽ�ｿｽ[�ｿｽﾌタ�ｿｽ[�ｿｽ�ｿｽ�ｿｽﾅの擾ｿｽ�ｿｽ�ｿｽ
        Debug.Log("Player is taking a turn");
        // NPC�ｿｽﾌ趣ｿｽD�ｿｽ�ｿｽ�ｿｽ辜会ｿｽ�ｿｽ�ｿｽ_�ｿｽ�ｿｽ�ｿｽ�ｿｽ1�ｿｽ�ｿｽ�ｿｽ�ｿｽ髀茨ｿｽ�ｿｽ
        if (npc.hands.Count > 0)
        {
            /*int randomIndex = Random.Range(0, npc.hands.Count);
            GameObject pickedCard = npc.hands[randomIndex];
            hands.Add(pickedCard);
            npc.hands.RemoveAt(randomIndex);
            */
            GameObject obj=null;
            foreach (GameObject cardobj in npc.hands)
            {
                RectTransform rect=cardobj.GetComponent<RectTransform>();
                if (rect == null) continue;
                Debug.Log(rect.position);
                Debug.Log(xy);
                if (IsCircleCenterInsideRectangle(rect, xy))
                {
                Debug.Log("言ってるね");
                    obj =cardobj;
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
                GameManager.Instance.EndTurn(); // �ｿｽ^�ｿｽ[�ｿｽ�ｿｽ�ｿｽI�ｿｽ�ｿｽ
            }
            else{
                GameManager.Instance.StartTurn();
            }

            // �ｿｽJ�ｿｽ[�ｿｽh�ｿｽ�ｿｽ\�ｿｽ�ｿｽ�ｿｽ�ｿｽ�ｿｽﾉゑｿｽ�ｿｽ髀茨ｿｽ�ｿｽ
        }
        else
        {
            GameManager.Instance.StartTurn();
        }

        // �ｿｽy�ｿｽA�ｿｽ�ｿｽ�ｿｽ�ｿｽ�ｿｽ�ｿｽ
    }

    // �ｿｽJ�ｿｽ[�ｿｽh�ｿｽ�ｿｽ\�ｿｽ�ｿｽ�ｿｽ�ｿｽ�ｿｽﾉゑｿｽ�ｿｽ髀茨ｿｽ�ｿｽ
    public void SetCardFaceUp(GameObject card)
    {
        Debug.Log("SetCardFaceUp");
        Card cardComponent = card.GetComponent<Card>();
        if (cardComponent != null)
        {
            cardComponent.isFaceUp = true; // �ｿｽ\�ｿｽ�ｿｽ�ｿｽ�ｿｽ�ｿｽﾉ設抵ｿｽ
            UpdateCardAppearance(card);
        }
    }

    // �ｿｽJ�ｿｽ[�ｿｽh�ｿｽﾌ鯉ｿｽ�ｿｽ�ｿｽ�ｿｽﾚゑｿｽ�ｿｽX�ｿｽV�ｿｽ�ｿｽ�ｿｽ髀茨ｿｽ�ｿｽ
    private void UpdateCardAppearance(GameObject card)
    {
        Card cardComponent = card.GetComponent<Card>();
        Image imageComponent = card.GetComponent<Image>();

        if (cardComponent != null && imageComponent != null)
        {

                imageComponent.sprite = cardComponent.faceUpSprite; // �ｿｽ\�ｿｽ�ｿｽ�ｿｽ�ｿｽ�ｿｽﾌ画像
                //Debug.Log(card.name + " player�ｿｽ�ｿｽ\�ｿｽ�ｿｽ�ｿｽ�ｿｽ�ｿｽﾉ更�ｿｽV�ｿｽ�ｿｽ�ｿｽﾜゑｿｽ�ｿｽ�ｿｽ�ｿｽB");
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
                    //Debug.Log("card1 == NUll" + hands[i].name);
                    continue;
                }
                else if (card2 == null)
                {
                    //Debug.Log("card 2 == NUll" + hands[j].name);
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
