using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Score : MonoBehaviour
{
    public static int overallScore = 0;
    public static int highScore = 0;

    public void IncrementOverallScore(int newScore)
    {
        overallScore += newScore;

        PlayerPrefs.SetInt("OverallScoreToUpdate", PlayerPrefs.GetInt("OverallScoreToUpdate", 0) + 1);
    }
}
