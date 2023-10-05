using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using Unity.XR.CoreUtils;
using UnityEngine.XR.OpenXR.Input;
using UnityEngine.XR.Interaction.Toolkit;

public class ExtendedXRDirectInteractor : XRDirectInteractor
{
    public void ForceSelect(XRGrabInteractable interactable, SelectEnterEventArgs args)
    {
        if (interactable != null)
        {
            base.OnSelectEntering(args);
        }
    }

        public void ForceDeselect(XRGrabInteractable interactable, SelectExitEventArgs args)
    {
        if (interactable != null)
        {
            base.OnSelectExiting(args);
        }
    }

        public void Attach(XRGrabInteractable interactable, SelectEnterEventArgs args)
    {
        // Attach the object to the interactor
        interactable.transform.SetParent(attachTransform);
        interactable.transform.localPosition = Vector3.zero;
        interactable.transform.localRotation = Quaternion.identity;

        // Set the selected interactable
        interactable.selectEntered.Invoke(args);
    }

    public void Detach(XRGrabInteractable interactable, SelectExitEventArgs args)
    {
        // Detach the object from the interactor
        interactable.transform.SetParent(null);

        // Set the deselected interactable
        interactable.selectExited.Invoke(args);
    }
}