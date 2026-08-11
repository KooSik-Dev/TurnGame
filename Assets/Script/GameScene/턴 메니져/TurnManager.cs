using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class TurnManager : MonoBehaviour
{
    public static TurnManager instance;

    public Enemy Enemy1;
    public Enemy Enemy2;
    public Enemy Enemy3;

    public Text TurnText;
    public Text StageText;
    public string StageName;

    public GameObject WinUI;
    public GameObject LossUI;

    public bool PlayerTurn = false;
    public bool BattleEnded = false;

    private void OnEnable()
    {
        instance = this;
        UpdateStageText();
    }

    private void Start()
    {
        if (WinUI != null) WinUI.SetActive(false);
        if (LossUI != null) LossUI.SetActive(false);

        PlayerManager.instance.SaveBattleState();
        BattleEnded = false;
        PlayerTurn = true;

        if (TurnText != null) TurnText.text = "플레이어 턴";
    }

    private void UpdateStageText()
    {
        if (StageText != null && string.IsNullOrEmpty(StageName) == false)
        {
            StageText.text = "스테이지 " + StageName;
        }
    }

    public bool CheckBattleClear()
    {
        bool EnemyDie1 = Enemy1 == null || Enemy1.isDie;
        bool EnemyDie2 = Enemy2 == null || Enemy2.isDie;
        bool EnemyDie3 = Enemy3 == null || Enemy3.isDie;

        if (EnemyDie1 && EnemyDie2 && EnemyDie3)
        {
            BattleEnded = true;
            PlayerTurn = false;

            if (WinUI != null) WinUI.SetActive(true);
            if (TurnText != null) TurnText.text = "전투 승리!";

            Debug.Log("전투 승리!");

            if (MapManager.instance != null)
            {
                MapManager.instance.CompleteBattle(StageName);
            }

            return true;
        }

        return false;
    }

    public void EndTurn(bool CountBuffTurn = true)
    {
        if (CheckBattleClear()) return;

        if (CountBuffTurn && PlayerManager.instance != null)
        {
            PlayerManager.instance.CountCriticalBuffTurn();
        }

        PlayerTurn = false;
        StartCoroutine(EnemyTurn());
    }

    public void ShowBattleMessage(string Message)
    {
        if (TurnText != null) TurnText.text = Message;
    }

    public IEnumerator EnemyTurn()
    {
        yield return new WaitForSeconds(1f);

        if (TurnText != null) TurnText.text = "적의 턴";

        if (Enemy1 != null && Enemy1.isDie == false)
        {
            ShowBattleMessage("적의 턴");
            Enemy1.EnemyAttack();
            yield return new WaitForSeconds(1f);
        }

        if (Enemy2 != null && Enemy2.isDie == false)
        {
            ShowBattleMessage("적의 턴");
            Enemy2.EnemyAttack();
            yield return new WaitForSeconds(1f);
        }

        if (Enemy3 != null && Enemy3.isDie == false)
        {
            ShowBattleMessage("적의 턴");
            Enemy3.EnemyAttack();
            yield return new WaitForSeconds(1f);
        }

        PlayerTurn = true;
        if (TurnText != null) TurnText.text = "플레이어 턴";
    }

    public void BattleFail()
    {
        if (BattleEnded) return;

        BattleEnded = true;
        PlayerTurn = false;
        StopAllCoroutines();

        if (LossUI != null) LossUI.SetActive(true);
        if (TurnText != null) TurnText.text = "전투 실패";

        Debug.Log("전투 실패!");
    }

    public void RetryBattle()
    {
        StopAllCoroutines();
        PlayerManager.instance.LoadBattleState();
        ResetEnemies();

        if (WinUI != null) WinUI.SetActive(false);
        if (LossUI != null) LossUI.SetActive(false);

        BattleEnded = false;
        PlayerTurn = true;

        if (TurnText != null) TurnText.text = "플레이어 턴";
        Debug.Log("전투를 다시 시작합니다.");
    }

    public void BeginBattle()
    {
        StopAllCoroutines();

        if (PlayerManager.instance != null)
        {
            // 다음 스테이지는 HP와 MP를 모두 채운 상태로 시작한다.
            PlayerManager.instance.Hp = PlayerManager.instance.MaxHp;
            PlayerManager.instance.Mp = PlayerManager.instance.MaxMp;
            PlayerManager.instance.CriticalBuffTurns = 0;
            PlayerManager.instance.UpdateUI();

            // 재도전해도 새 스테이지 시작 상태로 돌아오도록 저장한다.
            PlayerManager.instance.SaveBattleState();
        }

        ResetEnemies();

        if (WinUI != null) WinUI.SetActive(false);
        if (LossUI != null) LossUI.SetActive(false);

        BattleEnded = false;
        PlayerTurn = true;
        UpdateStageText();

        if (TurnText != null) TurnText.text = "플레이어 턴";
    }

    private void ResetEnemies()
    {
        if (Enemy1 != null) Enemy1.ResetEnemy();
        if (Enemy2 != null) Enemy2.ResetEnemy();
        if (Enemy3 != null) Enemy3.ResetEnemy();
    }
}
