using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public static PlayerMove instance;

    public AudioSource RunAudioSource;
    public Animator animator;

    public Transform characterImage;

    private float speed = 6f;

    private void Awake()
    {
        instance = this;
    }

    private void Update()
    {
        Animation();

        if (animator.GetBool("Run"))
        {
            Move();
        }

        Flip();
    }

    private void Move()
    {
        float move = Input.GetAxisRaw("Horizontal");

        transform.Translate( 0, 0, move * speed * Time.deltaTime );
    }

    private void RunSound(bool isRun)
    {
        if (isRun && RunAudioSource.isPlaying == false)
        {
            RunAudioSource.volume = PlayerPrefs.GetFloat("SFX", 0.8f);

            RunAudioSource.Play();
        }

        if (isRun == false && RunAudioSource.isPlaying == true)
        {
            RunAudioSource.Stop();
        }
    }

    private void Animation()
    {
        bool isRun = Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D);

        animator.SetBool("Run", isRun);

        RunSound(isRun);
    }

    private void Flip()
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

    public void AttackAnimation()
    {
        if (animator == null)
        {
            return;
        }

        animator.Play("BK_attack_1");
    }
}