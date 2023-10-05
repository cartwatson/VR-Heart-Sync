using UnityEngine.XR.Interaction.Toolkit;

public class CustomXRGrabInteractable : XRGrabInteractable
{
    protected override void OnSelectEntering(SelectEnterEventArgs args)
    {
        if (!isSelected)
        {
            base.OnSelectEntering(args);
        }
    }

    protected override void OnSelectExiting(SelectExitEventArgs args)
    {
        if (isSelected)
        {
            base.OnSelectExiting(args);
        }
    }
}