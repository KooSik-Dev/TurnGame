using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainButton : MonoBehaviour
{
    public GameObject guide;
    public GameObject ranking;
    public GameObject setting;
    

    private void Update()
    {
        ESC();
    }

    public void ESC()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            guide.SetActive(false);
            ranking.SetActive(false);
            setting.SetActive(false);
            Debug.Log("창 닫기 완료");
        }
    }

    public void Exit()
    {
        Application.Quit();
        Debug.Log("게임 종료");
    }

    public void NextGame(string SceneName)
    {
        SceneManager.LoadScene(SceneName);
        BgmManager.instance.isNextScene = true;
    }
    
}
