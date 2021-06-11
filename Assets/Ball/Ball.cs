using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    private float minPeak;  //Lowest peak
    public float maxPeak;  //Highest peak

    public float padding = 0.2f;

    public float minStartY; //Lowest possible point (for end of parabola too)
    public float maxStartY; //Highest starting point
    public float gravity = -9.8f;
    public float dx;

    private float startY;
    private float velocityX;
    private float velocityY;
    private float startTime;

    public bool backwards;

    public float destroyX;  //destroys if x is beyond this point

    private Camera cam;

    public float rotationSpeed;
    public float maxRotSpeed;
    private Vector3 rotVector;

    private bool playedSound = false;
    [SerializeField]
    private float playSoundX = 9f;
    public BallSpawner spawner = null;

    void Start()
    {
        cam = Camera.main;

        minPeak = Random.Range(minStartY + padding, maxPeak);

        startY = Random.Range(minStartY, minPeak);
        transform.position = new Vector3(-dx/2, startY, 0f);
        startTime = Time.time;

        backwards = Random.Range(-1, 1) == 0 ? true : false;
        if (backwards)
            transform.position = new Vector3(dx / 2, startY, 0f);

        //Finds velocity y where peak is between min and max
        float minVy = Mathf.Sqrt(4 * (0.5f*gravity * (startY-minPeak)));
        float maxVy = Mathf.Sqrt(4 * (0.5f * gravity * (startY - maxPeak)));
        velocityY = Random.Range(minVy, maxVy);
        while (velocityY == minVy || velocityY == maxVy)    //make it exclusive
        {
            velocityY = Random.Range(minVy, maxVy);
        }

        //finds time before ball passes buttonY
        float delta = (velocityY * velocityY) - (4 * 0.5f*gravity*(startY-minStartY));
        float t2 = (-velocityY - Mathf.Sqrt(delta)) / gravity;
        velocityX = dx / t2;

        //Rotation
        rotationSpeed = Random.Range(-maxRotSpeed, maxRotSpeed);
        rotVector = new Vector3(0f, 0f, rotationSpeed);
    }

    void Update()
    {
        //Move
        float tt = Time.time - startTime;
        float xx = velocityX * tt - dx/2;
        float yy = velocityY * tt + 0.5f*(gravity * tt * tt) + startY;
        if (backwards)
            xx *= -1;
        transform.position = new Vector3(cam.transform.position.x + xx, yy, 0f);

        if (Mathf.Abs(xx) > destroyX)
        {
            Destroy(gameObject);
        }

        //Rotate
        transform.Rotate(rotVector * Time.deltaTime);

        //Plays spawn sound
        if (Mathf.Abs(transform.position.x) < playSoundX && !playedSound)
        {
            spawner.playBallSpawnSound(velocityX);
            playedSound = true;
        }
    }
}