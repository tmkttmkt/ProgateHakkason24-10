using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public NPC npc; // NPC�̃X�N���v�g���Q��
    public List<GameObject> hands = new List<GameObject>(); // �v���C���[�̎�D
                                                            // �v���C���[���J�[�h�����
    bool IsOverlapping(RectTransform rectTransform, Vector2 point)
    {
        // �J������UI��\�����Ă��邽�߂̃J�������擾�i�ʏ��Canvas��Render Mode�ɉ����ĈقȂ�j
        Camera cam = null;

        // �w�肵�����W���l�p�`���ɂ��邩���`�F�b�N
        return RectTransformUtility.RectangleContainsScreenPoint(rectTransform, point, cam);
    }
    public void TakeTurn(Vector2 xy)
    {
        // �v���C���[�̃^�[���ł̏���
        // ������UI��J�[�h�I���̏�����ǉ����� (��: �J�[�h���N���b�N����NPC�������)
        Debug.Log("Player is taking a turn");
        // NPC�̎�D���烉���_����1����鏈��
        if (npc.hands.Count > 0)
        {
            /*int randomIndex = Random.Range(0, npc.hands.Count);
            GameObject pickedCard = npc.hands[randomIndex];
            hands.Add(pickedCard);//�J�[�h�ȊO���A�^�b�`����\��������
            npc.hands.RemoveAt(randomIndex);
            */
            GameObject obj=null;
            foreach (GameObject cardobj in npc.hands)
            {
                RectTransform rect=cardobj.GetComponent<RectTransform>();
                if (rect == null) continue;
                Debug.Log(rect.position);
                Debug.Log(xy);
                if (IsOverlapping(rect, xy))
                {
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
                GameManager.Instance.EndTurn(); // �^�[���I��
            }
            else{
                GameManager.Instance.StartTurn();
            }
        }
        else
        {
            GameManager.Instance.StartTurn();
        }
        // �y�A������΍폜
    }

    public void CheckForPairs()
    {
        // �t���Ń��[�v���邱�ƂŃC���f�b�N�X�̖������
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
                    i = -1; // ���̊O�����[�v�� i = 0 ����n�܂�悤�ɂ���
                    break;
                }
            }
        }
    }
}
