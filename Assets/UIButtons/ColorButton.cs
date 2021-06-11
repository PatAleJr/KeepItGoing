using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColorButton : MonoBehaviour
{
    [SerializeField]
    private Color normColor = Color.white;
    [SerializeField]
    private Color pushColor = Color.white;
    private Image myImage;

    public Section.TeamColor myColor;

    public SectionMaker SectionMaker;

    private Animator animator;

    void Start()
    {
        myImage = GetComponent<Image>();
        animator = GetComponent<Animator>();
    }

    public void push()
    {
        myImage.color = pushColor;
        animator.SetBool("Press", true);
        SectionMaker.pushButton((int)myColor);
    }
    public void release()
    {
        myImage.color = normColor;
        animator.SetBool("Press", false);
    }
}
