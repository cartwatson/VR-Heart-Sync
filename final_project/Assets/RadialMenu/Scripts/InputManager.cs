using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using Unity.XR.CoreUtils;
using UnityEngine.XR.OpenXR.Input;

public class InputManager : MonoBehaviour
{
    [Header("Controllers")]
    public XRNode rightController;
    public XRNode leftController;
    private InputDevice rController;
    private InputDevice lController;

    private bool leftPrimaryLastFrame = false;
    private bool rightPrimaryLastFrame = false;

    private void Start()
    {
        InputDevice rController = InputDevices.GetDeviceAtXRNode(rightController);
        InputDevice lController = InputDevices.GetDeviceAtXRNode(leftController);
    }

    public Vector2 GetRightJoystickValue()
    {
        InputDevice rController = InputDevices.GetDeviceAtXRNode(rightController);
        rController.TryGetFeatureValue(CommonUsages.primary2DAxis, out Vector2 joystickDirection);
        return joystickDirection;
    }

    public Vector2 GetLeftJoystickValue()
    {
        InputDevice lController = InputDevices.GetDeviceAtXRNode(leftController);
        lController.TryGetFeatureValue(CommonUsages.primary2DAxis, out Vector2 joystickDirection);
        return joystickDirection;
    }

    public bool GetRightGrip()
    {
        InputDevice rController = InputDevices.GetDeviceAtXRNode(rightController);
        rController.TryGetFeatureValue(CommonUsages.gripButton, out bool grip);
        return grip;
    }

    public bool GetRightTrigger()
    {
        InputDevice rController = InputDevices.GetDeviceAtXRNode(rightController);
        rController.TryGetFeatureValue(CommonUsages.triggerButton, out bool trigger);
        return trigger;
    }

    public Vector3 GetRightPosition()
    {
        InputDevice rController = InputDevices.GetDeviceAtXRNode(rightController);   
        rController.TryGetFeatureValue(CommonUsages.devicePosition, out Vector3 position);
        return position;
    }
            
    public Quaternion GetRightOrientation()
    {
        InputDevice rController = InputDevices.GetDeviceAtXRNode(rightController);   
        rController.TryGetFeatureValue(CommonUsages.deviceRotation, out Quaternion rotation);
        return rotation;
    }
        
    public bool GetRightPrimary() // A
    {
        InputDevice rController = InputDevices.GetDeviceAtXRNode(rightController);   
        rController.TryGetFeatureValue(CommonUsages.primaryButton, out bool button);
        return button;
    }

    public bool GetRightPrimaryRelease() // A
    {
        InputDevice rController = InputDevices.GetDeviceAtXRNode(rightController);
        rController.TryGetFeatureValue(CommonUsages.primaryButton, out bool button);

        if (rightPrimaryLastFrame && !button)
        {
            rightPrimaryLastFrame = button;
            return true;
        }

        rightPrimaryLastFrame = button;
        return false;
    }

    public bool GetRightSecondary() // B
    {
        InputDevice rController = InputDevices.GetDeviceAtXRNode(rightController);   
        rController.TryGetFeatureValue(CommonUsages.secondaryButton, out bool button);
        return button;
    }

    public bool GetLeftSecondary() // B
    {
        InputDevice lController = InputDevices.GetDeviceAtXRNode(leftController);
        lController.TryGetFeatureValue(CommonUsages.secondaryButton, out bool button);
        return button;
    }

    public bool GetLeftPrimaryPress() // X
    {
        InputDevice lController = InputDevices.GetDeviceAtXRNode(leftController);   
        lController.TryGetFeatureValue(CommonUsages.primaryButton, out bool button);

        if (!leftPrimaryLastFrame && button)
        {
            leftPrimaryLastFrame = button;
            return true;
        }

        leftPrimaryLastFrame = button;
        return false;
    }

    public bool GetLeftGrip()
    {
        InputDevice lController = InputDevices.GetDeviceAtXRNode(leftController);
        lController.TryGetFeatureValue(CommonUsages.gripButton, out bool grip);
        return grip;
    }
}