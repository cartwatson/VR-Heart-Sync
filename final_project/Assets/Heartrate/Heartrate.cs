using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine;
using System.Net;
using System;
using System.Threading.Tasks;

public class Heartrate : MonoBehaviour
{
    public DebugMenuLogic debugMenuLogic;
    public int InitialHeartrate;
    public string API_Key;
    public int CurrentHeartRate;
    private double _ElapsedTime = 0.0;
    private int _NormalizationMin;
    private int _NormalizationMax;
    
    [HideInInspector]
    public int NormalizedHeartRate;
    public float hrMaxNormalizedHeartRate;
    public int currentZone;

    public float hrMax = 185;
    
    [SerializeField] public float fatigueValue; //Serialized for debugging

    public float isFatiguedThreshold = 0.5f;

    private float zone2Min;
    private float zone3Min;
    private float zone4Min;
    private float zone5Min;

    
    // Start is called before the first frame update
    void Start()
    {

        currentZone = 0;

        debugMenuLogic = GameObject.Find("Debug Menu").GetComponent<DebugMenuLogic>();

        _NormalizationMax = InitialHeartrate * 2;
        _NormalizationMin = Convert.ToInt32(InitialHeartrate * .66);

        // Define the HR zones based off of user max HR
        //zones: v light, light, moderate, hard, vigorous
        zone2Min = (int) Mathf.Round(60 / hrMax); //Light exercise zone 

        zone3Min = 70 / hrMax; //Moderate exercise zone 
        zone4Min = 80 / hrMax; //Hard exercise zone 
        zone5Min = 90 / hrMax; //Vigorous exercise zone 
        Debug.Log("zone2Min: " + zone2Min);
        Debug.Log("zone3Min: " + zone3Min);
        Debug.Log("zone4Min: " + zone4Min);
        Debug.Log("zone5Min: " + zone5Min);
    }

    // Update is called once per frame
    void Update()
    {



        //Get heart rate from API
        _ElapsedTime += Time.deltaTime;
        HrMaxNormalize();
        if (!debugMenuLogic.isDebug)
        {
            if (_ElapsedTime > .5)
            {
                _ElapsedTime -= .5;
                StartCoroutine(CallApi("https://dev.pulsoid.net/api/v1/data/heart_rate/latest?response_mode=text_plain_only_heart_rate"));
            }



            float fatigueChange = 0;
            //Adjust the FatigueValue
            switch (currentZone) {
                case 0:
                    fatigueChange = -0.001f;
                    break;
                case 1:
                    fatigueChange = 0.0001f; // decrease fatigue by 0.1
                    break;
                case 2:
                    if (fatigueValue < 0.5f) {
                        fatigueChange = 0.0005f; // increase fatigue by 0.2 if below threshold
                    }
                    break;
                case 3:
                    if (fatigueValue < 0.75f) {
                        fatigueChange = 0.001f; // increase fatigue by 0.5 if below threshold
                    }
                    break;
                case 5:
                    if (fatigueValue < 1) {
                        fatigueChange = 0.005f; // increase fatigue by 0.5 if below threshold
                    }
                    break;
            }
            fatigueValue = Mathf.Clamp(fatigueValue + fatigueChange * Time.deltaTime, 0f, 1f);

        }
    //Determine what zone the user is currently in
            if (zone2Min > hrMaxNormalizedHeartRate)
            {
                currentZone = 1;
            }
            if ((zone2Min <= hrMaxNormalizedHeartRate) && (hrMaxNormalizedHeartRate < zone3Min)) {
                currentZone = 1;
            Debug.Log("Currently in zone 2");
            } else if ((zone3Min <= hrMaxNormalizedHeartRate) && (hrMaxNormalizedHeartRate < zone4Min)) {
                currentZone = 2;
            } else if ((zone4Min <= hrMaxNormalizedHeartRate) && (hrMaxNormalizedHeartRate < zone5Min)) {
                currentZone = 3;
            } else if (zone5Min <= hrMaxNormalizedHeartRate) {
                currentZone = 4;
            }
    }
    
    private IEnumerator CallApi(string uri)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
        {
            // Request and wait for the desired page.
            webRequest.SetRequestHeader("Content-Type", "application/json");
            webRequest.SetRequestHeader("Authorization", "bearer efed7e1b-4293-4900-8036-c8ac90d06a88");
            
            yield return webRequest.SendWebRequest();
            
            string[] pages = uri.Split('/');
            int page = pages.Length - 1;

            switch (webRequest.result)
            {
                case UnityWebRequest.Result.ConnectionError:
                case UnityWebRequest.Result.DataProcessingError:
                    Debug.LogError(pages[page] + ": Error: " + webRequest.error);
                    break;
                case UnityWebRequest.Result.ProtocolError:
                    Debug.LogError(pages[page] + ": HTTP Error: " + webRequest.error);
                    break;
                case UnityWebRequest.Result.Success:
                    Debug.Log(pages[page] + ":\nReceived: " + webRequest.downloadHandler.text);
                    CurrentHeartRate = int.Parse(webRequest.downloadHandler.text);
                    NormalizeHeartrate();
                    break;
            }     
        }
    }

    public bool checkIsExhausted()
    {
        if (fatigueValue > isFatiguedThreshold) {return true;}
        return false;
    }

    public void NormalizeHeartrate()
    {
        float floatMath = (CurrentHeartRate - _NormalizationMin) / (float)(_NormalizationMax - _NormalizationMin) * 100;
        NormalizedHeartRate = Convert.ToInt32(floatMath);
        //Debug.Log($"Current: {CurrentHeartRate} Min: {_NormalizationMin} \nMax: {_NormalizationMax} Normalized: {NormalizedHeartRate}");
    }

    
    // HRmax Normalization
    private void HrMaxNormalize()
    {
        float normalizedValue = CurrentHeartRate / hrMax;
        Debug.Log("NormalizedValue HRMAX: " + normalizedValue);
        int minValue = int.MinValue;
        int maxValue = int.MaxValue;
        hrMaxNormalizedHeartRate = Mathf.Clamp(normalizedValue, minValue, maxValue);
    }
}
