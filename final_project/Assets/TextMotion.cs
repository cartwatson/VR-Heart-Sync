using UnityEngine;
using TMPro;

public class TextMotion : MonoBehaviour
{
    [Header("Scene")]
    public InputManager inputManager;
    
    [Header("Text Movement Modifiers")]
    public float floatingAmplitude = 0.1f;
    public float floatingSpeed = 0.3f; // TODO: implement
    
    private Vector3 startingPosition;
    private TMP_Text textMeshPro;
    private string[] textOptionsLower = { "Breathe in", "Breathe out" };
    private string[] textOptionsHigher = { "Get Moving!", "Breathe, stay ready to start moving!" };
    private bool upper = false;
    private int currentOptionIndex = 0;
    private float timeSinceLastUpdate = 0f;
    private float floatingTimer = 0f;
    private bool isRising = true;

    void Start()
    {
        startingPosition = transform.position;
        textMeshPro = GetComponent<TMP_Text>();
        textMeshPro.text = textOptionsLower[currentOptionIndex];
    }

    void Update()
    {
        // method variables
        float updateTime;
        string[] textOption;

        // keep track of time
        floatingTimer += Time.deltaTime;
        timeSinceLastUpdate += Time.deltaTime;

        // check for upper/lower change - toggle on button press
        if (inputManager.GetLeftPrimaryPress())
        {
            // toggle upper or lower
            upper = !upper;
            
            // reset words
            currentOptionIndex = 0;

            if (upper)
                textOption = textOptionsHigher;
            else
                textOption = textOptionsLower;

            textMeshPro.text = textOption[currentOptionIndex];
            timeSinceLastUpdate = 0f;
        }


        // determine if in upper or lower state
        if (upper)
        {
            textOption = textOptionsHigher;
            // 30secs of moving, 10 of rest
            if (currentOptionIndex == 0)
                updateTime = 30.0f;
            else 
                updateTime = 10.0f;
        }
        else
        {
            textOption = textOptionsLower;
            updateTime = 6.0f;
        }

        // update text if time has elapsed
        if (timeSinceLastUpdate >= updateTime)
        {
            currentOptionIndex++;
            if (currentOptionIndex >= textOption.Length)
            {
                currentOptionIndex = 0;
            }
            textMeshPro.text = textOption[currentOptionIndex];
            timeSinceLastUpdate = 0f;
        }

        // update floating position
        float floatingOffset = Mathf.PingPong(floatingTimer / updateTime, 1f);
        if (!isRising)
        {
            floatingOffset = 1f - floatingOffset;
        }
        floatingOffset = floatingOffset * floatingAmplitude;
        Vector3 floatingVector = new Vector3(0f, floatingOffset, 0f);
        transform.position = startingPosition + floatingVector;

        // check if the text has reached the end of the rising or lowering phase
        if (floatingTimer >= updateTime)
        {
            isRising = !isRising;
            floatingTimer = 0f;
        }
    }
}
