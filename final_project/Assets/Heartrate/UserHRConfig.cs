using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This class stores configuration information about the user
public class UserHRConfig : MonoBehaviour
{
    public int hrMax;
    public int age;
    public int fitnessLevel;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private int getHrMax()
    {
        return age == -1 ? 220 - age : -1;
    }
}
