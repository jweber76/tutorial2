using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerScript : MonoBehaviour
{

    private Rigidbody2D rd2d;
    public float speed;
    public Text score;
    private int scoreValue = 0;
    public Text winText;
    public Text livesText;
    private int livesValue = 3;
    public AudioClip backgroundMusic;
    public AudioClip winSound;
    public AudioSource musicSource;
    Animator anim;
    private bool facingRight = true;
    private bool isJumping = true;

    // Start is called before the first frame update
    void Start()
    {
        score.text = scoreValue.ToString();
        winText.text = "";
        livesText.text = "Lives: " + livesValue.ToString();
        rd2d = GetComponent<Rigidbody2D>();
        musicSource.clip = backgroundMusic;
        musicSource.Play();
        musicSource.loop = true;
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float hozMovement = Input.GetAxis("Horizontal");
        float vertMovement = Input.GetAxis("Vertical");
        rd2d.AddForce(new Vector2(hozMovement * speed, vertMovement * speed));

        if (Input.GetKey("escape"))
        {
            Application.Quit();
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            anim.SetInteger("state", 1);
        }
        if (Input.GetKeyUp(KeyCode.A))
        {
            anim.SetInteger("state", 0);
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            anim.SetInteger("state", 1);
        }
        if (Input.GetKeyUp(KeyCode.D))
        {
            anim.SetInteger("state", 0);
        }
        if (scoreValue == 8)
        {
            winText.text = "You win! Game made by Josh Weber!";
        }
        if (livesValue <= 0)
        {
            winText.text = "Game Over!";
            Destroy(gameObject);
        }
        if (facingRight == false && hozMovement > 0)
        {
            Flip();
        }
        else if (facingRight == true && hozMovement < 0)
        {
            Flip();
        }
        if (vertMovement > 0)
        {
            isJumping = true;
        }
        if (isJumping == false && vertMovement == 0 && hozMovement == 0)
        {
            anim.SetInteger("state", 0);
        }
        if (isJumping == true && vertMovement > 0)
        {
            anim.SetInteger("state", 2);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == "Coin")
        {
            scoreValue += 1;
            score.text = scoreValue.ToString();
            Destroy(collision.collider.gameObject);
            
            if (scoreValue == 4)
            {
                transform.position = new Vector2(65, 1);
                livesValue = 3;
                livesText.text = "Lives: " + livesValue.ToString();
            }
            if (scoreValue == 8)
            {
                musicSource.clip = winSound;
                musicSource.Play();
                musicSource.loop = false;
            }
        }
        if (collision.collider.tag == "Enemy")
        {
            livesValue -= 1;
            livesText.text = "Lives: " + livesValue.ToString();
            Destroy(collision.collider.gameObject);
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.collider.tag == "Ground")
        {
            if (Input.GetKeyDown(KeyCode.W))
            {
                rd2d.AddForce(new Vector2(0, 5), ForceMode2D.Impulse);
            }
            isJumping = false;
        }
    }
    void Flip()
   {
     facingRight = !facingRight;
     Vector2 Scaler = transform.localScale;
     Scaler.x = Scaler.x * -1;
     transform.localScale = Scaler;
   }
}
