using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PauseButton : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI scoreText = null;
    [SerializeField]
    private TextMeshProUGUI highScoreText = null;

    public static bool paused = false;

    [SerializeField]
    private GameObject pauseMenu = null;
    [SerializeField]
    private GameObject pauseButton = null;
    private Animator pauseButtonAC = null;
    [SerializeField]
    private TextMeshProUGUI resumeTimeText = null;
    private int resumeTime;
    public int timeBeforeResume;  //What the timer starts at

    private void Start()
    {
        pauseButtonAC = pauseButton.GetComponent<Animator>();
    }

    public void onPausePressed()
    {
        if (paused)
        {
            resume();
        }
        else {
            pause();
        }
    }

    void pause()
    {
        paused = true;

        pauseMenu.SetActive(true);
        pauseButton.SetActive(false);

        if (Manager.highscore < SectionMaker.sectionMaker.score)
            Manager.highscore = SectionMaker.sectionMaker.score;

        highScoreText.text = "High Score: " + Manager.highscore;
        scoreText.text = "Score: " + SectionMaker.sectionMaker.score;
        Time.timeScale = 0f;
        SoundManager.soundManager.pauseGame();
    }

    void resume()   //Start timer to resume
    {
        pauseMenu.SetActive(false);

        resumeTimeText.gameObject.SetActive(true);
        resumeTime = timeBeforeResume;
        resumeTimeText.text = resumeTime.ToString();
        StartCoroutine(countForResume());
    }

    void resumeGame()   //Actually resumes the game
    {
        pauseButton.SetActive(true);
        pauseButtonAC.SetBool("IsThere", true);    //Deactivating object resets animator. Must do this

        paused = false;
        Time.timeScale = 1f;
        SoundManager.soundManager.resumeGame();
        resumeTimeText.gameObject.SetActive(false);
    }

    private IEnumerator countForResume()
    {
        while (resumeTime > 0)
        {
            yield return StartCoroutine(WaitForRealSeconds(1f));    //Runs code every second
            resumeTime--;
            resumeTimeText.text = resumeTime.ToString();
        }
        if (resumeTime <= 0)
            resumeGame();
    }

    IEnumerator WaitForRealSeconds(float time)  //Like WaitForSeconds() but works with Time.timeScale = 0
    {
        float start = Time.realtimeSinceStartup;
        while (Time.realtimeSinceStartup < start + time)
        {
            yield return null;
        }
    }
}
