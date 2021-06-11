using UnityEngine;
using GooglePlayGames;
using System.Collections;
using System.Collections.Generic;


public class Achievements : MonoBehaviour
{
    public static Achievements instance;

    public Dictionary<string, int> achievements = new Dictionary<string, int>();    //Achievements and their required score

    public int A_highscore_1 = 0;
    public int A_overallScore_1 = 0;
    public int A_mostBalls_1 = 0;
    public int A_overallBalls_1 = 0;

    void Awake()
    {
        if (Achievements.instance == null)
        {
            Achievements.instance = this;
        }
        else
        {
            if (Achievements.instance != this)
            {
                Destroy(Achievements.instance);
                Achievements.instance = this;
            }
        }

        //Required values to get achievement
        achievements.Add(GPGSIds.achievement_player, 200);
        achievements.Add(GPGSIds.achievement_big_player, 1000);
        achievements.Add(GPGSIds.achievement_high_scorer, 40);
        achievements.Add(GPGSIds.achievement_waving_101, 101);
        achievements.Add(GPGSIds.achievement_ball_collector, 30);
        achievements.Add(GPGSIds.achievement_ball_fanatic, 100);
        achievements.Add(GPGSIds.achievement_ball_popper, 5);
        achievements.Add(GPGSIds.achievement_ball_boomer, 12);
    }

    public void OpenAchievementPanel()
    {
        Social.ShowAchievementsUI();
    }

    public void UpdateIncrementals()   //send to server will send it unconditionally
    {
        if (PlayerPrefs.GetInt("OverallScoreToAdd", 0) >= 15)
        {
            PlayGamesPlatform.Instance.IncrementAchievement(GPGSIds.achievement_player, PlayerPrefs.GetInt("OverallScoreToAdd", 0), (bool success) =>
            {
                if (success)
                    PlayerPrefs.SetInt("OverallScoreToAdd", 0);
            });
            PlayGamesPlatform.Instance.IncrementAchievement(GPGSIds.achievement_big_player, PlayerPrefs.GetInt("OverallScoreToAdd", 0), (bool success) =>
            {
                if (success)
                    PlayerPrefs.SetInt("OverallScoreToAdd", 0);
            });
        }
        if (PlayerPrefs.GetInt("OverallBallsToAdd", 0) >= 4)
        {
            PlayGamesPlatform.Instance.IncrementAchievement(GPGSIds.achievement_ball_collector, PlayerPrefs.GetInt("OverallBallsToAdd", 0), (bool success) =>
            {
                if (success)
                    PlayerPrefs.SetInt("OverallBallsToAdd", 0);
            });
            PlayGamesPlatform.Instance.IncrementAchievement(GPGSIds.achievement_ball_fanatic, PlayerPrefs.GetInt("OverallBallsToAdd", 0), (bool success) =>
            {
                if (success)
                    PlayerPrefs.SetInt("OverallBallsToAdd", 0);
            });
        }
    }

    public void UpdateMostBalls()
    {
        if (Manager.mostBalls >= achievements[GPGSIds.achievement_ball_popper])
        {
            Social.ReportProgress(GPGSIds.achievement_ball_popper, 100f, null);

            if (Manager.mostBalls >= achievements[GPGSIds.achievement_ball_boomer])
                Social.ReportProgress(GPGSIds.achievement_ball_boomer, 100f, null);
        }
    }

    public void UpdateHighScore()
    {
        if (Manager.highscore >= achievements[GPGSIds.achievement_high_scorer])
        {
            Social.ReportProgress(GPGSIds.achievement_high_scorer, 100f, null);

            if (Manager.highscore >= achievements[GPGSIds.achievement_waving_101])
                Social.ReportProgress(GPGSIds.achievement_waving_101, 100f, null);
        }
    }
}
