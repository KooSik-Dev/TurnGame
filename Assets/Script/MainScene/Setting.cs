using UnityEngine;
using UnityEngine.UI;

public class Setting : MonoBehaviour
{
    public Slider bgmSlider;
    public Slider sfxSlider;
    public Toggle FullScreenToggle;

    private void Start()
    {
        if (bgmSlider != null) bgmSlider.value = PlayerPrefs.GetFloat("BGM", 0.8f);      
        if (sfxSlider != null) sfxSlider.value = PlayerPrefs.GetFloat("SFX", 0.8f);

        if (FullScreenToggle != null)
        {
            Screen.fullScreen = (PlayerPrefs.GetInt("Full", 1) == 1);
            FullScreenToggle.isOn = (PlayerPrefs.GetInt("Full", 1) == 1);
        }
        
    }

    public void BGM(float value)
    {
        if (bgmSlider == null) return;
        PlayerPrefs.SetFloat("BGM", value);
        PlayerPrefs.Save();

        BgmManager.instance.changeVolume(value);

        if (PlayerPrefs.HasKey("BGM"))
        {
            Debug.Log("BGM 상태 " + PlayerPrefs.GetFloat("BGM"));
        }
    }

    public void SFX(float value)
    {
        if (sfxSlider == null) return;
        PlayerPrefs.SetFloat("SFX", value);
        PlayerPrefs.Save();

        ClickSound.instance.changeVolume(value);

        if (PlayerPrefs.HasKey("SFX"))
        {
            Debug.Log("SFX 상태 " + PlayerPrefs.GetFloat("SFX"));
        }
    }

    public void FullScreen(bool isFullScreen)
    {
        if (FullScreenToggle == null) return;
        Screen.fullScreen = isFullScreen;
        PlayerPrefs.SetInt("Full", isFullScreen ? 1 : 0);
        PlayerPrefs.Save();

        if (PlayerPrefs.HasKey("Full"))
        {
            Debug.Log("화면상태 " + PlayerPrefs.GetInt("Full"));
        }
    }
}
