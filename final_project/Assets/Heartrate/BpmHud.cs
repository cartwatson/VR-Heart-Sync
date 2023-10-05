using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BpmHud : MonoBehaviour
{
    // Start is called before the first frame update
    public Heartrate heartrate;
    private TextMeshProUGUI textMeshPro;
    void Start()
    {
        heartrate = GameObject.Find("HeartRate").GetComponent<Heartrate>();
        textMeshPro = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        textMeshPro.text = "HR: " + heartrate.CurrentHeartRate;
    }
}
