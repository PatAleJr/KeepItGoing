using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Manager : MonoBehaviour
{
    public static Manager manager;

    //Main info
    public static int highscore = 0;
    public static int mostBalls = 0;

    [SerializeField]
    private int _hiscore = 0;   //so can see in inspector
    public GameObject endGame;  //Game part of canvas

    public Animator pauseButtonAnim;

    public float canvasDelay = 1f;
    public float inputDelay = 2f;
    public float resetGameDelay = 0.3f;

    public bool gameSet = true;
    public bool canReset = false;

    [SerializeField]
    BallSpawner ballSpawner = null;
    [SerializeField]
    BallTapInput tapInput = null;
    [SerializeField]
    MainCloudSpawner cloudSpawnerMain = null;
    [SerializeField]
    ButtonTouch buttonTouch = null;

    [Header("End Text")]
    [SerializeField]
    private TextMeshProUGUI scoreText = null;
    [SerializeField]
    private TextMeshProUGUI reasonText = null;
    [SerializeField]
    private TextMeshProUGUI highscoreText = null;

    [SerializeField]
    private TextMeshProUGUI titleHighscoreText = null;

    private void Awake()
    {
        if (Manager.manager == null)
        {
            Manager.manager = this;
        }
        else {
            if (Manager.manager != this)
            {
                Destroy(Manager.manager);
                Manager.manager = this;
            }
        }

        DontDestroyOnLoad(gameObject);

        StartCoroutine(loadSaveFile());

        //Leaderboards.instance.UpdateLeaderboardHighscore(); //In case didnt update during last play session
    }

    IEnumerator loadSaveFile()
    {
        yield return new WaitForEndOfFrame();
        highscore = SaveManager.Instance.getHighscore();
        titleHighscoreText.text = "Highscore: " + highscore;
    }

    void Update()
    {
        if (Input.anyKeyDown)
        {
            if (canReset && !gameSet)
            {
                resetGame();
            }
        }
    }

    public void startedPlaying()
    {
        SoundManager.soundManager.gameStart();
        ballSpawner.setSpawn(true);
        pauseButtonAnim.SetBool("IsThere", true);
    }

    void resetGame()
    {
        canReset = false;
        StartCoroutine(resetSections());
        endGame.GetComponent<Animator>().SetTrigger("Reset");

        titleHighscoreText.text = "Highscore: " + highscore;
    }

    public void lose(int score, string reason, int numBalls)
    {
        canReset = false;
        gameSet = false;

        pauseButtonAnim.SetBool("IsThere", false);

        PlayerPrefs.SetInt("OverallScoreToAdd", PlayerPrefs.GetInt("OverallScoreToAdd", 0) + score);
        PlayerPrefs.SetInt("OverallBallsToAdd", PlayerPrefs.GetInt("OverallBallsToAdd", 0) + numBalls);

        //Achievements.instance.UpdateIncrementals(); //Checks if should send to server, and then sends it

        if (score > highscore || numBalls > mostBalls)
        {
            //Beat highscore
            if (score > highscore)
            {
                highscore = score;
                PlayerPrefs.SetInt("HighscoreToUpdate", highscore);
                //Leaderboards.instance.UpdateLeaderboardHighscore();
                //Achievements.instance.UpdateHighScore();

                SaveManager.Instance.setHighscore(highscore);
            }

            //Beat Balls
            if (numBalls > mostBalls)
            {
                mostBalls = numBalls;
                //Achievements.instance.UpdateMostBalls();
            }
        }

        _hiscore = highscore;

        SoundManager.soundManager.Lost();
        ballSpawner.setSpawn(false);
        tapInput.lost();

        CameraShake.camShake.Shake(0.2f, 0.1f);

        StartCoroutine(activateLossCanvas());
        StartCoroutine(activateInput());

        scoreText.text = score.ToString();
        reasonText.text = reason;
        highscoreText.text = "Highscore: " + highscore;
    }

    IEnumerator activateLossCanvas()
    {
        yield return new WaitForSeconds(canvasDelay);
        endGame.GetComponent<Animator>().SetTrigger("Lose");
        buttonTouch.cancelTouch();
    }
    IEnumerator activateInput()
    {
        yield return new WaitForSeconds(inputDelay);
        canReset = true;
    }
    IEnumerator resetSections()
    {
        yield return new WaitForSeconds(resetGameDelay);
        SectionMaker.sectionMaker.setGame();
        SoundManager.soundManager.resetGame();
        cloudSpawnerMain.resetClouds();
        gameSet = true;
    }
}
