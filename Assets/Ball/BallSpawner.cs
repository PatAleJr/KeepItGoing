using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallSpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject ballPrefab = null;

    [SerializeField]
    private float firstBallSpawnTime = 6f;
    [SerializeField]
    private float minBallSpawnFrequency = 2f;
    [SerializeField]
    private float maxBallSpawnFrequency = 5f;
    private float nextSpawnTime;

    public bool canSpawn;

    [SerializeField]
    private float velocityToPitch = 0.05f;
    [SerializeField]
    private float minPitch = 0.8f;

    [Header ("Ball Info")]
    public float maxPeak;  //Highest peak
    public float minStartY; //Lowest possible point (for end of parabola too)
    public float maxStartY; //Highest starting point
    public float minDX;
    public float maxDX;
    public float gravity = -9.8f;
    public bool backwards;
    private float destroyX = 10;  //destroys if x is beyond this point
    public float padding = 0.1f;    //Difference between lowest possible start point and lowest possible peak

    void Start()
    {
        destroyX = maxDX / 2;
        nextSpawnTime = Time.time + firstBallSpawnTime;
        canSpawn = false;
    }

    void Update()
    {
        if (!canSpawn)
            return;

        if (Time.time > nextSpawnTime)
        {
            spawnBall();
            nextSpawnTime = Time.time + Random.Range(minBallSpawnFrequency, maxBallSpawnFrequency);
        }
    }

    public void setSpawn(bool _canSpawn)
    {
        canSpawn = _canSpawn;
        nextSpawnTime = Time.time + firstBallSpawnTime;
    }

    void spawnBall()
    {
        GameObject ball = Instantiate(ballPrefab);
        Ball bs = ball.GetComponent<Ball>();

        bs.spawner = this;
        bs.maxPeak = maxPeak;
        bs.minStartY = minStartY;
        bs.maxStartY = maxStartY;
        bs.dx = Random.Range(minDX, maxDX);
        bs.gravity = gravity;
        bs.destroyX = destroyX;
        bs.padding = padding;
    }

    public void playBallSpawnSound(float velocity)
    {
        float pitch = velocityToPitch * velocity + minPitch;
        SoundManager.soundManager.playBallSpawn(pitch);
    }
}
