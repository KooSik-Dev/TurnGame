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

    public bool isDie = false;

    public Slider HpSlider;

    void Start()
    {
        Hp = MaxHp;

        HpSlider.maxValue = MaxHp;
        HpSlider.value = Hp;
    }

    public void ClickButton()
    {
        if (isDie == true) return;

        if (TurnManager.instance.PlayerTurn == false) return;

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
        int Damage = PlayerManager.instance.Attack;

        TakeDamage(Damage);

        Debug.Log("기본 공격");

        TurnManager.instance.EndTurn();
    }

    public void Skill(int Number)
    {
        if (Number == 1)
        {
            Debug.Log("배기");
        }
        if (Number == 2)
        {
            Debug.Log("가르기");
        }
        if (Number == 3)
        {
            Debug.Log("노려보기");
        }
        if (Number == 4)
        {
            Debug.Log("명상");
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

        HpSlider.value = Hp;

        Debug.Log(gameObject.name + "데미지 : " + Damage + "/ 남은 HP : " + Hp);

        if (Hp <= 0)
        {
            Die();
        }
    }

    public void EnemyAttack()
    {
        if (isDie == true) return;

        PlayerManager.instance.TakeDamage(Attack);

        Debug.Log(gameObject.name + " 플레이어 공격 : " + Attack);
    }

    public void Die()
    {
        if (isDie == true) return;

        isDie = true;

        PlayerManager.instance.AddExp(Exp);

        Debug.Log(gameObject.name + " 사망 / 경험치 + " + Exp);

        gameObject.SetActive(false);
    }
}