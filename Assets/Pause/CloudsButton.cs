using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CloudsButton : MonoBehaviour
{
    public static bool makeClouds = true;

    public Image buttonImage;
    public Color onColor, offColor;

    public MainCloudSpawner cloudSpawnerMain;

    void Start()
    {
        makeClouds = true;
        buttonImage.color = onColor;
    }

    public void pushButton()
    {
        makeClouds = !makeClouds;

        if (makeClouds)
        {
            buttonImage.color = onColor;
            cloudSpawnerMain.resetClouds();
        }
        else {
            buttonImage.color = offColor;
            cloudSpawnerMain.cancelClouds();
        }
    }
}
