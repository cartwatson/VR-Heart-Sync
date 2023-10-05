using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; 

public class DebugHudUpdate : MonoBehaviour
{
    // Start is called before the first frame update
    public Heartrate heartrate;
    private DebugMenuLogic debugMenuLogic;
    private TextMeshProUGUI zoneLabel;
    private TextMeshProUGUI fatigueValueLabel;
    private TextMeshProUGUI hrMaxLabel;
    private TextMeshProUGUI normalizedHeartRateLabel;
    void Start()
    {
        debugMenuLogic = GameObject.Find("Debug Menu").GetComponent<DebugMenuLogic>();
        heartrate = GameObject.Find("HeartRate").GetComponent<Heartrate>();
        zoneLabel = GameObject.Find("Zone Label").GetComponent<TextMeshProUGUI>();
        fatigueValueLabel = GameObject.Find("Fatigue Label").GetComponent<TextMeshProUGUI>();
        hrMaxLabel = GameObject.Find("HrMaxNorm Label").GetComponent<TextMeshProUGUI>();
        normalizedHeartRateLabel = GameObject.Find("Normalized Label").GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        zoneLabel.text = "Zone: " + heartrate.currentZone;
        fatigueValueLabel.text = "Fatigue: " + Mathf.Round(heartrate.fatigueValue * 1000f) / 1000f;
        hrMaxLabel.text = "hrMax Norm: " + heartrate.hrMaxNormalizedHeartRate;
        normalizedHeartRateLabel.text = "Hr Norm: " + heartrate.NormalizedHeartRate;
    }
}
