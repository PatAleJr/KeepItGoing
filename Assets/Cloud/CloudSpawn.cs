using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudSpawn : MonoBehaviour
{
    public int maxNumToSpawn = 2;
    public float minSpd = -0.5f;
    public float maxSpd = -0.05f;
    public int maxDepth = 3;
    public float maxScale = 0.2f;
    public float minScale = 0.1f;
    public float spdVariation = 0.1f;

    [Header("Effectors")]
    public float depthOnSpeed = 0.1f;
    public float depthOnOffset = 0.5f;
    public float depthOnScale = 0.1f;
    public float depthOnAlpha = 0.2f;

    public List<Sprite> sprites;

    public float maxXoffset, maxYoffset = 0.2f;

    public GameObject cloudPrefab;
    public List<Transform> spawnPoints;

    void Start()
    {
        int numToSpawn = Random.Range(0, maxNumToSpawn+1);
        if (numToSpawn > spawnPoints.Count)
            numToSpawn = spawnPoints.Count;

        for (int i = 0; i < numToSpawn - 1; i++)
        {
            //Spawn it
            int spawnIndex = Random.Range(0, spawnPoints.Count);
            GameObject cloudGO = Instantiate(cloudPrefab, spawnPoints[spawnIndex]);
            spawnPoints.Remove(spawnPoints[spawnIndex]);
            Cloud cloud = cloudGO.GetComponent<Cloud>();

            //Depth
            int _depth = Random.Range(0, maxDepth + 1);
            
            //Speed
            float _spd = Random.Range(minSpd, maxSpd);
            _spd *= (1 - (_depth * depthOnSpeed));

            float _spdVariation = Random.Range(-spdVariation, spdVariation);

            //Scale
            float _Xscale = Random.Range( minScale, maxScale);
            if (Random.Range(0, 2) == 1)
                _Xscale *= -1;
            float _Yscale = Random.Range( minScale, maxScale);

            _Xscale -= depthOnScale * _depth;
            _Yscale -= depthOnScale * _depth;

            //Offsets
            float _Xoffset = Random.Range(-maxXoffset, maxXoffset);
            float _Yoffset = Random.Range(-maxYoffset, maxYoffset) + depthOnOffset * _depth;

            //Sprite and sets it
            int spriteIndex = Random.Range(0, sprites.Count);
            cloud.setup(sprites[spriteIndex], _spd, _spdVariation, _depth, _Xscale, _Yscale, _Xoffset, _Yoffset);
            sprites.Remove(sprites[spriteIndex]);
        }

        Destroy(this);
    }
}
