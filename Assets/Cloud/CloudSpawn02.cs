using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudSpawn02 : MonoBehaviour
{
    public List<Sprite> sprites;
    public float destroyX = -10f;

    [Header("Speed")]
    public float[] depthSpeed;  //Speed of each depth (multiplier of section speed)
    public float speedVariation = 0.1f; //Variation percent for each cloud (multiplier)
    public float sectionSpeed;
    public float idleSpeed = 1f; //Speed when sections arent moving
    public float changeSpeedFrequency = 0.2f;
    private float nextChangeSpeedTime;

    [Header("Alpha / Scale / Offset")]
    public float baseAlpha = 0.9f;
    public float depthOnAlpha = 0.2f;
    public float depthOnScale = 0.1f;
    public float maxScale = 0.2f;
    public float minScale = 0.1f;
    public float depthOnOffset = 0.5f;
    public float maxXoffset, maxYoffset = 0.2f;

    [Header("Spawning")]
    public int myDepth = 0;
    private float coveredDistance;
    public float spawnDistance = 2f;
    public float spawnChance = 0.4f;
    public GameObject cloudPrefab;
    public List<Transform> spawnPoints; //0 is regular spawning. Others are for initial

    public List<GameObject> clouds;

    public void reset(float _spawnChance)
    {
        spawnChance = _spawnChance;

        destroyClouds(true);

        sectionSpeed = idleSpeed;
        if (SectionMaker.sectionMaker.isPlaying)
            sectionSpeed = SectionMaker.sectionMaker.currentSpeed + idleSpeed;

        for (int i = 0; i < spawnPoints.Count; i++)
        {
            if (Random.Range(0f, 1f) <= spawnChance)
                spawnCloud(i);
        }

        coveredDistance = 0f;
        nextChangeSpeedTime = Time.time + changeSpeedFrequency;
    }

    void Update()
    {
        if (Time.time > nextChangeSpeedTime)
        {
            changeCloudSpeed();
            nextChangeSpeedTime = Time.time + changeSpeedFrequency;
        }

        coveredDistance += Time.deltaTime * SectionMaker.sectionMaker.currentSpeed * depthSpeed[myDepth];

        if (coveredDistance >= spawnDistance)
        {
            if (Random.Range(0f, 1f) <= spawnChance)
                spawnCloud(0);

            coveredDistance = 0f;

            destroyClouds();

            changeCloudSpeed();
        }
    }

    public void destroyClouds(bool destroyAll = false)
    {
        List<GameObject> cloudsToRemove = new List<GameObject>();

        foreach (GameObject cloud in clouds)    //Find which ones need to be destroyed
        {
            if (destroyAll || cloud.transform.position.x <= destroyX)
                cloudsToRemove.Add(cloud);
        }
        foreach (GameObject cloud in cloudsToRemove)    //Destroy them
        {
            clouds.Remove(cloud);
            Destroy(cloud);
        }
    }

    void changeCloudSpeed()
    {
        sectionSpeed = idleSpeed;
        if (SectionMaker.sectionMaker.isPlaying)
            sectionSpeed = SectionMaker.sectionMaker.currentSpeed + idleSpeed;

        foreach (GameObject cloud in clouds)
        {
            Cloud _cloud = cloud.GetComponent<Cloud>();
            float _spd = sectionSpeed * depthSpeed[_cloud.depth];
            _cloud.setSpeed(_spd);
        }
    }

    public void spawnCloud(int spawnIndex = 0)
    {
        //Spawn it
        GameObject cloudGO = Instantiate(cloudPrefab, spawnPoints[spawnIndex]);
        Cloud cloud = cloudGO.GetComponent<Cloud>();
        clouds.Add(cloudGO);

        //Depth
        int _depth = myDepth;

        //Speed
        float _spd = sectionSpeed * depthSpeed[_depth];
        float _spdVariation = Random.Range(-speedVariation, speedVariation);

        //Scale
        float _Xscale = Random.Range(minScale, maxScale);
        if (Random.Range(0, 2) == 1)
            _Xscale *= -1;
        float _Yscale = Random.Range(minScale, maxScale);

        _Xscale -= depthOnScale * _depth;
        _Yscale -= depthOnScale * _depth;

        //Offsets
        float _Xoffset = Random.Range(-maxXoffset, maxXoffset);
        float _Yoffset = Random.Range(-maxYoffset, maxYoffset) + depthOnOffset * _depth;

        //Sprite and sets it
        int spriteIndex = Random.Range(0, sprites.Count);

        cloud.setup(sprites[spriteIndex], _spd, _spdVariation, _depth,  _Xscale, _Yscale, _Xoffset, _Yoffset, baseAlpha, depthOnAlpha);
    }
}
