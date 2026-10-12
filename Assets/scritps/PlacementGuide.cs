using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class PlacementGuide : MonoBehaviour
{
    public GameObject placeHereArrow;

    private XRGrabInteractable grab;

    private void Awake()
    {
        grab = GetComponent<XRGrabInteractable>();

        if (placeHereArrow != null)
            placeHereArrow.SetActive(false);
    }

    private void OnEnable()
    {
        grab.selectEntered.AddListener(OnSelectEntered);
        grab.selectExited.AddListener(OnSelectExited);
    }

    private void OnDisable()
    {
        grab.selectEntered.RemoveListener(OnSelectEntered);
        grab.selectExited.RemoveListener(OnSelectExited);
    }

    private void OnSelectEntered(SelectEnterEventArgs args)
    {
        // Socket select panna arrow show panna koodathu
        if (args.interactorObject is XRSocketInteractor)
        {
            placeHereArrow.SetActive(false);
            return;
        }

        // Hand / Grab pannumbodhu mattum show
        placeHereArrow.SetActive(true);
    }

    private void OnSelectExited(SelectExitEventArgs args)
    {
        // Hand-la irundhu drop pannumbodhu hide
        placeHereArrow.SetActive(false);
    }
}