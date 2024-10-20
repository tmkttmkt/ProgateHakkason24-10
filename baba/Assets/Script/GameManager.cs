using System.Collections;
using System.Collections.Generic;
using UnityEditor.MemoryProfiler;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    // �V���O���g���̃C���X�^���X
    public static GameManager Instance { get; private set; }

    public NPC npc; // NPC�̃X�N���v�g���Q��
    public Player player; // �v���C���[�̃X�N���v�g���Q��
    public Text resultText; // ���s���ʂ�\������UI�e�L�X�g
    private int isPlayerTurn; // ���݂̃^�[�����v���C���[���ǂ���
    public ConnectionTest connection;
    public Vector2 Vector2;

    void Awake()
    {
        // �C���X�^���X��ݒ�i�V���O���g���̃Z�b�g�A�b�v�j
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject); // ������GameManager�����݂��Ȃ��悤�ɂ���
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

        // �����_���Ƀ^�[�������߂�
        isPlayerTurn = Random.Range(0, 2);
        StartTurn();
    }

    public void StartTurn()
    {
        if (player == null || npc == null)
        {
            Debug.LogError("Player or NPC is not assigned in StartTurn!");
            return; // �Q�Ƃ� null �Ȃ�^�[�����J�n���Ȃ�
        }

        if (isPlayerTurn != 0)
        {
            Debug.Log("Player's Turn");
            StartCoroutine(PlayerTurn()); // �v���C���[���J�[�h�����
        }
        else
        {
            Debug.Log("NPC's Turn");
            StartCoroutine(NPCTurn()); // NPC�̃^�[�����J�n
        }
    }
    IEnumerator PlayerTurn()
    {
        if (npc == null || player == null || player.hands == null)
        {
            Debug.LogError("NPC or Player is not properly initialized.");
            yield break; // NPC��Player������������Ă��Ȃ��ꍇ�A�R���[�`�����I��
        }
        yield return new WaitForSeconds(1f);
        yield return connection.GetData();
        player.TakeTurn(Vector2);

    }


    IEnumerator NPCTurn()
    {
        if (npc == null || player == null || player.hands == null)
        {
            Debug.LogError("NPC or Player is not properly initialized.");
            yield break; // NPC��Player������������Ă��Ȃ��ꍇ�A�R���[�`�����I��
        }

        // NPC�̃^�[���̏���
        yield return new WaitForSeconds(1f); // NPC�̎v�l���Ԃ��V�~�����[�V����
        npc.PickCard(player.hands); // NPC���v���C���[�̎�D����J�[�h�����
        EndTurn(); // �^�[�����I��
    }

    public void EndTurn()
    {
        // �y�A������΍폜 (�v���C���[��NPC�̗����Ń`�F�b�N)
        player.CheckForPairs();
        npc.CheckForPairs();

        // ���s�`�F�b�N
        //if (CheckForWinner()) return;

        // �^�[�������
        isPlayerTurn = (isPlayerTurn + 1) % 2;//ターンを入れ替える
        StartTurn();
    }

    /*bool CheckForWinner()
    {
        // �v���C���[���W���[�J�[1�������̏ꍇ
        if (player.hand.Count == 1 && player.hand[0].GetComponent<Card>().number == 14)
        {
            resultText.text = "NPC Wins!";
            Debug.Log("NPC Wins!");
            return true; // ���s�������̂�true��Ԃ�
        }

        // NPC���W���[�J�[1�������̏ꍇ
        if (npc.hand.Count == 1 && npc.hand[0].GetComponent<Card>().number == 14)
        {
            resultText.text = "Player Wins!";
            Debug.Log("Player Wins!");
            return true; // ���s�������̂�true��Ԃ�
        }

        return false; // �܂����s�����Ă��Ȃ��ꍇ
    }*/
}
