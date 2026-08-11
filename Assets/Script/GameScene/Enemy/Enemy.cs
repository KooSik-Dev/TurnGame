using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    public int Level = 1;

    public int MaxHp = 60;
    public int Hp = 60;

    public int Attack = 10;
    public int Defanse = 0;
    public int Speed = 8;

    public int Exp = 20;
    public int Gold = 10;

    public bool isDie = false;

    public Slider HpSlider;
    public Animator Animator;
    public ParticleSystem EnemyDamege;
    public ParticleSystem PlayerDamege;

    public AudioSource PlayerDamegeAudio;
    public AudioSource EnemyDamegeAudio;

    void Start()
    {
        Hp = MaxHp;

        SetupEnemyUI();

        if (HpSlider != null)
        {
            HpSlider.maxValue = MaxHp;
            HpSlider.value = Hp;
        }
    }

    private void OnMouseDown()
    {
        ClickButton();
    }

    public void ClickButton()
    {
        if (isDie == true) return;

        if (TurnManager.instance.PlayerTurn == false) return;


        if (EnemyDamege != null)
        {
            EnemyDamege.Play();
        }

        if (EnemyDamegeAudio != null)
        {
            EnemyDamegeAudio.volume = PlayerPrefs.GetFloat("SFX", 0.8f);
            EnemyDamegeAudio.Play();
        }

        int Type = PlayerPrefs.GetInt("Type", 0);
        int TypeNumber = PlayerPrefs.GetInt("TypeNumber", 0);

        if (Type == 1 && TypeNumber == 1)
        {
            NormalAttack();
        }

        if (Type == 2)
        {
            Skill(TypeNumber);
        }

        if (Type == 3)
        {
            Item(TypeNumber);
        }
    }

    public void NormalAttack()
    {
        TurnManager.instance.PlayerTurn = false;

        PlayerMove.instance.AttackAnimation();

        int Damage = PlayerManager.instance.Attack;
        bool IsCritical = Random.value < Mathf.Clamp01(PlayerManager.instance.GetCriticalChance());

        if (IsCritical == true)
        {
            Damage *= 2;
            TurnManager.instance.ShowBattleMessage("크리티컬!");
            Debug.Log("크리티컬 공격! 데미지 : " + Damage);
        }

        TakeDamage(Damage);

        Debug.Log("기본 공격");

        TurnManager.instance.EndTurn();
    }

    public void Skill(int Number)
    {
        if (Number == 1)
        {
            if (PlayerManager.instance.UseMp(30) == false)
            {
                TurnManager.instance.ShowBattleMessage("MP가 부족 합니다.");
                return;
            }
            TurnManager.instance.PlayerTurn = false;

            PlayerMove.instance.AttackAnimation();

            int Damage = Mathf.RoundToInt(PlayerManager.instance.Attack * 1.7f);

            TakeDamage(Damage);

            TurnManager.instance.ShowBattleMessage("배기!");
            Debug.Log("배기 피해량 : " + Damage);

            TurnManager.instance.EndTurn();
            return;
        }
        if (Number == 2)
        {
            if (PlayerManager.instance.UseMp(35) == false)
            {
                TurnManager.instance.ShowBattleMessage("MP가 부족 합니다.");
                return;
            }

            TurnManager.instance.PlayerTurn = false;

            PlayerMove.instance.AttackAnimation();

            int Danage = Mathf.RoundToInt(PlayerManager.instance.Attack * 1.4f);

            Enemy[] enemies = { TurnManager.instance.Enemy1, TurnManager.instance.Enemy2, TurnManager.instance.Enemy3 };

            foreach (Enemy targetEnemy in enemies)
            {
                if (targetEnemy != null && targetEnemy.isDie == false)
                {
                    targetEnemy.TakeDamage(Danage);
                }
            }

            TurnManager.instance.ShowBattleMessage("가르기");

            Debug.Log("가르기 피해량 : " + Danage);


            TurnManager.instance.EndTurn();
            return;
        }
        if (Number == 3)
        {
            if (PlayerManager.instance.UseMp(25) == false)
            {
                TurnManager.instance.ShowBattleMessage("MP가 부족합니다.");
                return;
            }

            TurnManager.instance.PlayerTurn = false;

            PlayerManager.instance.StartCriticalBuff();

            TurnManager.instance.ShowBattleMessage("노려보기! 크리티컬 확률 증가");

            TurnManager.instance.EndTurn(false);
            return;

        }
        if (Number == 4)
        {
            if (PlayerManager.instance.UseMp(45) == false)
            {
                TurnManager.instance.ShowBattleMessage("MP가 부족합니다.");
                return;
            }

            TurnManager.instance.PlayerTurn = false;

            int HealAmount = Mathf.RoundToInt(PlayerManager.instance.MaxHp * 0.3f);

            PlayerManager.instance.Heal(HealAmount);

            TurnManager.instance.ShowBattleMessage("명상! 체력 " + HealAmount + " 회복");

            TurnManager.instance.EndTurn();
            return;
        }
        if (Number == 5)
        {
            Debug.Log("필살기");
        }
        if (Number == 6)
        {
            Debug.Log("가드");
        }
        if (Number == 7)
        {
            Debug.Log("기사환생");
        }
        if (Number == 8)
        {
            Debug.Log("약점 격파");
        }
        if (Number == 9)
        {
            Debug.Log("화염구");
        }
        if (Number == 10)
        {
            Debug.Log("급습");
        }
        if (Number == 11)
        {
            Debug.Log("최후의 일격");
        }
        if (Number == 12)
        {
            Debug.Log("공방일체");
        }
        if (Number == 13)
        {
            Debug.Log("두 개의 심장");
        }
    }

    public void Item(int Number)
    {
        if (Number == 1)
        {
            Debug.Log("빨간 포션");
        }
        if (Number == 2)
        {
            Debug.Log("파란 포션");
        }
        if (Number == 3)
        {
            Debug.Log("힘의 포션");
        }
        if (Number == 4)
        {
            Debug.Log("지식의 영약");
        }
        if (Number == 5)
        {
            Debug.Log("회피의 물약");
        }
    }

    public void TakeDamage(int Damage)
    {
        if (isDie == true) return;

        Damage = Mathf.RoundToInt(Damage * (1f - Defanse / 100f));

        if (Damage < 1)
        {
            Damage = 1;
        }

        Hp -= Damage;

        if (Hp < 0)
        {
            Hp = 0;
        }

        if (HpSlider != null)
        {
            HpSlider.value = Hp;
        }

        Debug.Log(gameObject.name + "데미지 : " + Damage + "/ 남은 HP : " + Hp);

        if (Hp <= 0)
        {
            Die();
        }
    }

    public void EnemyAttack()
    {
        if (isDie == true) return;

        if (Animator != null)
        {
            Animator.Play("EnemyAttack");
        }

        bool IsDodge = Random.value < Mathf.Clamp01(PlayerManager.instance.Dodge);

        if (IsDodge == true)
        {
            TurnManager.instance.ShowBattleMessage("회피!");
            Debug.Log("플레이어가 " + gameObject.name + "의 공격을 회피했습니다!");
            return;
        }

        if (PlayerDamege != null)
        {
            PlayerDamege.Play();
        }

        if (PlayerDamegeAudio != null)
        {
            PlayerDamegeAudio.volume = PlayerPrefs.GetFloat("SFX", 0.8f);
            PlayerDamegeAudio.Play();
        }

        PlayerManager.instance.TakeDamage(Attack);

        Debug.Log(gameObject.name + " 플레이어 공격 : " + Attack);
    }

    public void Die()
    {
        if (isDie == true) return;

        isDie = true;

        PlayerManager.instance.AddExp(Exp);
        PlayerManager.instance.AddGold(Gold);

        Debug.Log(gameObject.name + " 사망 / 경험치 + " + Exp + " / 골드 + " + Gold);

        gameObject.SetActive(false);
    }

    public void ResetEnemy()
    {
        isDie = false;
        Hp = MaxHp;

        gameObject.SetActive(true);

        SetupEnemyUI();

        if (HpSlider != null)
        {
            HpSlider.maxValue = MaxHp;
            HpSlider.value = Hp;
        }

        Debug.Log(gameObject.name + "초기화");
    }

    private void SetupEnemyUI()
    {
        Canvas[] EnemyCanvases = GetComponentsInChildren<Canvas>(true);

        foreach (Canvas EnemyCanvas in EnemyCanvases)
        {
            EnemyCanvas.gameObject.SetActive(true);
            EnemyCanvas.enabled = true;
            EnemyCanvas.worldCamera = Camera.main;
            EnemyCanvas.overrideSorting = true;
            EnemyCanvas.sortingOrder = 100;

            GraphicRaycaster Raycaster = EnemyCanvas.GetComponent<GraphicRaycaster>();
            if (Raycaster != null)
            {
                Raycaster.enabled = true;
                Raycaster.ignoreReversedGraphics = false;
            }
        }
    }
}

