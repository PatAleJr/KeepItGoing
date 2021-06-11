using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ParticlesButton : MonoBehaviour
{
    public static bool makeParticles = false;

    public Image buttonImage;
    public Color onColor, offColor;

    void Start()
    {
        makeParticles = false;
        buttonImage.color = offColor;
    }

    public void pushButton()
    {
        makeParticles = !makeParticles;
        buttonImage.color = makeParticles ? onColor : offColor;
    }
}
