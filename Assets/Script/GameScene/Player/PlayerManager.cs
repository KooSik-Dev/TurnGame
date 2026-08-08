using UnityEngine;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager instance;

    public int Level = 1;
    public int Exp = 0;
    public int NeedExp = 100;

    public int MaxHp = 100;
    public int Hp = 100;

    public int MaxMp = 50;
    public int Mp = 50;

    public int Attack = 20;
    public int Defense = 10;
    public int Speed = 10;

    public float Critical = 0.1f;
    public float Dodge = 0.15f;

    public int Gold = 0;

    public int RedPotion = 0;
    public int BluePotion = 0;
    public int PowerPotion = 0;
    public int KnowledgePotion = 0;
    public int DodgePotion = 0;

    public Slider HpSlider;
    public Slider MpSlider;
    public Slider ExpSlider;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        UpdateUI();
    }

    public void TakeDamage(int Damage)
    {
        Hp -= Damage;
        if (Hp < 0)
        {
            Hp = 0;
        }

        UpdateUI();

        if (Hp <= 0)
        {
            Debug.Log("플레이어 사망");
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
            Debug.Log("MP 부족");

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

        Debug.Log("레벨 업 : " +  Level);

        if (Exp >= NeedExp)
        {
            LevelUpCheck();
        }
    }

    public void addGold(int GetGold)
    {
        Gold += GetGold;
    }

    public void UpdateUI()
    {
        if (HpSlider != null)
        {
            HpSlider.maxValue = MaxHp;
            HpSlider.value = Hp;
        }

        if (MpSlider != null)
        {
            MpSlider.maxValue = MaxMp;
            MpSlider.value = Mp;
        }

        if (ExpSlider != null)
        {
            ExpSlider.maxValue = NeedExp;
            ExpSlider.value = Exp;
        }
    }
}
