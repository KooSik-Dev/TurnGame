using UnityEngine;
using UnityEngine.Rendering;

public class ClickSound : MonoBehaviour
{
    public static ClickSound instance;

    public AudioSource audioSource;

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

    private void Start()
    {
        audioSource.volume = PlayerPrefs.GetFloat("SFX", 0.8f);
    }
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            audioSource.Play();
        }
    }

    public void changeVolume(float Volume)
    {
        audioSource.volume = Volume;
    }
}
