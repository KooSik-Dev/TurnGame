using UnityEngine;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager instance;

    public int Level = 1;
    public int Exp = 0;
    public int NeedExp = 100;

    public Text LevelText;
    public Text ExpText;

    public int MaxHp = 100;
    public int Hp = 100;

    public Text HpText;

    public int MaxMp = 50;
    public int Mp = 50;

    public Text MpText;

    public int Attack = 20;
    public int Defense = 10;
    public int GuardTurns = 0;
    public int DefenseBuffTurns = 0;
    public int ExtraActions = 0;
    public int Speed = 10;

    public float Critical = 0.1f;
    public float Dodge = 0.15f;

    public int CriticalBuffTurns = 0;
    public int PowerPotionTurns = 0;
    public int KnowledgePotionTurns = 0;
    public int DodgePotionTurns = 0;

    public int Gold = 0;

    public Text GoldText;

    public int RedPotion = 0;
    public int BluePotion = 0;
    public int PowerPotion = 0;
    public int KnowledgePotion = 0;
    public int DodgePotion = 0;

    // 1~6번은 기본 스킬이고, 7~13번은 상점에서 구매하는 스킬이다.
    public bool ShopSkill7 = false;
    public bool ShopSkill8 = false;
    public bool ShopSkill9 = false;
    public bool ShopSkill10 = false;
    public bool ShopSkill11 = false;
    public bool ShopSkill12 = false;
    public bool ShopSkill13 = false;

    public Slider HpSlider;
    public Slider MpSlider;
    public Slider ExpSlider;







    private int SaveLevel;
    private int SaveExp;
    private int SaveNeedExp;

    private int SaveMaxHp;
    private int SaveHp;

    private int SaveMaxMp;
    private int SaveMp;

    private int SaveAttack;
    private int SaveDefense;
    private int SaveSpeed;

    private float SaveCritical;
    private float SaveDodge;

    private int SaveGold;

    private int SaveRedPotion;
    private int SaveBluePotion;
    private int SavePowerPotion;
    private int SaveKnowledgePotion;
    private int SaveDodgePotion;

    // 인덱스는 스킬 번호(1~13), 값은 남은 쿨타임이다.
    private int[] SkillCooldowns = new int[14];










    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        UpdateUI();
    }

    public void SaveBattleState()
    {
        SaveLevel = Level;
        SaveExp = Exp;
        SaveNeedExp = NeedExp;

        SaveMaxHp = MaxHp;
        SaveHp = Hp;

        SaveMaxMp = MaxMp;
        SaveMp = Mp;

        SaveAttack = Attack;
        SaveDefense = Defense;
        SaveSpeed = Speed;

        SaveCritical = Critical;
        SaveDodge = Dodge;

        SaveGold = Gold;

        SaveRedPotion = RedPotion;
        SaveBluePotion = BluePotion;
        SavePowerPotion = PowerPotion;
        SaveKnowledgePotion = KnowledgePotion;
        SaveDodgePotion = DodgePotion;

        Debug.Log("���� �� ����");
    }

    public void LoadBattleState()
    {
         Level = SaveLevel;
         Exp = SaveExp;
         NeedExp = SaveNeedExp;

         MaxHp = SaveMaxHp;
         Hp = SaveHp;

         MaxMp = SaveMaxMp;
         Mp = SaveMp;

         Attack = SaveAttack;
         Defense = SaveDefense;
         Speed = SaveSpeed;

         Critical = SaveCritical;
         Dodge = SaveDodge;

         Gold = SaveGold;

         RedPotion = SaveRedPotion;
         BluePotion = SaveBluePotion;
         PowerPotion = SavePowerPotion;
         KnowledgePotion = SaveKnowledgePotion;
         DodgePotion = SaveDodgePotion;
         CriticalBuffTurns = 0;
         GuardTurns = 0;
         DefenseBuffTurns = 0;
         ExtraActions = 0;
         PowerPotionTurns = 0;
         KnowledgePotionTurns = 0;
         DodgePotionTurns = 0;
         ResetSkillCooldowns();

        UpdateUI();

        Debug.Log("���� �� ����");
    }

    public void TakeDamage(int Damage)
    {
        float CurrentDefense = Defense;

        if (DefenseBuffTurns > 0)
        {
            CurrentDefense *= 1.3f;
        }

        int FinalDamage = Mathf.RoundToInt(Damage * (1f - CurrentDefense / 100f));
        int MinimumDamage = Mathf.CeilToInt(Damage * 0.1f);

        if (FinalDamage < MinimumDamage)
        {
            FinalDamage = MinimumDamage;
        }

        if (GuardTurns > 0)
        {
            FinalDamage = Mathf.RoundToInt(FinalDamage * 0.7f);
        }

        Hp -= FinalDamage;

        if (Hp < 0)
        {
            Hp = 0;
        }

        UpdateUI();

        if (Hp <= 0)
        {
            if (TurnManager.instance != null)
            {
                TurnManager.instance.BattleFail();
            }
        }
    }

    public void Heal(int Heal)
    {
        Hp += Heal;

        if (Hp > MaxHp)
        {
            Hp = MaxHp;
        }

        UpdateUI();
    }

    public bool UseMp(int UseMp)
    {
        if (Mp < UseMp)
        {
            Debug.Log("MP ����");

            return false;
        }

        Mp -= UseMp;

        UpdateUI();

        return true;
    }

    public void AddExp(int GetExp)
    {
        Exp += GetExp;

        LevelUpCheck();

        UpdateUI();
    }

    public void LevelUpCheck()
    {
        if (Exp < NeedExp) return;

        if (Level >= 10) return;

        Exp -= NeedExp;

        Level++;

        MaxHp += 20;
        MaxMp += 10;
        Attack += 10;

        NeedExp += 100;

        Debug.Log("���� �� : " +  Level);

        if (Exp >= NeedExp)
        {
            LevelUpCheck();
        }
    }

    public void AddGold(int GetGold)
    {
        Gold += GetGold;
        UpdateUI();
    }

    public void RestoreMp(int Amount)
    {
        Mp = Mathf.Min(MaxMp, Mp + Amount);
        UpdateUI();
    }

    public bool TrySpendGold(int Price)
    {
        if (Gold < Price)
        {
            return false;
        }

        Gold -= Price;
        UpdateUI();
        return true;
    }

    public void AddShopItem(int ItemNumber)
    {
        if (CanAddShopItem(ItemNumber) == false) return;

        if (ItemNumber == 1) RedPotion = Mathf.Min(5, RedPotion + 1);
        if (ItemNumber == 2) BluePotion = Mathf.Min(5, BluePotion + 1);
        if (ItemNumber == 3) PowerPotion = Mathf.Min(1, PowerPotion + 1);
        if (ItemNumber == 4) KnowledgePotion = Mathf.Min(1, KnowledgePotion + 1);
        if (ItemNumber == 5) DodgePotion = Mathf.Min(1, DodgePotion + 1);

        UpdateUI();
    }

    public bool CanAddShopItem(int ItemNumber)
    {
        if (ItemNumber == 1) return RedPotion < 5;
        if (ItemNumber == 2) return BluePotion < 5;
        if (ItemNumber == 3) return PowerPotion < 1;
        if (ItemNumber == 4) return KnowledgePotion < 1;
        if (ItemNumber == 5) return DodgePotion < 1;
        return false;
    }

    public bool UseItemCount(int ItemNumber)
    {
        if (GetItemCount(ItemNumber) <= 0) return false;

        if (ItemNumber == 1) RedPotion--;
        if (ItemNumber == 2) BluePotion--;
        if (ItemNumber == 3) PowerPotion--;
        if (ItemNumber == 4) KnowledgePotion--;
        if (ItemNumber == 5) DodgePotion--;

        UpdateUI();
        return true;
    }

    public int GetItemCount(int ItemNumber)
    {
        if (ItemNumber == 1) return RedPotion;
        if (ItemNumber == 2) return BluePotion;
        if (ItemNumber == 3) return PowerPotion;
        if (ItemNumber == 4) return KnowledgePotion;
        if (ItemNumber == 5) return DodgePotion;
        return 0;
    }

    public int GetCurrentAttack()
    {
        if (PowerPotionTurns > 0)
        {
            return Mathf.RoundToInt(Attack * 1.3f);
        }

        return Attack;
    }

    public int CalculateSkillDamage(float DamageRate)
    {
        float Damage = GetCurrentAttack() * DamageRate;

        if (KnowledgePotionTurns > 0)
        {
            Damage *= 1.3f;
        }

        return Mathf.RoundToInt(Damage);
    }

    public float GetCurrentDodge()
    {
        if (DodgePotionTurns > 0)
        {
            return Mathf.Clamp01(Dodge * 2f);
        }

        return Mathf.Clamp01(Dodge);
    }

    public void CountPotionBuffTurns()
    {
        if (PowerPotionTurns > 0) PowerPotionTurns--;
        if (KnowledgePotionTurns > 0) KnowledgePotionTurns--;
        if (DodgePotionTurns > 0) DodgePotionTurns--;
    }

    public int GetSkillCooldown(int SkillNumber)
    {
        if (SkillNumber < 1 || SkillNumber >= SkillCooldowns.Length)
        {
            return 0;
        }

        return SkillCooldowns[SkillNumber];
    }

    public void StartSkillCooldown(int SkillNumber)
    {
        int Cooldown = GetRequiredSkillCooldown(SkillNumber);

        if (Cooldown > 0)
        {
            // 현재 행동이 끝날 때 바로 1 감소하므로 1을 더해서 저장한다.
            SkillCooldowns[SkillNumber] = Cooldown + 1;
        }
    }

    public void CountSkillCooldowns()
    {
        for (int SkillNumber = 1; SkillNumber < SkillCooldowns.Length; SkillNumber++)
        {
            if (SkillCooldowns[SkillNumber] > 0)
            {
                SkillCooldowns[SkillNumber]--;
            }
        }
    }

    public void ResetSkillCooldowns()
    {
        for (int SkillNumber = 1; SkillNumber < SkillCooldowns.Length; SkillNumber++)
        {
            SkillCooldowns[SkillNumber] = 0;
        }
    }

    private int GetRequiredSkillCooldown(int SkillNumber)
    {
        if (SkillNumber == 4) return 5;
        if (SkillNumber == 5) return 10;
        if (SkillNumber == 8) return 2;
        if (SkillNumber == 10) return 2;
        if (SkillNumber == 11) return 1;
        if (SkillNumber == 12) return 3;
        if (SkillNumber == 13) return 10;
        return 0;
    }

    public void AddShopSkill(int SkillNumber)
    {
        if (SkillNumber == 7) ShopSkill7 = true;
        if (SkillNumber == 8) ShopSkill8 = true;
        if (SkillNumber == 9) ShopSkill9 = true;
        if (SkillNumber == 10) ShopSkill10 = true;
        if (SkillNumber == 11) ShopSkill11 = true;
        if (SkillNumber == 12) ShopSkill12 = true;
        if (SkillNumber == 13) ShopSkill13 = true;
    }

    public bool HasShopSkill(int SkillNumber)
    {
        if (SkillNumber == 7) return ShopSkill7;
        if (SkillNumber == 8) return ShopSkill8;
        if (SkillNumber == 9) return ShopSkill9;
        if (SkillNumber == 10) return ShopSkill10;
        if (SkillNumber == 11) return ShopSkill11;
        if (SkillNumber == 12) return ShopSkill12;
        if (SkillNumber == 13) return ShopSkill13;
        return false;
    }

    public void StartCriticalBuff()
    {
        CriticalBuffTurns = 3;
    }

    public float GetCriticalChance()
    {
        if (CriticalBuffTurns > 0)
        {
            return Critical + 0.25f;
        }

        return Critical;
    }

    public void CountCriticalBuffTurn()
    {
        if (CriticalBuffTurns > 0)
        {
            CriticalBuffTurns--;
        }
    }

    public void RetryCurrentBattle()
    {
        if (TurnManager.instance != null)
        {
            TurnManager.instance.RetryBattle();
        }
    }

    public void UpdateUI()
    {
        LevelText.text = Level.ToString();
        if (HpSlider != null)
        {
            HpSlider.maxValue = MaxHp;
            HpSlider.value = Hp;
            HpText.text = Hp.ToString();
        }

        if (MpSlider != null)
        {
            MpSlider.maxValue = MaxMp;
            MpSlider.value = Mp;
            MpText.text = Mp.ToString();
        }

        if (ExpSlider != null)
        {
            ExpSlider.maxValue = NeedExp;
            ExpSlider.value = Exp;
            ExpText.text = Exp.ToString();
        }

        if (GoldText != null)
        {
            GoldText.text = Gold.ToString();
        }
    }
}
