using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    public float levelStartDelay = 0.2f;
    public float turnDelay = .1f;
    public static GameManager instance = null;
    public BoardManger boardScript;
    public int playerFoodPoints = 100;
    [HideInInspector] public bool playersTurn = true;


    private Text levelText;
    private GameObject levelImage;

    private int level = 1;
    private List<Enemy> enemies;
    private bool enemiesMoving;
    private bool doingSetup;

    private bool justGameOver = false;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
        enemies = new List<Enemy>();
        boardScript = GetComponent<BoardManger>();
    }

    private void OnLevelWasLoaded(int index)
    {
        int buildIndex = SceneManager.GetActiveScene().buildIndex;
        if (buildIndex == 2)
        {
            if (SoundManager.instance.isStopped)
            {
                SoundManager.instance.musicSource.Play();
                SoundManager.instance.isStopped = false;
            }
            if (!enabled)
            {
                enabled = true;
            }
            InitGame();
        }
    }

    public void LevelUp()
    {
        level++;
    }

    void InitGame()
    {
        doingSetup = true;
        levelImage = GameObject.Find("LevelImage");
        levelText = GameObject.Find("LevelText").GetComponent<Text>();
        levelText.text = "Block " + level;
        levelImage.SetActive(true);
        Invoke("HideLevelImage", levelStartDelay);
        enemies.Clear();
        boardScript.SetupScene(level);
    }

    private void HideLevelImage()
    {
        levelImage.SetActive(false);
        doingSetup = false;
    }

    public void GameOver()
    {
        levelText.text = "You starved at Block " + level + ".";
        levelImage.SetActive(true);
        enabled = false;
        level = 1;
        Invoke("MoveToStatus", 2f);
        // SceneManager.LoadScene(0);

    }

    private void MoveToStatus()
    {
        SceneManager.LoadScene(0);
    }

    void Update()
    {
        if (playersTurn || enemiesMoving || doingSetup)
            return;
        StartCoroutine(MoveEnemies());
    }

    public void AddEnemyToList(Enemy script)
    {
        enemies.Add(script);
    }

    IEnumerator MoveEnemies()
    {
        enemiesMoving = true;
        yield return new WaitForSeconds(turnDelay);
        if (enemies.Count == 0)
        {
            yield return new WaitForSeconds(turnDelay);
        }

        for (int i = 0; i < enemies.Count; i++)
        {
            enemies[i].MoveEnemy();
            yield return new WaitForSeconds(enemies[i].moveTime);
        }
        playersTurn = true;
        enemiesMoving = false;

    }

}
