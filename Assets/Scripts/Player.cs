using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Player : MovingObject
{

    public int wallDamage = 1;
    public int pointsPerFood = 10;
    public int pointsPerSoda = 20;
    public float restartLevelDelay = 0.2f;
    public Text foodText;

    public AudioClip moveSound1;
    public AudioClip moveSound2;
    public AudioClip eatSound1;
    public AudioClip eatSound2;
    public AudioClip drinkSound1;
    public AudioClip drinkSound2;
    public AudioClip gameOverSound;
    public AudioClip stoneSound;

    private Animator animator;
    private int food;
    private Vector2 tounchOrigin = -Vector2.one;


    // Start is called before the first frame update
    protected override void Start()
    {
        animator = GetComponent<Animator>();
        food = GameManager.instance.playerFoodPoints;
        foodText.text = "Food: " + food;
        base.Start();
    }

    private void OnDisable()
    {
        GameManager.instance.playerFoodPoints = food;
    }

    // Update is called once per frame
    void Update()
    {
        if (!GameManager.instance.playersTurn) return;

        int horizontal = 0;
        int vertical = 0;

        // #if UNITY_STANDALONE || UNITY_WEBPLAYER

        horizontal = (int)Input.GetAxisRaw("Horizontal");
        vertical = (int)Input.GetAxisRaw("Vertical");

        if (horizontal != 0)
            vertical = 0;

        // #else

        //         if (Input.touchCount > 0)
        //         {
        //             Touch myTouch = Input.touches[0];
        //             if (myTouch.phase == TouchPhase.Began)
        //             {
        //                 tounchOrigin = myTouch.position;
        //             }
        //             else if (myTouch.phase == TouchPhase.Ended && tounchOrigin.x >= 0)
        //             {
        //                 Vector2 touchEnd = myTouch.position;
        //                 float x = touchEnd.x - tounchOrigin.x;
        //                 float y = touchEnd.y - tounchOrigin.y;
        //                 if (Mathf.Abs(x) > Mathf.Abs(y))
        //                     horizontal = x > 0 ? 1 : -1;
        //                 else
        //                     vertical = y > 0 ? 1 : -1;
        //             }
        //         }

        // #endif

        if (horizontal != 0 || vertical != 0)
            AttemptMove<Wall>(horizontal, vertical);

    }

    protected override void AttemptMove<T>(int xDir, int yDir)
    {
        food--;
        foodText.text = "Food: " + food;
        base.AttemptMove<T>(xDir, yDir);
        RaycastHit2D hit;

        if (Move(xDir, yDir, out hit))
        {
            SoundManager.instance.RandomizeSfx(moveSound1, moveSound2);
        }

        CheckIfGameOver();
        GameManager.instance.playersTurn = false;
    }

    private async void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Exit")
        {
            Invoke("Restart", restartLevelDelay);
            enabled = false;
        }
        else if (other.tag == "Food")
        {
            food += pointsPerFood;
            foodText.text = "+" + pointsPerFood + " Food: " + food;
            SoundManager.instance.RandomizeSfx(eatSound1, eatSound1);
            other.gameObject.SetActive(false);

        }
        else if (other.tag == "Soda")
        {
            food += pointsPerSoda;
            foodText.text = "+" + pointsPerSoda + " Food: " + food;
            SoundManager.instance.RandomizeSfx(drinkSound1, drinkSound2);
            other.gameObject.SetActive(false);
        }
        else if (other.tag == "Stone")
        {
            SoundManager.instance.RandomizeSfx(stoneSound, stoneSound);
            other.gameObject.SetActive(false);
            var hash = await AccountManager.instance.GetAlgodStone();
            Debug.Log("mint hash: " + hash);
        }
    }

    protected override void OnCantMove<T>(T component)
    {
        Wall hitWall = component as Wall;
        hitWall.DamageWall(wallDamage);
        animator.SetTrigger("playerChop");
    }

    private void Restart()
    {
        SceneManager.LoadScene(2);
        GameManager.instance.LevelUp();
    }

    public void LoseFood(int loss)
    {
        animator.SetTrigger("playerHit");
        food -= loss;
        foodText.text = "-" + loss + " Food: " + food;
        CheckIfGameOver();
    }


    private void CheckIfGameOver()
    {
        if (food <= 0)
        {
            SoundManager.instance.PlaySingle(gameOverSound);
            SoundManager.instance.isStopped = true;
            SoundManager.instance.musicSource.Stop();
            GameManager.instance.GameOver();
            food = 100;
        }
    }
}
