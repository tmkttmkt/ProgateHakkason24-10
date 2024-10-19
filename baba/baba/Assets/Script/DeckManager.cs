using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DeckManager : MonoBehaviour
{
    public List<GameObject> deck = new List<GameObject>(); // �f�b�L
    public Transform playerHand; // �v���C���[�̎�D�ʒu
    public Transform npcHand; // NPC�̎�D�ʒu
    public GameObject cardBackPrefab; // �J�[�h�̗���Prefab
    Player player = new Player();
    public Player Player_deck;
    public NPC NPC_deck;

    void Start()
    {
        // �f�b�L�𐶐�����
        GenerateDeck();
        // �J�[�h���V���b�t�����Ĕz��
        ShuffleAndDeal();
    }

    void GenerateDeck()
    {
        // Resources/Prefabs����J�[�hPrefab�����[�h
        for (int i = 1; i <= 13; i++)
        {
            foreach (string suit in new string[] { "heart", "diamond", "club", "spade" })
            {
                GameObject cardPrefab = Resources.Load<GameObject>("Prefabs/" + "Card_" + suit + "_" + i);
                if (cardPrefab != null)
                {
                    deck.Add(Instantiate(cardPrefab)); // Prefab�ł͂Ȃ��A���̉������J�[�h��ǉ�
                }
            }
        }
        // �W���[�J�[��ǉ�
        GameObject joker = Resources.Load<GameObject>("Prefabs/Card_joker");
        if (joker != null)
        {
            deck.Add(Instantiate(joker)); // Prefab�ł͂Ȃ��A���̉������W���[�J�[��ǉ�
        }
    }

    void ShuffleAndDeal()
    {
        // �J�[�h���V���b�t��
        for (int i = 0; i < deck.Count; i++)
        {
            GameObject temp = deck[i];
            int randomIndex = Random.Range(0, deck.Count);
            deck[i] = deck[randomIndex];
            deck[randomIndex] = temp;
        }

        // �J�[�h���v���C���[��NPC�ɔz��
        List<GameObject> tempDeck = new List<GameObject>(deck); // �f�b�L���ꎞ�I�ɃR�s�[

        for (int i = 0; i < tempDeck.Count; i++)
        {
            GameObject card = tempDeck[i];
            if (i % 2 == 0)
            {
                card.transform.SetParent(playerHand);
                Player_deck.hands.Add(tempDeck[i]);
            }
            else
            {
                card.transform.SetParent(npcHand);
                NPC_deck.hands.Add(tempDeck[i]);
            }
            // �J�[�h���z��ꂽ��̓f�b�L����폜
            deck.Remove(card);
            player.CheckForPairs();
        }
    }
}
