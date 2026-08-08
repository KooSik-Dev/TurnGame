using UnityEngine;

public class StageCanvas : MonoBehaviour
{
    public GameObject Stage;
    public AudioSource AudioSource;
    private float timer = 0;
    public bool isStop = false;

    void Start()
    {
        AudioSource.volume = PlayerPrefs.GetFloat("SFX", 0.8f);
    }

    void Update()
    {
        if (BgmManager.instance == null) return;
        timer += Time.deltaTime;
        if (timer > 3f)
        { 
            if (isStop == true) return;
            AudioSource.Play();
            Stage.SetActive(false);
            isStop = true;
        }
    }
}
