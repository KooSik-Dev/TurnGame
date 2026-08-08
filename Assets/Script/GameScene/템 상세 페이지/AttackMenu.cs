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
        a1.SetActive(false);

        s1.SetActive(true);
        s2.SetActive(true);
        s3.SetActive(true);
        s4.SetActive(true);
        s5.SetActive(true);
        s6.SetActive(true);
        s7.SetActive(true);
        s8.SetActive(true);
        s9.SetActive(true);
        s10.SetActive(true);
        s11.SetActive(true);
        s12.SetActive(true);
        s13.SetActive(true);

        i1.SetActive(false);
        i2.SetActive(false);
        i3.SetActive(false);
        i4.SetActive(false);
        i5.SetActive(false);

        asiText.text = "스킬";
    }

    public void item()
    {
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

        i1.SetActive(true);
        i2.SetActive(true);
        i3.SetActive(true);
        i4.SetActive(true);
        i5.SetActive(true);

        asiText.text = "아이템";
    }
    
}
