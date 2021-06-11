using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Column : MonoBehaviour
{
    [SerializeField]
    private Guy[] myGuys = null;

    public Section.TeamColor myTeamColor = Section.TeamColor.red;

    public float satMultiplier = 1f;
    public float satGainTime = 0.5f;
    public float satLoseTime = 0.2f;
    private float startTime = 0f;
    private bool reduceSat = false;
    private bool increaseSat = false;

    [Header("Particles")]
    public bool makeParticles;
    private bool madeParticles;
    [SerializeField]
    private GameObject[] particles = null;

    [SerializeField]
    private Color[] redColors = { Color.white };
    [SerializeField]
    private Color[] blueColors = { Color.white};
    [SerializeField]
    private Color[] yellowColors = { Color.white };

    private void Start()
    {
        makeParticles = madeParticles = false;
    }

    public void setMyColor(Section.TeamColor myColor)
    {
        foreach (Guy guy in myGuys)
            guy.setMyColor(myColor);

        myTeamColor = myColor;
    }

    public void setSaturation(float _satMultiplier)
    {
        satMultiplier = _satMultiplier;
        reduceSat = true;
        increaseSat = false;
        startTime = Time.time;
    }

    void Update()
    {
        if (!ParticlesButton.makeParticles)
            return;

        if (makeParticles && !madeParticles)
        {
            Color[] colorsToChooseFrom = blueColors;

            switch (myTeamColor)
            {
                case Section.TeamColor.blue:
                    colorsToChooseFrom = blueColors;
                    break;

                case Section.TeamColor.red:
                    colorsToChooseFrom = redColors;
                    break;

                case Section.TeamColor.yellow:
                    colorsToChooseFrom = yellowColors;
                    break;
            }

            foreach (GameObject parts in particles)
            {
                Color col = colorsToChooseFrom[Random.Range(0, colorsToChooseFrom.Length)];
                parts.SetActive(true);
                ParticleSystem.MainModule settings = parts.GetComponent<ParticleSystem>().main;
                settings.startColor = col;
            }
            madeParticles = true;
        }

        if (reduceSat)
        {
            float t = (Time.time - startTime) / satLoseTime;
            float ss = Mathf.Lerp(1, satMultiplier, t);

            foreach (Guy guy in myGuys)
                guy.setSat(ss);

            if (ss - satMultiplier < 0.05f)
            {
                reduceSat = false;
                increaseSat = true;
                startTime = Time.time;
            }
        }
        else if (increaseSat)
        {
            float t = (Time.time - startTime) / satGainTime;
            float ss = Mathf.Lerp(satMultiplier, 1, t);

            foreach (Guy guy in myGuys)
                guy.setSat(ss);

            if (1 - ss < 0.05f)
                increaseSat = false;
        }
    }
}