using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteRenderLighting : MonoBehaviour
{
    [Header("HR Integration Settings")]
    [SerializeField] public Heartrate heartRateData;

    private SpriteRenderer mySpriteRenderer;
    void Start()
    {
        mySpriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        float colorVal = (1 - (heartRateData.NormalizedHeartRate / 100f));
        if (colorVal > 1) colorVal = 1;
        else if (colorVal < 0) colorVal = 0;
        
        var myColor = new Color(colorVal, colorVal, colorVal);
        mySpriteRenderer.color = myColor;
        // Debug.Log("sprite renderer color " + mySpriteRenderer.color);
    }
}
