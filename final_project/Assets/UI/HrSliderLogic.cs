using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HrSliderLogic : MonoBehaviour
{
    // Reference to the slider component
    public Slider slider;
    private TextMeshProUGUI currentRateLabel;

    // Reference to the target object that uses the float value
    public Heartrate heartRateData;

    // The float value that will be controlled by the slider
    private int sliderValue;

    // The minimum and maximum values for the slider
    public int minValue = 60;
    public int maxValue = 130;

    // Start is called before the first frame update
    void Start()
    {
        slider = GetComponent<Slider>();
        // Set the minimum and maximum values for the slider
        slider.minValue = minValue;
        slider.maxValue = maxValue;

        currentRateLabel = GameObject.Find("Current Rate Label").GetComponent<TextMeshProUGUI>();

        // Set the initial value of the slider to the current value of the float
        slider.value = minValue;
        slider.value = (int) sliderValue;

        // Add a listener to the slider's OnValueChanged event
        slider.onValueChanged.AddListener(OnSliderValueChanged);
    }

    // Update is called once per frame
    void Update()
    {
        // Update the value of the float in the target object with the current value of the slider
        heartRateData.CurrentHeartRate = sliderValue;
        currentRateLabel.text = "" + sliderValue;
    }

    // This method is called when the slider's value is changed
    private void OnSliderValueChanged(float value)
    {
        // Clamp the value within the minimum and maximum range
        sliderValue = (int) Mathf.Clamp(value, minValue, maxValue);
    }
}