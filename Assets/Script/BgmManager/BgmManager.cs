using UnityEngine;

public class BgmManager : MonoBehaviour
{
    public static BgmManager instance;

    public AudioSource bgmAudio;

    public bool isNextScene = false;
    private bool isStop = false;

    public float TempFloat;

    private void Update()
    {
        isNestScene();
    }
    public void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
    }
    void Start()
    {
        bgmAudio.volume = PlayerPrefs.GetFloat("BGM", 0.8f);

        if (!bgmAudio.isPlaying)
        {
            bgmAudio.Play();
        }
    }

    public void changeVolume(float volume)
    {
        bgmAudio.volume = volume;
    }

    public void isNestScene()
    {
        if (bgmAudio.volume == TempFloat && isNextScene == isStop) return;

        if (isNextScene == true)
        {
            bgmAudio.volume = Mathf.MoveTowards(bgmAudio.volume, 0f, Time.deltaTime * 0.2f);
            TempFloat = 0f;
            isStop = true;
        }
        else
        {
            bgmAudio.volume = Mathf.MoveTowards(bgmAudio.volume, PlayerPrefs.GetFloat("BGM", 0.8f), Time.deltaTime * 0.2f);
            TempFloat = PlayerPrefs.GetFloat("BGM", 0.8f);
            isStop = false;
        }
    }
}
