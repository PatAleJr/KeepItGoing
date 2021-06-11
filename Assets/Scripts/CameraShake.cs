using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public static CameraShake camShake;

    void Awake() { 
        if (CameraShake.camShake == null)
        {
            CameraShake.camShake = this;
        }
        else {
            if (CameraShake.camShake != this)
            {
                Destroy(CameraShake.camShake);
                CameraShake.camShake = this;
            }
        }
    }

    public void Shake(float _duration, float _magnitude)
    {
        StartCoroutine(shake(_duration, _magnitude));
    }

    public IEnumerator shake(float duration, float magnitude)
    {
        Vector3 originalPos = transform.localPosition;

        float elapsed = 0f;

        while (elapsed < duration)
        {
            float x = Random.Range(-1, 1) * magnitude;
            float y = Random.Range(-1, 1) * magnitude;

            transform.localPosition = new Vector3(x, y, originalPos.z);

            elapsed += Time.deltaTime;

            if (PauseButton.paused)
                break;

            yield return null;
        }

        transform.localPosition = originalPos;
    }
}
