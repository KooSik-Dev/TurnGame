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

    private bool InitialTurnStarted = false;

    private void OnEnable()
    {
        instance = this;
        UpdateStageText();
    }

    private void Start()
    {
        // BeginBattle이 Start보다 먼저 호출된 경우 첫 턴을 중복 실행하지 않는다.
        if (InitialTurnStarted)
        {
            return;
        }

        if (WinUI != null) WinUI.SetActive(false);
        if (LossUI != null) LossUI.SetActive(false);

        PlayerManager.instance.SaveBattleState();
        BattleEnded = false;
        StartFirstTurnBySpeed();
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

        if (PlayerManager.instance != null && PlayerPrefs.GetInt("Type", 0) == 2)
        {
            int UsedSkillNumber = PlayerPrefs.GetInt("TypeNumber", 0);
            PlayerManager.instance.StartSkillCooldown(UsedSkillNumber);
        }

        if (CountBuffTurn && PlayerManager.instance != null)
        {
            PlayerManager.instance.CountCriticalBuffTurn();
        }

        if (PlayerManager.instance != null)
        {
            PlayerManager.instance.CountSkillCooldowns();
        }

        if (PlayerManager.instance != null && PlayerManager.instance.ExtraActions > 0)
        {
            PlayerManager.instance.ExtraActions--;
            PlayerTurn = true;

            if (TurnText != null)
            {
                TurnText.text = "추가 행동! 한 번 더 행동하세요";
            }

            return;
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
            Enemy1.PerformPlannedAction();
            yield return new WaitForSeconds(1f);

            if (BattleEnded) yield break;
        }

        if (Enemy2 != null && Enemy2.isDie == false)
        {
            ShowBattleMessage("적의 턴");
            Enemy2.PerformPlannedAction();
            yield return new WaitForSeconds(1f);

            if (BattleEnded) yield break;
        }

        if (Enemy3 != null && Enemy3.isDie == false)
        {
            ShowBattleMessage("적의 턴");
            Enemy3.PerformPlannedAction();
            yield return new WaitForSeconds(1f);

            if (BattleEnded) yield break;
        }

        if (PlayerManager.instance != null && PlayerManager.instance.GuardTurns > 0)
        {
            PlayerManager.instance.GuardTurns--;
            Debug.Log("가드 남은 턴 : " + PlayerManager.instance.GuardTurns);
        }

        if (PlayerManager.instance != null && PlayerManager.instance.DefenseBuffTurns > 0)
        {
            PlayerManager.instance.DefenseBuffTurns--;
            Debug.Log("공방일체 방어 증가 남은 턴 : " + PlayerManager.instance.DefenseBuffTurns);
        }

        if (PlayerManager.instance != null)
        {
            PlayerManager.instance.CountPotionBuffTurns();
        }

        PlanEnemyActions();
        PlayerTurn = true;
        ShowPlayerTurnAndDefenseWarning();
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
        StartFirstTurnBySpeed();
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
            PlayerManager.instance.GuardTurns = 0;
            PlayerManager.instance.DefenseBuffTurns = 0;
            PlayerManager.instance.ExtraActions = 0;
            PlayerManager.instance.PowerPotionTurns = 0;
            PlayerManager.instance.KnowledgePotionTurns = 0;
            PlayerManager.instance.DodgePotionTurns = 0;
            PlayerManager.instance.ResetSkillCooldowns();
            PlayerManager.instance.UpdateUI();

            // 재도전해도 새 스테이지 시작 상태로 돌아오도록 저장한다.
            PlayerManager.instance.SaveBattleState();
        }

        ResetEnemies();

        if (WinUI != null) WinUI.SetActive(false);
        if (LossUI != null) LossUI.SetActive(false);

        BattleEnded = false;
        UpdateStageText();

        StartFirstTurnBySpeed();
    }

    private void ResetEnemies()
    {
        if (Enemy1 != null) Enemy1.ResetEnemy();
        if (Enemy2 != null) Enemy2.ResetEnemy();
        if (Enemy3 != null) Enemy3.ResetEnemy();
    }

    private void PlanEnemyActions()
    {
        if (Enemy1 != null) Enemy1.PlanNextAction();
        if (Enemy2 != null) Enemy2.PlanNextAction();
        if (Enemy3 != null) Enemy3.PlanNextAction();
    }

    private void StartFirstTurnBySpeed()
    {
        InitialTurnStarted = true;
        PlanEnemyActions();

        int PlayerSpeed = PlayerManager.instance != null ? PlayerManager.instance.Speed : 0;
        int FastestEnemySpeed = GetFastestEnemySpeed();

        if (FastestEnemySpeed > PlayerSpeed)
        {
            PlayerTurn = false;
            ShowBattleMessage("적 선공! 적 속도 " + FastestEnemySpeed + " / 플레이어 속도 " + PlayerSpeed);
            Debug.Log("적 선공 : " + FastestEnemySpeed + " > " + PlayerSpeed);
            StartCoroutine(EnemyTurn());
        }
        else
        {
            PlayerTurn = true;
            ShowPlayerTurnAndDefenseWarning();
            Debug.Log("플레이어 선공 : " + PlayerSpeed + " >= " + FastestEnemySpeed);
        }
    }

    private int GetFastestEnemySpeed()
    {
        int FastestSpeed = 0;

        AddEnemySpeed(Enemy1, ref FastestSpeed);
        AddEnemySpeed(Enemy2, ref FastestSpeed);
        AddEnemySpeed(Enemy3, ref FastestSpeed);

        return FastestSpeed;
    }

    private void AddEnemySpeed(Enemy TargetEnemy, ref int FastestSpeed)
    {
        if (TargetEnemy == null || TargetEnemy.isDie)
        {
            return;
        }

        if (TargetEnemy.Speed > FastestSpeed)
        {
            FastestSpeed = TargetEnemy.Speed;
        }
    }

    private void ShowPlayerTurnAndDefenseWarning()
    {
        string GuardEnemies = "";

        AddGuardEnemyName(Enemy1, ref GuardEnemies);
        AddGuardEnemyName(Enemy2, ref GuardEnemies);
        AddGuardEnemyName(Enemy3, ref GuardEnemies);

        if (string.IsNullOrEmpty(GuardEnemies))
        {
            ShowBattleMessage("플레이어 턴");
        }
        else
        {
            ShowBattleMessage("플레이어 턴 | 방어 예정: " + GuardEnemies);
        }
    }

    private void AddGuardEnemyName(Enemy TargetEnemy, ref string GuardEnemies)
    {
        if (TargetEnemy == null || TargetEnemy.isDie || TargetEnemy.NextActionIsGuard == false)
        {
            return;
        }

        if (string.IsNullOrEmpty(GuardEnemies) == false)
        {
            GuardEnemies += ", ";
        }

        GuardEnemies += TargetEnemy.gameObject.name;
    }
}
