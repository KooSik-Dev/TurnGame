using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public AudioSource RunAudioSource;
    public Animator animator;

    public Transform characterImage;
    private float speed = 6f;

    void Update()
    {
        Animation();
        if (animator.GetBool("Run"))
            Move();
        
        Flip();
    }

    void Move()
    {
        float move = Input.GetAxisRaw("Horizontal");

        transform.Translate(0, 0, move * speed * Time.deltaTime);
    }

    void RunSound(bool isRun)
    {
        if (isRun && !RunAudioSource.isPlaying)
        {
            RunAudioSource.volume = PlayerPrefs.GetFloat("SFX", 0.8f);
            RunAudioSource.Play();
        }
    }

    void Animation()
    {
        bool isRun = Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D);

        animator.SetBool("Run", isRun);

        RunSound(isRun);
    }

    void Flip()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            characterImage.localScale = new Vector3(-6, 6, 1);
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            characterImage.localScale = new Vector3(6, 6, 1);
        }
    }
}