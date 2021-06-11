using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCloudSpawner : MonoBehaviour
{
    [SerializeField]
    private CloudSpawn02[] cloudSpawners = { null};

    public float minCloudiness= 0.1f; //Decimal (percent)
    public float maxCloudiness = 0.7f; //Decimal (percent)
    public float cloudSpawnChance;

    private void Start()
    {
        resetClouds();
    }

    public void resetClouds()   //Called at begining of game and when enables making clouds
    {
        if (!CloudsButton.makeClouds)
            return;

        cloudSpawnChance = Random.Range(minCloudiness, maxCloudiness);

        foreach (CloudSpawn02 cloudSpawner in cloudSpawners)
        {
            cloudSpawner.gameObject.SetActive(true);
            cloudSpawner.reset(cloudSpawnChance);
        }
    }

    public void cancelClouds()  //Called when making clouds are disabled (pause menu)
    {
        foreach (CloudSpawn02 cloudSpawner in cloudSpawners)
        {
            cloudSpawner.destroyClouds(true);
            cloudSpawner.gameObject.SetActive(false);
        }
    }
}
