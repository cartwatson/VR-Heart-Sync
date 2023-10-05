using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class LightIntensityHighHRLowers : MonoBehaviour
{
    [Header("HR Integration Settings")]
    [SerializeField] public Heartrate heartRateData;

    private Light myLight = null;

    void Start()
    {
        myLight = this.GetComponent<Light>();
    }

    void Update()
    {
        // format heartRateData
        //Debug.Log(heartRateData.NormalizedHeartRate);
        myLight.intensity = 8 * (1 - (heartRateData.NormalizedHeartRate / 100f));
    }
}
