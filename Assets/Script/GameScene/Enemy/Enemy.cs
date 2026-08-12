using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    public int Level = 1;

    public int MaxHp = 60;
    public int Hp = 60;

    public int Attack = 10;
    public int Defanse = 0;
    public int DefenseDownTurns = 0;
    public bool IsGuarding = false;
    public bool NextActionIsGuard = false;
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

        int Damage = PlayerManager.instance.GetCurrentAttack();
        bool IsCritical = UnityEngine.Random.value < Mathf.Clamp01(PlayerManager.instance.GetCriticalChance());

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
        int RemainingCooldown = PlayerManager.instance.GetSkillCooldown(Number);

        if (RemainingCooldown > 0)
        {
            TurnManager.instance.ShowBattleMessage("쿨타임 " + RemainingCooldown + "턴 남음");
            return;
        }

        if (Number == 1)
        {
            if (PlayerManager.instance.UseMp(30) == false)
            {
                TurnManager.instance.ShowBattleMessage("MP가 부족 합니다.");
                return;
            }
            TurnManager.instance.PlayerTurn = false;

            PlayerMove.instance.AttackAnimation();

            int Damage = PlayerManager.instance.CalculateSkillDamage(1.7f);

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

            int Danage = PlayerManager.instance.CalculateSkillDamage(1.4f);

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
            if (PlayerManager.instance.UseMp(100) == false)
            {
                TurnManager.instance.ShowBattleMessage("MP가 부족합니다.");
                return;
            }

            TurnManager.instance.PlayerTurn = false;

            PlayerMove.instance.AttackAnimation();

            int Damage = PlayerManager.instance.CalculateSkillDamage(3f);

            Enemy[] enemies =
            {
                TurnManager.instance.Enemy1,
                TurnManager.instance.Enemy2,
                TurnManager.instance.Enemy3
            };

            foreach (Enemy TargetEnemy in enemies)
            {
                if (TargetEnemy != null && TargetEnemy.isDie == false)
                {
                    TargetEnemy.TakeDamage(Damage);
                }
            }

            TurnManager.instance.ShowBattleMessage("필살기! 모든 적에게 " + Damage + " 피해");

            Debug.Log("필살기 피해량 : " + Damage);

            TurnManager.instance.EndTurn();
            return;
        }
        if (Number == 6)
        {
            if (PlayerManager.instance.UseMp(30) == false)
            {
                TurnManager.instance.ShowBattleMessage("MP가 부족합니다.");
                return;
            }

            TurnManager.instance.PlayerTurn = false;
            PlayerManager.instance.GuardTurns = 2;

            TurnManager.instance.ShowBattleMessage("가드! 2턴 동안 받는 피해 30% 감소");
            Debug.Log("가드 사용");

            TurnManager.instance.EndTurn();
            return;
        }
        if (Number == 7)
        {
            if (PlayerManager.instance.UseMp(45) == false)
            {
                TurnManager.instance.ShowBattleMessage("MP가 부족합니다.");
                return;
            }

            TurnManager.instance.PlayerTurn = false;

            PlayerMove.instance.AttackAnimation();

            float LostHpRate = 1f - ((float)PlayerManager.instance.Hp / PlayerManager.instance.MaxHp);
   
            float DamageRate = Mathf.Lerp(1.5f, 2.5f, LostHpRate);

            int Damage = PlayerManager.instance.CalculateSkillDamage(DamageRate);

            TakeDamage(Damage);

            TurnManager.instance.ShowBattleMessage("기사회생! " + Damage + " 피해");

            Debug.Log("기사회생 공격 배율 : " + DamageRate + " / 피해량 : " + Damage);

            TurnManager.instance.EndTurn();
            return;
        }
        if (Number == 8)
        {
            if (PlayerManager.instance.UseMp(55) == false)
            {
                TurnManager.instance.ShowBattleMessage("MP가 부족합니다.");
                return;
            }

            TurnManager.instance.PlayerTurn = false;
            PlayerMove.instance.AttackAnimation();

            int Damage = PlayerManager.instance.CalculateSkillDamage(1.3f);

            // 약점 격파 자체의 공격은 기존 방어력으로 피해를 계산한다.
            TakeDamage(Damage);

            // 살아 있다면 이후 공격 3회 동안 방어력이 30 감소한다.
            if (isDie == false)
            {
                DefenseDownTurns = 3;
            }

            TurnManager.instance.ShowBattleMessage("약점 격파! 방어력 30 감소");
            Debug.Log("약점 격파 피해량 : " + Damage);

            TurnManager.instance.EndTurn();
            return;
        }
        if (Number == 9)
        {
            if (PlayerManager.instance.UseMp(65) == false)
            {
                TurnManager.instance.ShowBattleMessage("MP가 부족합니다.");
                return;
            }

            TurnManager.instance.PlayerTurn = false;
            PlayerMove.instance.AttackAnimation();

            int Damage = PlayerManager.instance.CalculateSkillDamage(1.8f);

            Enemy[] Enemies =
            {
                TurnManager.instance.Enemy1,
                TurnManager.instance.Enemy2,
                TurnManager.instance.Enemy3
            };

            foreach (Enemy TargetEnemy in Enemies)
            {
                if (TargetEnemy != null && TargetEnemy.isDie == false)
                {
                    TargetEnemy.TakeDamage(Damage);
                }
            }

            TurnManager.instance.ShowBattleMessage("화염구! 모든 적에게 " + Damage + " 피해");
            Debug.Log("화염구 피해량 : " + Damage);

            TurnManager.instance.EndTurn();
            return;
        }
        if (Number == 10)
        {
            if (PlayerManager.instance.UseMp(60) == false)
            {
                TurnManager.instance.ShowBattleMessage("MP가 부족합니다.");
                return;
            }

            TurnManager.instance.PlayerTurn = false;
            PlayerMove.instance.AttackAnimation();

            float DamageRate = 1.5f;
            bool BrokeGuard = IsGuarding;

            if (BrokeGuard)
            {
                IsGuarding = false;
                DamageRate = 2.5f;
            }

            int Damage = PlayerManager.instance.CalculateSkillDamage(DamageRate);
            TakeDamage(Damage);

            if (BrokeGuard)
            {
                TurnManager.instance.ShowBattleMessage("급습! 방어 해제, " + Damage + " 피해");
            }
            else
            {
                TurnManager.instance.ShowBattleMessage("급습! " + Damage + " 피해");
            }

            Debug.Log("급습 피해량 : " + Damage + " / 방어 해제 : " + BrokeGuard);

            TurnManager.instance.EndTurn();
            return;
        }
        if (Number == 11)
        {
            if (PlayerManager.instance.UseMp(70) == false)
            {
                TurnManager.instance.ShowBattleMessage("MP가 부족합니다.");
                return;
            }

            TurnManager.instance.PlayerTurn = false;
            PlayerMove.instance.AttackAnimation();

            float HpRate = MaxHp > 0 ? (float)Hp / MaxHp : 0f;
            float DamageRate = HpRate <= 0.3f ? 3f : 1.7f;
            int Damage = PlayerManager.instance.CalculateSkillDamage(DamageRate);

            TakeDamage(Damage);

            if (DamageRate == 3f)
            {
                TurnManager.instance.ShowBattleMessage("최후의 일격! 빈사 상태 추가 피해 " + Damage);
            }
            else
            {
                TurnManager.instance.ShowBattleMessage("최후의 일격! " + Damage + " 피해");
            }

            Debug.Log("최후의 일격 피해량 : " + Damage);
            TurnManager.instance.EndTurn();
            return;
        }
        if (Number == 12)
        {
            if (PlayerManager.instance.UseMp(100) == false)
            {
                TurnManager.instance.ShowBattleMessage("MP가 부족합니다.");
                return;
            }

            TurnManager.instance.PlayerTurn = false;
            PlayerMove.instance.AttackAnimation();

            int Damage = PlayerManager.instance.CalculateSkillDamage(1f);
            Enemy[] Enemies =
            {
                TurnManager.instance.Enemy1,
                TurnManager.instance.Enemy2,
                TurnManager.instance.Enemy3
            };

            foreach (Enemy TargetEnemy in Enemies)
            {
                if (TargetEnemy != null && TargetEnemy.isDie == false)
                {
                    TargetEnemy.TakeDamage(Damage);
                }
            }

            PlayerManager.instance.DefenseBuffTurns = 3;

            TurnManager.instance.ShowBattleMessage("공방일체! 모든 적 공격, 방어력 증가");
            Debug.Log("공방일체 피해량 : " + Damage);

            TurnManager.instance.EndTurn();
            return;
        }
        if (Number == 13)
        {
            if (PlayerManager.instance.ExtraActions > 0)
            {
                TurnManager.instance.ShowBattleMessage("이미 두 개의 심장을 사용 중입니다.");
                return;
            }

            PlayerManager.instance.ExtraActions = 1;
            PlayerManager.instance.StartSkillCooldown(13);
            TurnManager.instance.PlayerTurn = true;

            TurnManager.instance.ShowBattleMessage("두 개의 심장! 이번 턴에 두 번 행동 가능");
            Debug.Log("두 개의 심장 사용");
            return;
        }
    }

    public void Item(int Number)
    {
        if (Number == 1)
        {
            if (PlayerManager.instance.Hp >= PlayerManager.instance.MaxHp)
            {
                TurnManager.instance.ShowBattleMessage("체력이 이미 가득 찼습니다.");
                return;
            }

            if (PlayerManager.instance.UseItemCount(1) == false)
            {
                TurnManager.instance.ShowBattleMessage("빨간 포션이 없습니다.");
                return;
            }

            int HealAmount = Mathf.RoundToInt(PlayerManager.instance.MaxHp * 0.2f);
            PlayerManager.instance.Heal(HealAmount);
            TurnManager.instance.ShowBattleMessage("빨간 포션! HP " + HealAmount + " 회복");
        }
        else if (Number == 2)
        {
            if (PlayerManager.instance.Mp >= PlayerManager.instance.MaxMp)
            {
                TurnManager.instance.ShowBattleMessage("MP가 이미 가득 찼습니다.");
                return;
            }

            if (PlayerManager.instance.UseItemCount(2) == false)
            {
                TurnManager.instance.ShowBattleMessage("파란 포션이 없습니다.");
                return;
            }

            int RestoreAmount = Mathf.RoundToInt(PlayerManager.instance.MaxMp * 0.2f);
            PlayerManager.instance.RestoreMp(RestoreAmount);
            TurnManager.instance.ShowBattleMessage("파란 포션! MP " + RestoreAmount + " 회복");
        }
        else if (Number == 3)
        {
            if (PlayerManager.instance.UseItemCount(3) == false)
            {
                TurnManager.instance.ShowBattleMessage("힘의 영약이 없습니다.");
                return;
            }

            PlayerManager.instance.PowerPotionTurns = 5;
            TurnManager.instance.ShowBattleMessage("힘의 영약! 5턴간 공격력 30% 증가");
        }
        else if (Number == 4)
        {
            if (PlayerManager.instance.UseItemCount(4) == false)
            {
                TurnManager.instance.ShowBattleMessage("지식의 영약이 없습니다.");
                return;
            }

            PlayerManager.instance.KnowledgePotionTurns = 5;
            TurnManager.instance.ShowBattleMessage("지식의 영약! 5턴간 스킬 피해 30% 증가");
        }
        else if (Number == 5)
        {
            if (PlayerManager.instance.UseItemCount(5) == false)
            {
                TurnManager.instance.ShowBattleMessage("회피의 물약이 없습니다.");
                return;
            }

            PlayerManager.instance.DodgePotionTurns = 5;
            TurnManager.instance.ShowBattleMessage("회피의 물약! 5턴간 회피율 2배");
        }

        TurnManager.instance.PlayerTurn = false;
        PlayerPrefs.SetInt("Type", 0);
        PlayerPrefs.SetInt("TypeNumber", 0);
        TurnManager.instance.EndTurn();
    }

    public void TakeDamage(int Damage)
    {
        if (isDie == true) return;

        int OriginalDamage = Damage;

        int CurrentDefense = Defanse;

        if (DefenseDownTurns > 0)
        {
            CurrentDefense = Mathf.Max(0, CurrentDefense - 30);
            DefenseDownTurns--;

            Debug.Log(gameObject.name + " 약점 격파 남은 횟수 : " + DefenseDownTurns);
        }

        Damage = Mathf.RoundToInt(Damage * (1f - CurrentDefense / 100f));

        if (IsGuarding)
        {
            float GuardReduction = Mathf.Clamp01((50f + Level * 3f) / 100f);
            Damage = Mathf.RoundToInt(Damage * (1f - GuardReduction));
            Debug.Log(gameObject.name + " 방어 중! 피해 " + Mathf.RoundToInt(GuardReduction * 100f) + "% 감소");
        }

        int MinimumDamage = Mathf.Max(1, Mathf.CeilToInt(OriginalDamage * 0.1f));

        if (Damage < MinimumDamage)
        {
            Damage = MinimumDamage;
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

        bool IsDodge = UnityEngine.Random.value < PlayerManager.instance.GetCurrentDodge();

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

    public void PlanNextAction()
    {
        if (isDie)
        {
            NextActionIsGuard = false;
            IsGuarding = false;
            return;
        }

        NextActionIsGuard = UnityEngine.Random.value < 0.3f;
        IsGuarding = NextActionIsGuard;
    }

    public void PerformPlannedAction()
    {
        if (isDie) return;

        if (NextActionIsGuard)
        {
            TurnManager.instance.ShowBattleMessage(gameObject.name + " 방어!");
            Debug.Log(gameObject.name + " 방어 사용");
            return;
        }

        EnemyAttack();
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
        DefenseDownTurns = 0;
        IsGuarding = false;
        NextActionIsGuard = false;

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

