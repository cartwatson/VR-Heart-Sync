using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadialMenu : MonoBehaviour
{
    [Header("Scene")]
    public Transform selectionTransform = null;
    public Transform cursorTransform = null;
    public InputManager inputManager;

    [Header("Default Sprites")]
    public Sprite emptySprite;
    public Sprite blueSprite;
    public Sprite redSprite;
    public Sprite greenSprite;
    public Sprite yellowSprite;
    // fill in below with appropriate number of sprites
    // sprites can be put in the /Assets/RadialMenu/Textures
    // public Sprite descriptive_name;
    public Sprite OldRecordPlayer;
    public Sprite RecordPlayer70s;
    public Sprite LeatherChair;
    public Sprite FilingCabinet;

    [Header("Custom Sprites")]
    public Sprite waterBottleBlue;
    public Sprite waterBottleRed;
    public Sprite dumbell;
    public Sprite kettleBell;
    public Sprite barWeight;
    public Sprite plateWeight;
    public Sprite yogaBall;
    public Sprite yogaMat;
    public Sprite electricScale;

    // [Header("Radial Sections")]
    private SpriteRenderer top = null;
    private SpriteRenderer right = null;
    private SpriteRenderer bottom = null;
    private SpriteRenderer left = null;

    private List<SpriteRenderer> renderers = new List<SpriteRenderer>();

    private readonly float degreeIncrement = 90.0f;
    private float rotation = 0.0f;

    [HideInInspector]
    public List<GameObject> menuOptions;



    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("RadialMenu Start");
        // get renderers
        top    = GameObject.Find("TopRenderer").GetComponent<SpriteRenderer>();
        right  = GameObject.Find("RightRenderer").GetComponent<SpriteRenderer>();
        bottom = GameObject.Find("BottomRenderer").GetComponent<SpriteRenderer>();
        left   = GameObject.Find("LeftRenderer").GetComponent<SpriteRenderer>();

        // add to list of renderers
        renderers.Add(top);
        renderers.Add(right);
        renderers.Add(bottom);
        renderers.Add(left);
    }

    public void Show(bool value)
    {
        // set the correct sprites
        SetSprites();

        // determine visibility
        gameObject.SetActive(value);
    }

    // Update is called once per frame
    void Update()
    {
        // get quadrant that joystick is over
        Vector2 direction = Vector2.zero + inputManager.GetLeftJoystickValue();
        rotation = GetDegree(direction);

        // move cursor and highlight appropriate section
        SetCursorPosition();
        SetSelectedSection();
    }

    private float GetDegree(Vector2 direction)
    {
        float value = Mathf.Atan2(direction.x, direction.y);
        value *= Mathf.Rad2Deg;

        // convert from -180 through 180 to 0 to 360
        if (value < 0)
            value += 360;

        return value;
    }

    private void SetCursorPosition()
    {
        cursorTransform.localPosition = inputManager.GetLeftJoystickValue();
    }

    private void SetSprites()
    {
        Debug.Log("Candidate Count: " + menuOptions.Count);
        for (int i = 0; i < menuOptions.Count; i++)
        {
            // get objects to iterate over
            GameObject candidate = menuOptions[i];
            // Sprite sprite = renderers[i].sprite;
            Sprite newSprite;

            // assign sprite
            switch (candidate.name)
            {
                // TODO: fill this out // example below
                // case "<Object name>":
                //     newSprite = "<sprite name>"
                //     break;

                case "Old Record Player":
                    newSprite = OldRecordPlayer;
                    break;

                case "70s record player":
                    newSprite = RecordPlayer70s;
                    break;

                case "Leather_Chair":
                    newSprite = LeatherChair;
                    break;

                case "Rolled Up Mat":
                    newSprite = LeatherChair;
                    break;

                case "Waterbottle B":
                    newSprite = waterBottleBlue;
                    break;

                case "Waterbottle R":
                    newSprite = waterBottleRed;
                    break;

                case "Kettle Bell":
                    newSprite = kettleBell;
                    break;

                case "Dumbell":
                    newSprite = dumbell;
                    break;

                case "Plate":
                    newSprite = plateWeight;
                    break;

                case "RolledUpMat":
                    newSprite = yogaMat;
                    break;

                case "Ball":
                    newSprite = yogaBall;
                    break;

                case "Electric Scale":
                    newSprite = electricScale;
                    break;

                case "Bar Weight":
                    newSprite = barWeight;
                    break;

                default:
                    // assign default sprite based on highlight color
                    switch (i)
                    {
                        case 0:
                            newSprite = blueSprite;
                            break;
                        case 1:
                            newSprite = redSprite;
                            break;
                        case 2:
                            newSprite = greenSprite;
                            break;
                        case 3:
                            newSprite = yellowSprite;
                            break;
                        default:
                            newSprite = blueSprite;
                            break;
                    }
                    break;
            }

            // assign sprite
            renderers[i].sprite = newSprite;
        }
    }

    public void ClearSprites()
    {
        for (int i = 0; i < 4; i++)
        {
            renderers[i].sprite = null;
        }
        menuOptions.Clear();
    }

    // Section Highlighter --- functions to highlight appropriate section
    private void SetSelectedSection()
    {
        float snappedRotation = GetNearestIncrement() * degreeIncrement;
        selectionTransform.localEulerAngles = new Vector3(0, 0, -snappedRotation);
    }

    private int GetNearestIncrement()
    {
        return Mathf.RoundToInt(rotation / degreeIncrement);
    }

    public int GetIndex()
    {
        int index = GetNearestIncrement();
        // sometimes index is rounded to 4, if that's the case it's actually closest to zero
        return index == 4 ? 0 : index;
    }
}
