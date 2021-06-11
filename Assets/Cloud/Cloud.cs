using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cloud : MonoBehaviour
{
    public float spd, Xscale, Yscale, Xoffset, Yoffset, spdVariation;
    public int depth;
    private SpriteRenderer sr;

    private Vector3 moveVec = new Vector3(0,0,0);

    public void setup(Sprite _sprite, float _spd, float _spdVariation, int _depth, 
        float _Xscale, float _Yscale, float _Xoffset = 0.5f, float _Yoffset = 0.2f, float _baseAlpha = 0.9f, float _depthOnAlpha = 0.25f)
    {
        spd = _spd;
        spdVariation = _spdVariation;
        Xscale = _Xscale;
        Yscale = _Yscale;
        Xoffset = _Xoffset;
        Yoffset = _Yoffset;

        depth = _depth;

        sr = GetComponent<SpriteRenderer>();
        sr.sprite = _sprite;

        Vector3 offset = new Vector3(_Xoffset, _Yoffset, 0f);
        transform.position += offset;

        Vector3 scale = new Vector3(_Xscale, _Yscale, 1f);
        transform.localScale = scale;

        sr.sortingOrder = -depth;
        Color newColor = Color.white;
        newColor.a = _baseAlpha - _depthOnAlpha*depth;
        sr.color = newColor;

        moveVec = new Vector3(spd, 0f, 0f);
    }

    public void setSpeed(float newBaseSpeed)
    {
        spd = newBaseSpeed;
        //spd += spd * spdVariation;
        moveVec = new Vector3(-spd, 0f, 0f);
    }

    private void Update()
    {
        transform.Translate(moveVec * Time.deltaTime);
    }
}
