using UnityEngine;

public class GameBgm : MonoBehaviour
{
    public AudioSource GamebgmAudio;

    private void Start()
    {
        GamebgmAudio.volume = 0;
    }

    private void Update()
    {
        isGameScene();
    }
    public void isGameScene()
    {
        if (BgmManager.instance == null) return;
        if (BgmManager.instance.isNextScene == false)
        {
            GamebgmAudio.volume = Mathf.MoveTowards(GamebgmAudio.volume, 0f, Time.deltaTime * 0.15f);
        }
        else
        {
            GamebgmAudio.volume = Mathf.MoveTowards(GamebgmAudio.volume, PlayerPrefs.GetFloat("BGM", 0.8f), Time.deltaTime * 0.15f);
        }
    }
}
