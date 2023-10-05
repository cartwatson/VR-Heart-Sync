using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using Unity.XR.CoreUtils;

public class Navigation : MonoBehaviour
{
    public float speed = 10f;
    public float jumpPower = 0.5f;
    public float turnAngle = 1f;

    private XROrigin rig;
    private InputManager inputManager;
    private Vector2 inputAxis;
    private CharacterController characterController;
    private Vector3 playerVelocity;
    private float lateralSpeed = 0;
    private float fallingSpeed = -9.81f;
    // Start is called before the first frame update
    void Start()
    {
        inputManager = GameObject.Find("InputManager").GetComponent<InputManager>();
        rig = GameObject.Find("XR Origin").GetComponent<XROrigin>();
        characterController = GetComponent<CharacterController>();

    }

    // Update is called once per frame
    void Update()
    {
    }

    void FixedUpdate()
    {
        Quaternion headYaw = Quaternion.Euler(0, rig.Camera.transform.eulerAngles.y, 0);
        Vector2 rInputAxis = inputManager.GetRightJoystickValue();
        Vector3 direction = headYaw * new Vector3(rInputAxis.x, 0, rInputAxis.y);

        if (inputManager.GetLeftGrip())
        {
            lateralSpeed = speed * 2;
        }
        else
        {
            lateralSpeed = speed;
        }

        if (characterController.isGrounded && inputManager.GetLeftPrimaryPress())
        {
            playerVelocity.y = jumpPower;
        }


        //Turn right if right secondary is pressed and visa-versa
        if (inputManager.GetRightSecondary())
        {
            transform.Rotate(Vector3.up, Time.fixedDeltaTime * turnAngle);
        }
        if (inputManager.GetLeftSecondary())
        {
            transform.Rotate(Vector3.up, Time.fixedDeltaTime * -turnAngle);
        }

        playerVelocity.y += fallingSpeed;


        characterController.Move(direction * Time.fixedDeltaTime * lateralSpeed);
        characterController.Move(playerVelocity * Time.fixedDeltaTime);
    }
}
