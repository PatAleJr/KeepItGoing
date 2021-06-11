using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallTapInput : MonoBehaviour
{
    [SerializeField]
    private GameObject psPrefab = null;
    [SerializeField]
    private GameObject lostPsPrefab = null;
    [SerializeField]
    private float psDuration = 1f;
    private List<GameObject> particleClones = new List<GameObject>();

    void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                Vector3 touchPosWorld = Camera.main.ScreenToWorldPoint(touch.position);
                Vector2 touchPosWorld2D = new Vector2(touchPosWorld.x, touchPosWorld.y);

                int layerMask = 1 << 8;
                RaycastHit2D hitInformation = Physics2D.Raycast(touchPosWorld2D, Camera.main.transform.forward, Mathf.Infinity, layerMask);
                if (hitInformation.collider != null)
                {
                    GameObject ball = hitInformation.collider.gameObject;
                    tapBall(ball);
                }
            }
        }
    }

    void tapBall(GameObject ball)
    {
        GameObject ps = Instantiate(psPrefab);
        ps.transform.position = ball.transform.position;
        particleClones.Add(ps);
        StartCoroutine(destroyPs(psDuration, ps));

        SoundManager.soundManager.playBallTap();
        SectionMaker.sectionMaker.incrementScore();
        Destroy(ball);
    }

    IEnumerator destroyPs(float time, GameObject _ps)
    {
        yield return new WaitForSeconds(time);
        particleClones.Remove(_ps);
        Destroy(_ps);
    }

    public void lost()
    {
        foreach (GameObject ball in GameObject.FindGameObjectsWithTag("Ball"))
        {
            GameObject ps = Instantiate(lostPsPrefab);
            ps.transform.position = ball.transform.position;
            particleClones.Add(ps);
            StartCoroutine(destroyPs(psDuration, ps));
            Destroy(ball);
        }
    }
}