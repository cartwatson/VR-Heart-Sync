using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LightPulsing : MonoBehaviour
{
    public Heartrate heartRateManager;
    public float intensityMultiplier = 1.0f;
    public float minIntensity = 0.2f;
    public float maxIntensity = 2.0f;

    public Color zone1Color = Color.white;
    public Color zone2Color = Color.yellow;
    public Color zone3Color = Color.blue;
    public Color zone4Color = Color.magenta;
    public Color zone5Color = Color.red;

    private List<Color> zoneColorList;

    private Light lightComponent;
    private float timer;
    private float beatDuration;

    void Start()
    {
        zoneColorList = new List<Color>(){
            zone1Color,
            zone2Color,
            zone3Color,
            zone4Color,
            zone5Color
        };
        lightComponent = GetComponent<Light>();
        UpdateBeatDuration();
    }

    void Update()
    {
        timer += Time.deltaTime;

        //Set Color to match HR
        lightComponent.color = zoneColorList[heartRateManager.currentZone];


        if (timer >= beatDuration)
        {
            timer = 0;
            StartCoroutine(Pulse());
            UpdateBeatDuration();
        }
    }

    IEnumerator Pulse()
    {
        float pulseTime = 0;

        while (pulseTime < beatDuration)
        {
            lightComponent.intensity = Mathf.Lerp(minIntensity, maxIntensity, Mathf.PingPong(pulseTime * intensityMultiplier, 1.0f));
            pulseTime += Time.deltaTime;
            yield return null;
        }

        lightComponent.intensity = minIntensity;
    }

    void UpdateBeatDuration()
    {
        float heartRateBPM = heartRateManager.CurrentHeartRate;
        beatDuration = 60.0f / heartRateBPM;
    }
}