using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class DebugMenuLogic : MonoBehaviour
{
    // Reference to the target object that uses the float value
    public Heartrate heartRateData;

    // This public value will tell other elements whether or not we are in debug mode
    public bool isDebug;

    // The minimum and maximum values for the slider
    public int hrMinValue = 50;
    public int hrMaxValue = 200;

    //Reference Toggle Button
    private Toggle debugToggle;

    // Reference to the slider component
    public Slider hrSlider;
    private TextMeshProUGUI currentRateLabel;

    public Slider fatigueSlider;
    private TextMeshProUGUI currentFatigueLabel;

    // The  values that will be controlled by the sliders
    private int hrSliderValue;
    private float fatigueSliderValue;

    private GameObject debugHud;

    // Start is called before the first frame update
    void Start()
    {
        heartRateData = GameObject.Find("HeartRate").GetComponent<Heartrate>();
        debugToggle = GameObject.Find("Debug Toggle").GetComponent<Toggle>();
        debugToggle.isOn = false;

        // Initilize Hr slider and set its minimum and maximum values
        hrSlider = GameObject.Find("Heart Rate Slider").GetComponent<Slider>();
        hrSlider.minValue = hrMinValue;
        hrSlider.maxValue = hrMaxValue;

        // Initilize Hr slider and set its minimum and maximum values
        fatigueSlider = GameObject.Find("Fatigue Slider").GetComponent<Slider>();
        fatigueSlider.minValue = 0f;
        fatigueSlider.maxValue = 1f;


        currentRateLabel = GameObject.Find("Current Rate Label").GetComponent<TextMeshProUGUI>();
        currentFatigueLabel = GameObject.Find("Current Fatigue Label").GetComponent<TextMeshProUGUI>();

        debugHud = GameObject.Find("Debug HUD");

        // Set the initial value of the slider to the current value of the float
        hrSlider.value = 80;

        fatigueSliderValue = 0;

        // Add a listener to the slider's OnValueChanged event
        debugToggle.onValueChanged.AddListener(OnToggleChanged);
        hrSlider.onValueChanged.AddListener(OnHrSliderValueChanged);
        fatigueSlider.onValueChanged.AddListener(OnFatigueSliderValueChanged);

    }

    // Update is called once per frame
    void Update()
    {
        if (isDebug)
        {
            Debug.Log("DebugMenuLogic thinks isDebug is true!");
            heartRateData.CurrentHeartRate = hrSliderValue;
            heartRateData.NormalizeHeartrate();
            currentRateLabel.text = "" + hrSliderValue;

            heartRateData.fatigueValue = fatigueSliderValue;
            //Round fatigue value to nearest thousandth
            currentFatigueLabel.text = "" + Mathf.Round(fatigueSliderValue * 1000f) / 1000f;
        }
        // Update the value of the float in the target object with the current value of the slider
    }
    private void OnToggleChanged(bool value)
    {
        isDebug = value;
        hrSlider.interactable = value;
        fatigueSlider.interactable = value;
        debugHud.SetActive(value);
    }

    // This method is called when the slider's value is changed
    private void OnHrSliderValueChanged(float value)
    {
        // Clamp the value within the minimum and maximum range
        hrSliderValue = (int) Mathf.Clamp(value, hrMinValue, hrMaxValue);
    }
    private void OnFatigueSliderValueChanged(float value)
    {
        // Clamp the value within the minimum and maximum range
        fatigueSliderValue = Mathf.Clamp(value, 0, 1);
    }

}