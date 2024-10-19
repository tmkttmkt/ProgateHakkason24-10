using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    // シングルトンのインスタンス
    public static GameManager Instance { get; private set; }

    public NPC npc; // NPCのスクリプトを参照
    public Player player; // プレイヤーのスクリプトを参照
    public Text resultText; // 勝敗結果を表示するUIテキスト
    private int isPlayerTurn; // 現在のターンがプレイヤーかどうか

    void Awake()
    {
        // インスタンスを設定（シングルトンのセットアップ）
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject); // 複数のGameManagerが存在しないようにする
        }
    }

    void Start()
    {
        if (player == null)
        {
            Debug.LogError("Player is not assigned in GameManager!");
        }

        if (npc == null)
        {
            Debug.LogError("NPC is not assigned in GameManager!");
        }

        // ランダムにターンを決める
        isPlayerTurn = Random.Range(0, 2);
        StartTurn();
    }

    void StartTurn()
    {
        if (player == null || npc == null)
        {
            Debug.LogError("Player or NPC is not assigned in StartTurn!");
            return; // 参照が null ならターンを開始しない
        }

        if (isPlayerTurn != 0)
        {
            Debug.Log("Player's Turn");
            player.TakeTurn(); // プレイヤーがカードを取る
        }
        else
        {
            Debug.Log("NPC's Turn");
            StartCoroutine(NPCTurn()); // NPCのターンを開始
        }
    }


    IEnumerator NPCTurn()
    {
        if (npc == null || player == null || player.hands == null)
        {
            Debug.LogError("NPC or Player is not properly initialized.");
            yield break; // NPCやPlayerが初期化されていない場合、コルーチンを終了
        }

        // NPCのターンの処理
        yield return new WaitForSeconds(1f); // NPCの思考時間をシミュレーション
        npc.PickCard(player.hands); // NPCがプレイヤーの手札からカードを取る
        EndTurn(); // ターンを終了
    }

    public void EndTurn()
    {
        // ペアがあれば削除 (プレイヤーとNPCの両方でチェック)
        player.CheckForPairs();
        npc.CheckForPairs();

        // 勝敗チェック
        //if (CheckForWinner()) return;

        // ターンを交代
        isPlayerTurn = (isPlayerTurn + 1) % 2;
        StartTurn();
    }

    /*bool CheckForWinner()
    {
        // プレイヤーがジョーカー1枚だけの場合
        if (player.hand.Count == 1 && player.hand[0].GetComponent<Card>().number == 14)
        {
            resultText.text = "NPC Wins!";
            Debug.Log("NPC Wins!");
            return true; // 勝敗がついたのでtrueを返す
        }

        // NPCがジョーカー1枚だけの場合
        if (npc.hand.Count == 1 && npc.hand[0].GetComponent<Card>().number == 14)
        {
            resultText.text = "Player Wins!";
            Debug.Log("Player Wins!");
            return true; // 勝敗がついたのでtrueを返す
        }

        return false; // まだ勝敗がついていない場合
    }*/
}
