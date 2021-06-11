using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Guy : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer Head = null;
    [SerializeField]
    private SpriteRenderer[] Hands = { null, null};
    [SerializeField]
    private SpriteRenderer Body = null;

    public Section.TeamColor myTeamColor = Section.TeamColor.red;

    public Color[] redColors = null;
    public Color[] yellowColors = null;
    public Color[] blueColors = null;

    public Color[] skinColors = null;

    private Color clothesColor;
    private Color skinColor;

    public int baseOrder = 3000;

    public float maxXoffset;

    public float widthDif;
    public float heightDif;

    [Header("Animation")]
    private float C_originalH;
    public float C_originalS;
    private float C_originalV;

    private float S_originalH;
    public float S_originalS;
    private float S_originalV;

    private void Start()
    {
        //Adds random X
        float xOffset = Random.Range(-maxXoffset, maxXoffset);
        transform.position += new Vector3(xOffset, 0, 0);

        //Adds random scale
        float width = Random.Range(-widthDif, widthDif);
        float height = Random.Range(-heightDif, heightDif);
        transform.localScale += new Vector3(width, height, 1);

        //Sets random skin color
        int skinColorIndex = Random.Range(0, skinColors.Length);
        skinColor = skinColors[skinColorIndex];
        Head.color = skinColor;
        Hands[0].color = skinColor;
        Hands[1].color = skinColor;

        //Set depth based on Y position
        int order = (int)(baseOrder - transform.position.y * 4);    //*4 to be more precise
        Head.sortingOrder = order;
        Body.sortingOrder = order;
        Hands[0].sortingOrder = order+1;    //Hands must be seen above body
        Hands[1].sortingOrder = order+1;

        //Set original stuff
        Color.RGBToHSV(skinColor, out S_originalH, out S_originalS, out S_originalV);
        Color.RGBToHSV(clothesColor, out C_originalH, out C_originalS, out C_originalV);
    }

    public void setMyColor(Section.TeamColor myColor)
    {
        myTeamColor = myColor;
        int colorIndex;
        switch (myTeamColor)
        {
            case Section.TeamColor.red:
                colorIndex = Random.Range(0, redColors.Length);
                clothesColor = redColors[colorIndex];
                break;

            case Section.TeamColor.yellow:
                colorIndex = Random.Range(0, yellowColors.Length);
                clothesColor = yellowColors[colorIndex];
                break;
            case Section.TeamColor.blue:
                colorIndex = Random.Range(0, blueColors.Length);
                clothesColor = blueColors[colorIndex];
                break;
        }
        Body.color = clothesColor;
    }

    public void setSat(float satMultiplier)
    {
        float newS, newV;
        Color newColor;

        //Clothes
        newS = C_originalS * satMultiplier;
        newV = Mathf.Lerp(C_originalV, 1, C_originalS - newS);
        newV = ((C_originalV - 1) / (C_originalS)) * C_originalS * satMultiplier + 1;
        newColor = Color.HSVToRGB(C_originalH, newS, newV, true);
        Body.color = newColor;

        //Skin
        newS = S_originalS * satMultiplier;
        newV = Mathf.Lerp(S_originalV, 1, S_originalV - newS);
        newV = ((S_originalV - 1) / (S_originalS)) * S_originalS * satMultiplier + 1;
        newColor = Color.HSVToRGB(S_originalH, newS, newV, true);
        foreach (SpriteRenderer hand in Hands)
            hand.color = newColor;
        Head.color = newColor;
    }
}
