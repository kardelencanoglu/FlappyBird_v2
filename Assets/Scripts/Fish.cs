using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fish : MonoBehaviour
{

    Rigidbody2D _rb;

    [SerializeField] //editorde _speed verisine ula�mak i�in
    private float _speed;
    int angle;

    int maxAngle = 20;
    int minAngle = -60;
 
    public Score score;
    bool touchedGround;
    public GameManager gameManager;
    public Sprite fishDied;
    SpriteRenderer sp;
    Animator anim;
    public ObstacleSpawner obstaclespawner;

    [SerializeField] private AudioSource swim,hit,point;


    // Start is called before the first frame update
    void Start()
    {

        _rb = GetComponent<Rigidbody2D>();
        _rb.gravityScale = 0;
        //        _rb.gravityScale = -1;
        sp = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();

    }

    // Update is called once per frame
    void Update()
    {

        FishSwim();
    }

    private void FixedUpdate()
    {
        FishRotation();
    }

    void FishSwim()
    {
        if (Input.GetMouseButtonDown(0) && GameManager.gameOver == false)
        {
            swim.Play();
            if(GameManager.gameStarted == false)
            {
                _rb.gravityScale = 4f;
                _rb.velocity = Vector2.zero;
                _rb.velocity = new Vector2(_rb.velocity.x, _speed);
                obstaclespawner.InstantiateObstacle();
                gameManager.GameHasStarted();
            }
            else
            {
                _rb.velocity = Vector2.zero;
                _rb.velocity = new Vector2(_rb.velocity.x, _speed);
            }
        }
    }

    void FishRotation()
    {
        if (_rb.velocity.y > 0)
        {
            if (angle <= maxAngle)
            {
                angle = angle + 4;
            }
        }
        else if (_rb.velocity.y < -1.2)
        {
            if (angle > minAngle)
            {
                angle = angle - 2;
            }
        }

        if (touchedGround == false)
        {
            transform.rotation = Quaternion.Euler(0, 0, angle); //a��sal d�n�� i�in
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Obstacle"))
        {
//           Debug.Log("Skor");
            score.Scored(); ;
            point.Play();
        }
        else if (collision.CompareTag("Column") && GameManager.gameOver == false)
        {
            //game over
            FishDieAudio();
            gameManager.GameOver();
        }
    }

    void FishDieAudio()
    {
        hit.Play();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            if (GameManager.gameOver == false)
            {
                //game over
                FishDieAudio();
                gameManager.GameOver();
                GameOver();
            }
            else
            {
                //game over
                GameOver();
            }
//            Debug.Log("die");
        }
    }

    void GameOver()
    {
        touchedGround = true;
        transform.rotation = Quaternion.Euler(0, 0, -90);
        sp.sprite = fishDied;
        anim.enabled = false;
    }
}
