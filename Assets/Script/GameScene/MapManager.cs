using UnityEngine;
using UnityEngine.UI;

public class MapManager : MonoBehaviour
{
    public static MapManager instance;

    public GameObject MapUI;
    public Button VictoryMapButton;
    public Button[] StageButtons;
    public GameObject[] StageObjects;
    public int HighestUnlockedIndex = 0;

    private readonly string[] StageNames =
    {
        "1-1", "1-2", "1-Shop", "1-Boss",
        "2-1", "2-2", "2-Shop", "2-Boss",
        "3-1", "3-2", "3-Shop", "3-Boss"
    };

    private void Awake()
    {
        instance = this;

        for (int i = 0; i < StageButtons.Length; i++)
        {
            int StageIndex = i;
            StageButtons[i].onClick.AddListener(() => OpenStage(StageIndex));
        }

        if (VictoryMapButton != null)
        {
            VictoryMapButton.onClick.AddListener(ShowMap);
        }

        RefreshButtons();
    }

    public void CompleteBattle(string ClearedStageName)
    {
        int ClearedIndex = FindStageIndex(ClearedStageName);

        if (ClearedIndex >= 0 && ClearedIndex + 1 < StageObjects.Length)
        {
            HighestUnlockedIndex = Mathf.Max(HighestUnlockedIndex, ClearedIndex + 1);
        }

        // 여기서는 다음 칸만 해금한다.
        // 지도는 승리 화면의 버튼을 눌렀을 때 연다.
        RefreshButtons();
    }

    public void ShowMap()
    {
        if (TurnManager.instance != null && TurnManager.instance.WinUI != null)
        {
            TurnManager.instance.WinUI.SetActive(false);
        }

        CloseAllStages();
        RefreshButtons();
        MapUI.SetActive(true);
    }

    public void OpenStage(int StageIndex)
    {
        if (StageIndex < 0 || StageIndex >= StageObjects.Length) return;
        if (StageIndex > HighestUnlockedIndex) return;

        CloseAllStages();
        MapUI.SetActive(false);
        GameObject ObjectToOpen = StageObjects[StageIndex];

        ObjectToOpen.SetActive(true);

        // 상점에 들어오면 그 다음 보스 스테이지를 해금한다.
        if (IsShop(StageIndex) && StageIndex + 1 < StageObjects.Length)
        {
            HighestUnlockedIndex = Mathf.Max(HighestUnlockedIndex, StageIndex + 1);
            SetupShopUI(ObjectToOpen);
        }

        TurnManager Battle = ObjectToOpen.GetComponent<TurnManager>();
        if (Battle != null) Battle.BeginBattle();
    }

    private void RefreshButtons()
    {
        for (int i = 0; i < StageButtons.Length; i++)
        {
            StageButtons[i].interactable = i <= HighestUnlockedIndex;
        }
    }

    private void CloseAllStages()
    {
        foreach (GameObject StageObject in StageObjects)
        {
            if (StageObject != null) StageObject.SetActive(false);
        }
    }

    private int FindStageIndex(string StageName)
    {
        for (int i = 0; i < StageNames.Length; i++)
        {
            if (StageNames[i] == StageName) return i;
        }

        return -1;
    }

    private bool IsShop(int StageIndex)
    {
        return StageIndex == 2 || StageIndex == 6 || StageIndex == 10;
    }

    private void SetupShopUI(GameObject ShopObject)
    {
        Button[] Buttons = ShopObject.GetComponentsInChildren<Button>(true);

        foreach (Button TargetButton in Buttons)
        {
            if (TargetButton.gameObject.name == "Button (Legacy)")
            {
                TargetButton.onClick.RemoveListener(ShowMap);
                TargetButton.onClick.AddListener(ShowMap);
            }
        }

        RefreshShopGold();
    }

    public void RefreshShopGold()
    {
        if (PlayerManager.instance == null)
        {
            return;
        }

        int[] ShopIndexes = { 2, 6, 10 };

        foreach (int ShopIndex in ShopIndexes)
        {
            if (ShopIndex >= StageObjects.Length || StageObjects[ShopIndex] == null)
            {
                continue;
            }

            Transform[] ShopChildren = StageObjects[ShopIndex].GetComponentsInChildren<Transform>(true);

            foreach (Transform ShopChild in ShopChildren)
            {
                if (ShopChild.name != "Gold")
                {
                    continue;
                }

                Text[] GoldTexts = ShopChild.GetComponentsInChildren<Text>(true);

                if (GoldTexts.Length >= 2)
                {
                    // Gold 오브젝트 안의 첫 글자는 제목, 마지막 글자는 보유 골드 숫자이다.
                    GoldTexts[0].text = "Gold";
                    GoldTexts[GoldTexts.Length - 1].text = PlayerManager.instance.Gold.ToString();
                }
                else if (GoldTexts.Length == 1)
                {
                    // 글자가 하나뿐인 상점 UI도 사용할 수 있게 한다.
                    GoldTexts[0].text = "Gold : " + PlayerManager.instance.Gold;
                }
            }
        }
    }
}
