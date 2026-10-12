using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class RPSKnob : MonoBehaviour
{
    public RPSController rpsController;

    [Header("Settings")]
    public float maxVoltage = 30f;

    [Tooltip("Smaller = slower voltage change")]
    public float sensitivity = 0.05f;

    private XRGrabInteractable grabInteractable;
    private bool isGrabbed = false;
    private float lastRotationY;

    public float Voltage { get; internal set; }

    void Awake()
    {
        grabInteractable =
            GetComponent<XRGrabInteractable>();

        if (grabInteractable == null)
        {
            Debug.LogError("XR Grab Interactable missing!");
            return;
        }

        // Knob fixed
        grabInteractable.trackPosition = false;
        grabInteractable.trackRotation = false;
        grabInteractable.throwOnDetach = false;

        grabInteractable.selectEntered.AddListener(OnGrab);
        grabInteractable.selectExited.AddListener(OnRelease);
    }

    void OnGrab(SelectEnterEventArgs args)
    {
        isGrabbed = true;

        // Hand current rotation
        lastRotationY =
            args.interactorObject.transform
            .eulerAngles.y;

        Debug.Log("KNOB GRABBED");
    }

    void OnRelease(SelectExitEventArgs args)
    {
        isGrabbed = false;
        Debug.Log("KNOB RELEASED");
    }

    void Update()
    {
        if (!isGrabbed) return;

        if (rpsController == null) return;

        if (!rpsController.isOn) return;

        if (grabInteractable.firstInteractorSelecting == null)
            return;

        Transform hand =
            grabInteractable
            .firstInteractorSelecting
            .transform;

        float currentRotationY =
            hand.eulerAngles.y;

        float rotationDelta =
            Mathf.DeltaAngle(
                lastRotationY,
                currentRotationY
            );

        // Rotation -> Voltage
        float voltageChange =
            rotationDelta * sensitivity;

        float newVoltage =
            Mathf.Clamp(
                rpsController.voltage + voltageChange,
                0f,
                maxVoltage
            );

        rpsController.SetVoltage(newVoltage);

        // Update reference every frame
        lastRotationY = currentRotationY;
    }
}