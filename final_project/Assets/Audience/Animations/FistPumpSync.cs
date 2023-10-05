using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FistPumpSync : MonoBehaviour
{
    public Animator animator;
    public string animationStateName = "FistPump";
    public float baseAnimationBPM = 68f;

    private Heartrate heartRate;
    private GameObject camera;

    private Vector3 initialRotation;

    void Start()
    {
        heartRate = GameObject.Find("HeartRate").GetComponent<Heartrate>();
        camera = GameObject.Find("Main Camera");
        initialRotation = transform.localEulerAngles;

    }

    private void Update()
    {
        float currentHeartRate = heartRate.CurrentHeartRate;
        SyncAnimationToHeartRate(currentHeartRate);

        // Look at player
        transform.LookAt(camera.transform);
        transform.localEulerAngles = new Vector3(initialRotation.x, transform.localEulerAngles.y, initialRotation.z);
    }

    private void SyncAnimationToHeartRate(float heartRate)
    {
        float speed = heartRate / baseAnimationBPM;

        // Set the speed of the animation state to match
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        if (stateInfo.IsName(animationStateName))
        {
            animator.SetFloat("FistPumpSpeed", speed);
            animator.speed = speed;
        }
    }
}