using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Tutorial : MonoBehaviour
{
    [SerializeField]
    private GameObject leftArrow = null;
    [SerializeField]
    private GameObject rightArrow = null;
    [SerializeField]
    private TextMeshProUGUI explanationText = null;
    [SerializeField]
    private Image image = null;

    [TextArea(2,4)]
    public string[] explanations;
    public Sprite[] sprites;

    public int index = 0;


    public void next()
    {
        index++;

        if (index == explanations.Length - 1)
            rightArrow.SetActive(false);
        if (index == 1)
            leftArrow.SetActive(true);

        explanationText.text = explanations[index];
        image.sprite = sprites[index];
    }

    public void previous()
    {
        index--;

        if (index == 0)
            leftArrow.SetActive(false);
        if (index == explanations.Length-2)
            rightArrow.SetActive(true);

        explanationText.text = explanations[index];
        image.sprite = sprites[index];
    }
}
