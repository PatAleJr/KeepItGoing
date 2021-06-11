using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance { set; get; }
    public SaveState state;

    private void Awake()
    {
        Instance = this;
        Load();
    }

    public int getHighscore()   //Called at awake on Manager
    {
        Load();
        return state.highscore;
    }

    public void setHighscore(int newScore)  //Called on lose of Manager
    {
        state.highscore = newScore;
        Save();
    }



    // Save the whole state of saveState script to playerpref
    public void Save()
    {
        PlayerPrefs.SetString("save", Helper.Serialize<SaveState>(state));
    }

    // Load the previous save state from player pref
    public void Load()
    {
        //Has save file?
        if (PlayerPrefs.HasKey("save"))
        {
            state = Helper.Deserialize<SaveState>(PlayerPrefs.GetString("save"));
        } else {
            state = new SaveState();
            Save();
            Debug.Log("No save file found, creating a new one");

        }
    }


}