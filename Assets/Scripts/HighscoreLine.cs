using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HighscoreLine : MonoBehaviour
{
    public bool moveBack = false;
    public float moveSpeed;
    public float sectionWidth = 6f;
    public Vector3 destination;

    void Update()
    {
        if (moveBack)
        {
            Vector3 moveVec = new Vector3(-moveSpeed * Time.deltaTime, 0f, 0f);
            transform.localPosition += moveVec;
            if (transform.localPosition.x <= destination.x)
            {
                moveBack = false;
                transform.localPosition = destination;
            }
        }
    }

    public void changeScore()
    {
        destination = new Vector3(transform.localPosition.x - sectionWidth, 0f, 0f);
        moveBack = true;
    }
}