using UnityEngine;
using UnityEngine.UI;

public class BossStage : MonoBehaviour
{
    public Enemy TemplateEnemy;
    public TurnManager BattleManager;

    public string BossName = "Boss";
    public int Level = 1;
    public int MaxHp = 200;
    public int Attack = 20;
    public int Defense = 10;
    public int Speed = 10;
    public int Exp = 100;
    public int Gold = 50;
    public float Size = 1.4f;

    private void Awake()
    {
        if (TemplateEnemy == null || BattleManager == null)
        {
            Debug.LogError(gameObject.name + "의 보스 설정이 비어 있습니다.");
            return;
        }

        Transform ExistingBoss = transform.Find(BossName);
        GameObject BossObject;

        if (ExistingBoss != null)
        {
            BossObject = ExistingBoss.gameObject;
        }
        else
        {
            BossObject = Instantiate(TemplateEnemy.gameObject, transform);
            BossObject.name = BossName;
            BossObject.transform.localPosition = new Vector3(2.83f, -0.02f, -1f);
            BossObject.transform.localRotation = TemplateEnemy.transform.localRotation;
            BossObject.transform.localScale = TemplateEnemy.transform.localScale * Size;
        }

        Enemy Boss = BossObject.GetComponent<Enemy>();

        if (Boss.HpSlider == null && TemplateEnemy.transform.childCount > 0)
        {
            GameObject BossUI = Instantiate(TemplateEnemy.transform.GetChild(0).gameObject, Boss.transform);
            Boss.HpSlider = BossUI.GetComponentInChildren<Slider>(true);

            foreach (Button Button in BossUI.GetComponentsInChildren<Button>(true))
            {
                Button.enabled = false;
            }

            foreach (Graphic Graphic in BossUI.GetComponentsInChildren<Graphic>(true))
            {
                Graphic.raycastTarget = false;
            }
        }

        Boss.Level = Level;
        Boss.MaxHp = MaxHp;
        Boss.Hp = MaxHp;
        Boss.Attack = Attack;
        Boss.Defanse = Defense;
        Boss.Speed = Speed;
        Boss.Exp = Exp;
        Boss.Gold = Gold;
        Boss.isDie = false;

        BattleManager.Enemy1 = Boss;
        BattleManager.Enemy2 = null;
        BattleManager.Enemy3 = null;
    }
}
