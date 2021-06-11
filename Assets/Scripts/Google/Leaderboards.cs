using UnityEngine;

public class Leaderboards : MonoBehaviour
{
    public static Leaderboards instance;

    void Awake()
    {
        if (Leaderboards.instance == null)
        {
            Leaderboards.instance = this;
        }
        else
        {
            if (Leaderboards.instance != this)
            {
                Destroy(Leaderboards.instance);
                Leaderboards.instance = this;
            }
        }
    }
    public void OpenLeaderboards()
    {
        Social.ShowLeaderboardUI();
    }

    public void UpdateLeaderboardHighscore()
    {
        if (PlayerPrefs.GetInt("HighscoreToUpdate") == 0)
            return;

        Social.ReportScore(PlayerPrefs.GetInt("HighscoreToUpdate"), GPGSIds.leaderboard_high_score, (bool success) =>
        {
            if (success)
                PlayerPrefs.SetInt("HighscoreToUpdate", 0);
        });
    }
}

