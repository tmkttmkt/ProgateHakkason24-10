using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DeckManager : MonoBehaviour
{
    public List<GameObject> deck = new List<GameObject>(); // デッキ
    public Transform playerHand; // プレイヤーの手札位置
    public Transform npcHand; // NPCの手札位置
    public GameObject cardBackPrefab; // カードの裏面Prefab
    Player player = new Player();
    public Player Player_deck;
    public NPC NPC_deck;

    void Start()
    {
        // デッキを生成する
        GenerateDeck();
        // カードをシャッフルして配る
        ShuffleAndDeal();
    }

    void GenerateDeck()
    {
        // Resources/PrefabsからカードPrefabをロード
        for (int i = 1; i <= 13; i++)
        {
            foreach (string suit in new string[] { "heart", "diamond", "club", "spade" })
            {
                GameObject cardPrefab = Resources.Load<GameObject>("Prefabs/" + "Card_" + suit + "_" + i);
                if (cardPrefab != null)
                {
                    deck.Add(Instantiate(cardPrefab)); // Prefabではなく、実体化したカードを追加
                }
            }
        }
        // ジョーカーを追加
        GameObject joker = Resources.Load<GameObject>("Prefabs/Card_joker");
        if (joker != null)
        {
            deck.Add(Instantiate(joker)); // Prefabではなく、実体化したジョーカーを追加
        }
    }

    void ShuffleAndDeal()
    {
        // カードをシャッフル
        for (int i = 0; i < deck.Count; i++)
        {
            GameObject temp = deck[i];
            int randomIndex = Random.Range(0, deck.Count);
            deck[i] = deck[randomIndex];
            deck[randomIndex] = temp;
        }

        // カードをプレイヤーとNPCに配る
        List<GameObject> tempDeck = new List<GameObject>(deck); // デッキを一時的にコピー

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
            // カードが配られた後はデッキから削除
            deck.Remove(card);
            player.CheckForPairs();
        }
    }
}
