using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Transform playerHand; // Playerの手札の位置
    public Transform enemyHand;  // Enemyの手札の位置
    public Transform discardPile; // 捨てるカードの位置
    public GameObject replayButton; // リプレイボタン

    private List<GameObject> playerCards = new List<GameObject>();
    private List<GameObject> enemyCards = new List<GameObject>();

    // カードのPrefabを格納する辞書
    public Dictionary<string, GameObject> cardPrefabs;

    private bool isPlayerTurn = true; // プレイヤーのターンかどうか
    private bool isGameOver = false; // ゲーム終了フラグ

    void Start()
    {
        InitializeDeck();
        DealCards();
        CheckForPairs(playerCards, playerHand);
        CheckForPairs(enemyCards, enemyHand);
    }

    // カードデッキの初期化
    void InitializeDeck()
    {
        cardPrefabs = new Dictionary<string, GameObject>();

        // 各スートのカードを追加
        AddSuitCards("diamond");
        AddSuitCards("club");
        AddSuitCards("heart");
        AddSuitCards("spade");

        // ジョーカーを追加
        GameObject jokerPrefab = Resources.Load<GameObject>("Prefabs/Card_joker");  // Prefabフォルダを指定
        if (jokerPrefab == null)
        {
            Debug.LogError("Failed to load Card_joker");
        }
        cardPrefabs.Add("Card_joker", jokerPrefab);
    }

    // スートごとのカードを辞書に追加するメソッド
    void AddSuitCards(string suit)
    {
        for (int i = 1; i <= 13; i++)
        {
            string cardName = $"Card_{suit}_{i}";
            GameObject cardPrefab = Resources.Load<GameObject>($"Prefabs/{cardName}");
            cardPrefabs.Add(cardName, cardPrefab);
        }
    }

    void DealCards()
    {
        List<string> deck = new List<string>(cardPrefabs.Keys);
        Shuffle(deck);

        HashSet<string> usedCards = new HashSet<string>();

        for (int i = 0; i < deck.Count; i++)
        {
            string cardName = deck[i];
            if (!usedCards.Contains(cardName))
            {
                if (cardPrefabs.TryGetValue(cardName, out GameObject cardPrefab))
                {
                    GameObject card = Instantiate(cardPrefab);
                    if (card != null)
                    {
                        usedCards.Add(cardName);
                        if (i % 2 == 0)
                        {
                            card.transform.SetParent(playerHand);
                            playerCards.Add(card);
                        }
                        else
                        {
                            card.transform.SetParent(enemyHand);
                            enemyCards.Add(card);
                        }
                        Debug.Log($"Card created and added: {cardName}");
                    }
                    else
                    {
                        Debug.LogError($"Failed to instantiate card: {cardName}");
                    }
                }
                else
                {
                    Debug.LogError($"Card prefab not found: {cardName}");
                }
            }
            else
            {
                Debug.LogWarning($"Duplicate card skipped: {cardName}");
            }
        }
    }

    // カードをシャッフルする
    void Shuffle(List<string> deck)
    {
        for (int i = 0; i < deck.Count; i++)
        {
            string temp = deck[i];
            int randomIndex = Random.Range(i, deck.Count);
            deck[i] = deck[randomIndex];
            deck[randomIndex] = temp;
        }
    }

    // ペアのチェックと捨てる処理
    void CheckForPairs(List<GameObject> hand, Transform handTransform)
    {
        for (int i = 0; i < hand.Count - 1; i++)
        {
            for (int j = i + 1; j < hand.Count; j++)
            {
                Card card1 = hand[i].GetComponent<Card>();
                Card card2 = hand[j].GetComponent<Card>();

                if (card1.IsPair(card2))
                {
                    // ペアが見つかった場合、捨てる
                    DiscardCard(hand[i]);
                    DiscardCard(hand[j]);
                    hand.RemoveAt(j);
                    hand.RemoveAt(i);
                    i--;
                    break;
                }
            }
        }
    }


    // カードを捨てる
    void DiscardCard(GameObject card)
    {
        card.transform.SetParent(discardPile);
    }

    // プレイヤーが相手のカードを引く
    public void DrawCard(bool isPlayerTurn)
    {
        if (isPlayerTurn)
        {
            int randomIndex = Random.Range(0, enemyCards.Count);
            GameObject drawnCard = enemyCards[randomIndex];
            enemyCards.RemoveAt(randomIndex);
            playerCards.Add(drawnCard);
            drawnCard.transform.SetParent(playerHand);
        }
        else
        {
            int randomIndex = Random.Range(0, playerCards.Count);
            GameObject drawnCard = playerCards[randomIndex];
            playerCards.RemoveAt(randomIndex);
            enemyCards.Add(drawnCard);
            drawnCard.transform.SetParent(enemyHand);
        }

        // 引いた後、ペアのチェックを行う
        CheckForPairs(playerCards, playerHand);
        CheckForPairs(enemyCards, enemyHand);

        CheckForWinCondition();
    }

    // 勝敗判定
    void CheckForWinCondition()
    {
        if (playerCards.Count == 0)
        {
            Debug.Log("Player Wins!");
            replayButton.SetActive(true);
        }
        else if (enemyCards.Count == 0)
        {
            Debug.Log("Enemy Wins!");
            replayButton.SetActive(true);
        }
    }

    // リプレイ機能
    public void Replay()
    {
        playerCards.Clear();
        enemyCards.Clear();
        foreach (Transform child in playerHand) Destroy(child.gameObject);
        foreach (Transform child in enemyHand) Destroy(child.gameObject);
        foreach (Transform child in discardPile) Destroy(child.gameObject);
        replayButton.SetActive(false);
        Start();
    }
}