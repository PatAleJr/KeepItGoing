using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonTouch : MonoBehaviour
{
    GraphicRaycaster m_Raycaster;
    PointerEventData m_PointerEventData;
    EventSystem m_EventSystem;

    public GameObject currentButton;

    private bool canTakeInput = true;

    void Start()
    {
        m_Raycaster = GetComponent<GraphicRaycaster>();
        m_EventSystem = GetComponent<EventSystem>();
    }

    void Update()
    {
        if (PauseButton.paused || TitleManager.tutIsOpen || !canTakeInput)
            return;

        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            m_PointerEventData = new PointerEventData(m_EventSystem);
            m_PointerEventData.position = touch.position;

            Vector3 touchPosWorld = Camera.main.ScreenToWorldPoint(touch.position);
            Vector2 touchPosWorld2D = new Vector2(touchPosWorld.x, touchPosWorld.y);

            List<RaycastResult> results = new List<RaycastResult>();
            m_Raycaster.Raycast(m_PointerEventData, results);

            if (results.Count == 0 || results[0].gameObject == null)
                return;

            if (touch.phase == TouchPhase.Began)
            {
                if (results[0].gameObject.tag == "Button")
                {
                    currentButton = results[0].gameObject;
                    currentButton.GetComponent<ColorButton>().push();
                }
            }
            else if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
            {
                if (results[0].gameObject.tag == "Button")
                {
                    currentButton = results[0].gameObject;
                    currentButton.GetComponent<ColorButton>().release();
                    currentButton = null;
                }
            }
            else if (touch.phase == TouchPhase.Moved)
            {
                //Release only if no longer over button
                if (results[0].gameObject.tag != "Button")
                {
                    if (currentButton != null)
                    {
                        currentButton.GetComponent<ColorButton>().release();
                        currentButton = null;
                    }
                }
            }
        }
    }

    public void cancelTouch()
    {
        if (currentButton != null)
        {
            canTakeInput = false;
            currentButton.GetComponent<ColorButton>().release();
            currentButton = null;
            StartCoroutine(activateInput());
        }
    }

    IEnumerator activateInput()
    {
        yield return new WaitForSeconds(0.05f);
        canTakeInput = true;
    }
}
