using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // Image �R���|�[�l���g�ɃA�N�Z�X���邽�߂ɕK�v

public class NPC : MonoBehaviour
{
    public List<GameObject> hands = new List<GameObject>(); // NPC�̎�D
    public List<GameObject> field = new List<GameObject>(); // �̂Ă�ꏊ (�t�B�[���h)

    void Start()
    {
        ShowCardsFaceDown(); // NPC�̎��J�[�h�𗠌����ɂ���
    }

    // NPC�̎��J�[�h�����ׂė������ɂ���
    public void ShowCardsFaceDown()
    {
        foreach (GameObject card in hands)
        {
            Card cardComponent = card.GetComponent<Card>();
            if (cardComponent != null)
            {
                cardComponent.isFaceUp = false; // �������ɐݒ�
                UpdateCardAppearance(card); // �����ڂ��X�V
            }
        }
    }

    // �J�[�h������
    public void PickCard(List<GameObject> playerHand)
    {
        if (playerHand.Count > 0)
        {
            int randomIndex = Random.Range(0, playerHand.Count);
            GameObject pickedCard = playerHand[randomIndex];
            pickedCard.transform.SetParent(transform);
            hands.Add(pickedCard);
            playerHand.RemoveAt(randomIndex);

            // �������J�[�h�𗠌����ɐݒ�
            SetCardFaceDown(pickedCard);
        }
        // �J�[�h����������A�������Ԃ�u���Ă���y�A���`�F�b�N����
        StartCoroutine(WaitAndCheckForPairs(5.0f));
    }

    // �J�[�h�𗠌����ɂ���
    public void SetCardFaceDown(GameObject card)
    {
        Debug.Log("SetCardFaceDown");
        Card cardComponent = card.GetComponent<Card>();
        if (cardComponent != null)
        {
            cardComponent.isFaceUp = false; // �������ɂ���
            UpdateCardAppearance(card);
        }
    }

    // �J�[�h��\�����E�������ɉ����Č����ڂ��X�V
    private void UpdateCardAppearance(GameObject card)
    {
        Card cardComponent = card.GetComponent<Card>();
        Image imageComponent = card.GetComponent<Image>();

        if (cardComponent != null && imageComponent != null)
        {
                imageComponent.sprite = cardComponent.faceDownSprite; // �������̉摜
                Debug.Log(card.name + " �𗠌����ɍX�V���܂����B");
        }
    }

    // ��莞�ԑ҂��Ă���y�A�̃`�F�b�N�ƍ폜���s��
    private IEnumerator WaitAndCheckForPairs(float delay)
    {
        yield return new WaitForSeconds(delay);
        CheckForPairs();
    }

    // �y�A�̃`�F�b�N�ƍ폜
    public void CheckForPairs()
    {
        // �t���Ń��[�v���邱�ƂŃC���f�b�N�X�̖������
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

                    i = -1; // ���̊O�����[�v�� i = 0 ����n�܂�悤�ɂ���
                    break;
                }
            }
        }
    }
}
