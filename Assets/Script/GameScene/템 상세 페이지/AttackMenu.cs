using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AttackMenu : MonoBehaviour
{
    public GameObject a1;

    public GameObject s1;
    public GameObject s2;
    public GameObject s3;
    public GameObject s4;
    public GameObject s5;
    public GameObject s6;
    public GameObject s7;
    public GameObject s8;
    public GameObject s9;
    public GameObject s10;
    public GameObject s11;
    public GameObject s12;
    public GameObject s13;

    public GameObject i1;
    public GameObject i2;
    public GameObject i3;
    public GameObject i4;
    public GameObject i5;

    public Text asiText;

    public void attack()
    {
        PlayerPrefs.SetInt("Type", 1);
        PlayerPrefs.SetInt("TypeNumber", 1);

        a1.SetActive(true);

        s1.SetActive(false);
        s2.SetActive(false);
        s3.SetActive(false);
        s4.SetActive(false);
        s5.SetActive(false);
        s6.SetActive(false);
        s7.SetActive(false);
        s8.SetActive(false);
        s9.SetActive(false);
        s10.SetActive(false);
        s11.SetActive(false);
        s12.SetActive(false);
        s13.SetActive(false);

        i1.SetActive(false);
        i2.SetActive(false);
        i3.SetActive(false);
        i4.SetActive(false);
        i5.SetActive(false);

        asiText.text = "공격";
    }

    public void skill()
    {
        PlayerPrefs.SetInt("Type", 0);
        PlayerPrefs.SetInt("TypeNumber", 0);

        a1.SetActive(false);

        // 각 카드 안에 적힌 실제 획득 레벨에 맞춰 보여준다.
        if (PlayerManager.instance != null)
        {
            s1.SetActive(PlayerManager.instance.Level >= 1);
            s2.SetActive(PlayerManager.instance.Level >= 1);
            s3.SetActive(PlayerManager.instance.Level >= 2);
            s4.SetActive(PlayerManager.instance.Level >= 2);
            s5.SetActive(PlayerManager.instance.Level >= 4);
            s6.SetActive(PlayerManager.instance.Level >= 6);

            // 7~13번은 상점 스킬이므로 구매한 것만 보여준다.
            s7.SetActive(PlayerManager.instance.HasShopSkill(7));
            s8.SetActive(PlayerManager.instance.HasShopSkill(8));
            s9.SetActive(PlayerManager.instance.HasShopSkill(9));
            s10.SetActive(PlayerManager.instance.HasShopSkill(10));
            s11.SetActive(PlayerManager.instance.HasShopSkill(11));
            s12.SetActive(PlayerManager.instance.HasShopSkill(12));
            s13.SetActive(PlayerManager.instance.HasShopSkill(13));
        }
        else
        {
            s1.SetActive(false);
            s2.SetActive(false);
            s3.SetActive(false);
            s4.SetActive(false);
            s5.SetActive(false);
            s6.SetActive(false);

            s7.SetActive(false);
            s8.SetActive(false);
            s9.SetActive(false);
            s10.SetActive(false);
            s11.SetActive(false);
            s12.SetActive(false);
            s13.SetActive(false);
        }

        i1.SetActive(false);
        i2.SetActive(false);
        i3.SetActive(false);
        i4.SetActive(false);
        i5.SetActive(false);

        asiText.text = "스킬";
    }

    public void item()
    {
        PlayerPrefs.SetInt("Type", 0);
        PlayerPrefs.SetInt("TypeNumber", 0);

        a1.SetActive(false);

        s1.SetActive(false);
        s2.SetActive(false);
        s3.SetActive(false);
        s4.SetActive(false);
        s5.SetActive(false);
        s6.SetActive(false);
        s7.SetActive(false);
        s8.SetActive(false);
        s9.SetActive(false);
        s10.SetActive(false);
        s11.SetActive(false);
        s12.SetActive(false);
        s13.SetActive(false);

        // 상점에서 구매해서 1개 이상 가지고 있는 아이템만 보여준다.
        if (PlayerManager.instance != null)
        {
            i1.SetActive(PlayerManager.instance.GetItemCount(1) > 0);
            i2.SetActive(PlayerManager.instance.GetItemCount(2) > 0);
            i3.SetActive(PlayerManager.instance.GetItemCount(3) > 0);
            i4.SetActive(PlayerManager.instance.GetItemCount(4) > 0);
            i5.SetActive(PlayerManager.instance.GetItemCount(5) > 0);
        }
        else
        {
            i1.SetActive(false);
            i2.SetActive(false);
            i3.SetActive(false);
            i4.SetActive(false);
            i5.SetActive(false);
        }

        asiText.text = "아이템";
    }
    
}

