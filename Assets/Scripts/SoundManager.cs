using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundManager : MonoBehaviour
{
    public static SoundManager soundManager;

    [SerializeField]
    private AudioSource[] cheerSources = null;
    [SerializeField]
    private AudioSource clapSource = null;
    [SerializeField]
    private AudioSource[] booSources = null;
    [SerializeField]
    private AudioClip clapClip = null;
    [SerializeField]
    private AudioClip failClip = null;

    public bool cheer = false;
    public bool clap = true;
   
    [SerializeField]
    private List<AudioClip> cheerClips = new List<AudioClip>();
    private AudioClip c_currentItem;
    private int c_currentPosition = -1;
    public int c_Size { get { return cheerClips.Count; } }

    [SerializeField]
    private List<AudioClip> booClips = new List<AudioClip>();
    private AudioClip b_currentItem;
    private int b_currentPosition = -1;
    public int b_Size { get { return booClips.Count; } }

    [Header("Beeps")]
    [SerializeField]
    private AudioSource beepSource = null;

    [Header("Balls")]
    [SerializeField]
    private AudioSource ballSpawnSource = null;
    [SerializeField]
    private AudioSource ballTapSource = null;

    public AudioMixer mainAG;

    private void Awake()
    {
        if (SoundManager.soundManager == null)
        {
            SoundManager.soundManager = this;
        }
        else
        {
            if (SoundManager.soundManager != this)
            {
                Destroy(SoundManager.soundManager);
                SoundManager.soundManager = this;
            }
        }
    }

    void Start()
    {
        clapSource.clip = clapClip;
        clapSource.Play();

        changeVolume(-10f);
    }

    void Update()
    {
        if (cheer)
        {
            for (int i = 0; i < cheerSources.Length; i++)
            {
                if (!cheerSources[i].isPlaying)
                {
                    playCheer(i);
                }
            }
        }
    }

    public void Lost()
    {
        clap = false;
        cheer = false;
        clapSource.Stop();

        //Boo here
        foreach (AudioSource source in booSources)
        {
            source.clip = getBoo();
            source.pitch = Random.Range(0.8f, 1.2f);
            source.volume = Random.Range(0.8f, 1.2f);
            source.Play();
        }

        beepSource.volume = 1;
        beepSource.pitch = 1;
        beepSource.clip = failClip;
        beepSource.Play();
    }


    public void resetGame()
    {
        clap = true;
        cheer = false;
        clapSource.Play();
    }

    public void gameStart()
    {
        clap = true;
        cheer = true;
    }

    void playCheer(int source)
    {
        if (PauseButton.paused)
            return;

        cheerSources[source].clip = getCheer();

        cheerSources[source].pitch = Random.Range(0.8f, 1.2f);
        cheerSources[source].volume = Random.Range(0.7f, 1f);
        cheerSources[source].Play();
    }

    public void playBallSpawn(float _pitch)
    {
        ballSpawnSource.pitch = _pitch;
        ballSpawnSource.Play();
    }
    public void playBallTap()
    {
        ballTapSource.Play();
    }

    private AudioClip getCheer()
    {
        if (c_currentPosition < 1)
        {
            c_currentPosition = c_Size - 1;
            c_currentItem = cheerClips[0];
            return c_currentItem;
        }

        int pos = Random.Range(0, c_currentPosition);

        c_currentItem = cheerClips[pos];
        cheerClips[pos] = cheerClips[c_currentPosition];
        cheerClips[c_currentPosition] = c_currentItem;
        c_currentPosition--;

        return c_currentItem;
    }

    private AudioClip getBoo()
    {
        if (b_currentPosition < 1)
        {
            b_currentPosition = b_Size - 1;
            b_currentItem = cheerClips[0];
            return b_currentItem;
        }

        int pos = Random.Range(0, b_currentPosition);

        b_currentItem = booClips[pos];
        booClips[pos] = booClips[b_currentPosition];
        booClips[b_currentPosition] = b_currentItem;
        b_currentPosition--;

        return b_currentItem;
    }

    public void pauseGame()
    {
        clapSource.volume = 0f;
        foreach (AudioSource source in cheerSources)
            source.volume = 0f;
        foreach (AudioSource source in booSources)
            source.volume = 0f;
    }

    public void resumeGame()
    {
        clapSource.volume = 1f;
        foreach (AudioSource source in cheerSources)
            source.volume = 1f;
        foreach (AudioSource source in booSources)
            source.volume = 1f;
    }

    public void changeVolume(float volume)
    {
        if (volume <= -20)
            volume = -80;

        mainAG.SetFloat("MasterVolume", volume);
    }
}
