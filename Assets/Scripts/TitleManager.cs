using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleManager : MonoBehaviour
{
    public Animator tutorial;

    public static bool tutIsOpen = false;

    private void Start()
    {
        tutIsOpen = false;
    }

    public void activateTut()
    {
        tutorial.SetBool("IsThere", true);
        tutIsOpen = true;
    }

    public void deactivateTut()
    {
        tutorial.SetBool("IsThere", false);
        tutIsOpen = false;
    }
}
