using UnityEngine;
using UnityEngine.UI;

public class SaveButton : MonoBehaviour
{
    public int Type;
    public int TypeNumber;

    private Text ShopStatusText;

    private void Start()
    {
        if (IsShopCard())
        {
            FindShopStatusText();
            RefreshShopCard();
        }
    }

    public void SaveButtons()
    {
        if (IsShopCard())
        {
            BuyProduct();
            return;
        }

        PlayerPrefs.SetInt("Type", Type);
        PlayerPrefs.SetInt("TypeNumber", TypeNumber);
        Debug.Log("Type : " + Type);
        Debug.Log("TypeNumber : " + TypeNumber);
    }

    private void BuyProduct()
    {
        if (PlayerManager.instance == null)
        {
            return;
        }

        // 이미 구매한 상점 스킬은 다시 구매할 수 없다.
        if (Type == 2 && PlayerManager.instance.HasShopSkill(TypeNumber))
        {
            gameObject.SetActive(false);
            return;
        }

        if (Type == 3 && PlayerManager.instance.CanAddShopItem(TypeNumber) == false)
        {
            SetStatusText("최대 보유 개수입니다.");
            return;
        }

        int Price = GetPrice();

        if (PlayerManager.instance.TrySpendGold(Price) == false)
        {
            SetStatusText("골드 부족 / 가격 " + Price + "G");
            Debug.Log("골드가 부족합니다. 필요 골드 : " + Price);
            return;
        }

        if (MapManager.instance != null)
        {
            MapManager.instance.RefreshShopGold();
        }

        if (Type == 2)
        {
            PlayerManager.instance.AddShopSkill(TypeNumber);
            Debug.Log("스킬 구매 완료 : " + gameObject.name);
            gameObject.SetActive(false);
        }
        else if (Type == 3)
        {
            PlayerManager.instance.AddShopItem(TypeNumber);
            Debug.Log("아이템 구매 완료 : " + gameObject.name);
            RefreshShopCard();
        }
    }

    private void RefreshShopCard()
    {
        if (PlayerManager.instance == null)
        {
            return;
        }

        if (Type == 2)
        {
            bool AlreadyOwned = PlayerManager.instance.HasShopSkill(TypeNumber);
            gameObject.SetActive(AlreadyOwned == false);

            if (AlreadyOwned == false)
            {
                SetStatusText("가격 " + GetPrice() + "G");
            }
        }
        else if (Type == 3)
        {
            int Count = PlayerManager.instance.GetItemCount(TypeNumber);
            SetStatusText("가격 " + GetPrice() + "G / " + Count + "개 보유");
        }
    }

    private int GetPrice()
    {
        if (Type == 2)
        {
            if (TypeNumber == 7) return 30;
            if (TypeNumber == 8) return 40;
            if (TypeNumber == 9) return 50;
            if (TypeNumber == 10) return 60;
            if (TypeNumber == 11) return 75;
            if (TypeNumber == 12) return 90;
            if (TypeNumber == 13) return 110;
        }

        if (Type == 3)
        {
            if (TypeNumber == 1) return 10;
            if (TypeNumber == 2) return 10;
            if (TypeNumber == 3) return 15;
            if (TypeNumber == 4) return 15;
            if (TypeNumber == 5) return 20;
        }

        return 0;
    }

    private bool IsShopCard()
    {
        Transform Current = transform;

        while (Current != null)
        {
            if (Current.name.EndsWith("-Shop"))
            {
                return true;
            }

            Current = Current.parent;
        }

        return false;
    }

    private void FindShopStatusText()
    {
        Text[] Texts = GetComponentsInChildren<Text>(true);

        foreach (Text TargetText in Texts)
        {
            if (TargetText.gameObject.name == "Text (Legacy)")
            {
                ShopStatusText = TargetText;
                return;
            }
        }
    }

    private void SetStatusText(string Message)
    {
        if (ShopStatusText == null)
        {
            FindShopStatusText();
        }

        if (ShopStatusText != null)
        {
            ShopStatusText.text = Message;
        }
    }
}
