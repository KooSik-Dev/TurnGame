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

    public bool PlayerTurn = false;
    public bool BattleEnded = false;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        if (CheckBattleClear() == true)
        {
            return;
        }

        PlayerTurn = true;
        TurnText.text = "플레이어의 턴";
    }

    public bool CheckBattleClear()
    {
        bool EnemyDie1 = Enemy1 == null || Enemy1.isDie == true;
        bool EnemyDie2 = Enemy2 == null || Enemy2.isDie == true;
        bool EnemyDie3 = Enemy3 == null || Enemy3.isDie == true;

        if (EnemyDie1 && EnemyDie2 && EnemyDie3)
        {
            BattleEnded = true;
            PlayerTurn = false;

            TurnText.text = "전투 승리!";

            Debug.Log("전투 승리!");

            return true;
        }

        return false;
    }

    public void EndTurn()
    {
        if (CheckBattleClear() == true)
        {
            return;
        }

        PlayerTurn = false;

        StartCoroutine(EnemyTurn());
    }

    public IEnumerator EnemyTurn()
    {
        yield return new WaitForSeconds(1f);

        TurnText.text = "적의 턴";

        if (Enemy1 != null && Enemy1.isDie == false)
        {
            Enemy1.EnemyAttack();

            yield return new WaitForSeconds(1);
        }

        if (Enemy2 != null && Enemy2.isDie == false)
        {
            Enemy2.EnemyAttack();

            yield return new WaitForSeconds(1);
        }

        if (Enemy3 != null && Enemy3.isDie == false)
        {
            Enemy3.EnemyAttack();

            yield return new WaitForSeconds(1);
        }

        PlayerTurn = true;
        TurnText.text = "플레이어 턴";
    }


}
