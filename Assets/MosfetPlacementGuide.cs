using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class MosfetPlacementGuide : MonoBehaviour
{
    public GameObject placeHereArrow;

    private XRGrabInteractable grabInteractable;

    private void Awake()
    {
        grabInteractable = GetComponent<XRGrabInteractable>();

        // Initially hide
        placeHereArrow.SetActive(false);
    }

    private void OnEnable()
    {
        grabInteractable.selectEntered.AddListener(OnGrabbed);
        grabInteractable.selectExited.AddListener(OnReleased);
    }

    private void OnDisable()
    {
        grabInteractable.selectEntered.RemoveListener(OnGrabbed);
        grabInteractable.selectExited.RemoveListener(OnReleased);
    }

    private void OnGrabbed(SelectEnterEventArgs args)
    {
        GuideManager.Instance.ShowOnly(placeHereArrow);
    }

    private void OnReleased(SelectExitEventArgs args)
    {
        GuideManager.Instance.HideAllGuides();
    }
}