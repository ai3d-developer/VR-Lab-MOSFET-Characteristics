using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class ComponentSnap : MonoBehaviour
{
    [Header("TWO PINS")]
    public Transform pin1;
    public Transform pin2;

    [Header("BREADBOARD")]
    public Transform snapPointParent;

    [Header("RED PREVIEW")]
    public float previewDistance = 0.08f;

    [Header("SNAP HEIGHT")]
    public float surfaceOffset = 0f;

    private XRGrabInteractable grab;

    private BreadboardHole previewHole1;
    private BreadboardHole previewHole2;

    private void Awake()
    {
        grab = GetComponent<XRGrabInteractable>();

        if (grab == null)
        {
            grab = GetComponentInChildren<XRGrabInteractable>();
        }

        if (grab == null)
        {
            grab = GetComponentInParent<XRGrabInteractable>();
        }

        if (grab == null)
            Debug.LogError("XRGrabInteractable NOT FOUND!");

        if (pin1 == null)
            Debug.LogError("PIN1 NOT ASSIGNED!");

        if (pin2 == null)
            Debug.LogError("PIN2 NOT ASSIGNED!");

        if (snapPointParent == null)
            Debug.LogError("SNAP POINT NOT ASSIGNED!");
    }

    private void Update()
    {
        if (grab == null)
            return;

        if (grab.isSelected)
        {
            FindAndHighlightTwoHoles();
        }
        else
        {
            ClearPreview();
        }
    }

    // =========================================================
    // FIND TWO VALID HOLES
    // =========================================================

    private void FindAndHighlightTwoHoles()
    {
        if (pin1 == null || pin2 == null || snapPointParent == null)
            return;

        BreadboardHole[] holes =
            snapPointParent.GetComponentsInChildren<BreadboardHole>(true);

        BreadboardHole bestHole1 = null;
        BreadboardHole bestHole2 = null;

        float bestScore = float.MaxValue;

        // Check every possible pair of holes
        for (int i = 0; i < holes.Length; i++)
        {
            BreadboardHole hole1 = holes[i];

            if (hole1.IsOccupied())
                continue;

            float distance1 =
                Vector3.Distance(
                    pin1.position,
                    hole1.transform.position
                );

            if (distance1 > previewDistance)
                continue;

            for (int j = 0; j < holes.Length; j++)
            {
                BreadboardHole hole2 = holes[j];

                if (hole2 == hole1)
                    continue;

                if (hole2.IsOccupied())
                    continue;

                float distance2 =
                    Vector3.Distance(
                        pin2.position,
                        hole2.transform.position
                    );

                if (distance2 > previewDistance)
                    continue;

                // Total distance error
                float score = distance1 + distance2;

                if (score < bestScore)
                {
                    bestScore = score;
                    bestHole1 = hole1;
                    bestHole2 = hole2;
                }
            }
        }

        // Update red indication
        SetPreview(
            bestHole1,
            bestHole2
        );
    }

    // =========================================================
    // RED INDICATION
    // =========================================================

    private void SetPreview(
        BreadboardHole newHole1,
        BreadboardHole newHole2)
    {
        // Remove old Hole 1 highlight
        if (previewHole1 != null &&
            previewHole1 != newHole1 &&
            previewHole1 != newHole2)
        {
            previewHole1.HideRed();
        }

        // Remove old Hole 2 highlight
        if (previewHole2 != null &&
            previewHole2 != newHole1 &&
            previewHole2 != newHole2)
        {
            previewHole2.HideRed();
        }

        previewHole1 = newHole1;
        previewHole2 = newHole2;

        // Show both holes red
        if (previewHole1 != null)
            previewHole1.ShowRed();

        if (previewHole2 != null)
            previewHole2.ShowRed();
    }

    private void ClearPreview()
    {
        if (previewHole1 != null)
        {
            previewHole1.HideRed();
            previewHole1 = null;
        }

        if (previewHole2 != null)
        {
            previewHole2.HideRed();
            previewHole2 = null;
        }
    }

    // =========================================================
    // RELEASE
    // =========================================================

    private void OnEnable()
    {
        if (grab != null)
        {
            grab.selectExited.AddListener(OnReleased);
        }
    }

    private void OnDisable()
    {
        if (grab != null)
        {
            grab.selectExited.RemoveListener(OnReleased);
        }

        ClearPreview();
    }

    private void OnReleased(SelectExitEventArgs args)
    {
        SnapTwoPins();
    }

    // =========================================================
    // SNAP TWO PINS
    // =========================================================

    private void SnapTwoPins()
    {
        if (previewHole1 == null || previewHole2 == null)
        {
            Debug.Log("No valid two-hole combination found.");
            return;
        }

        BreadboardHole hole1 = previewHole1;
        BreadboardHole hole2 = previewHole2;

        // -----------------------------------------------------
        // PIN 1 TARGET
        // -----------------------------------------------------

        Vector3 targetPin1 =
            hole1.transform.position +
            hole1.transform.up * surfaceOffset;

        Vector3 move =
            targetPin1 - pin1.position;

        // Move complete resistor
        transform.position += move;

        Physics.SyncTransforms();

        // -----------------------------------------------------
        // REMOVE RED
        // -----------------------------------------------------

        ClearPreview();

        // -----------------------------------------------------
        // ELECTRICAL CONNECTION
        // -----------------------------------------------------

        CircuitPin circuitPin1 =
            pin1.GetComponent<CircuitPin>();

        CircuitPin circuitPin2 =
            pin2.GetComponent<CircuitPin>();

        if (circuitPin1 != null)
        {
            circuitPin1.ConnectToHole(hole1);
        }

        if (circuitPin2 != null)
        {
            circuitPin2.ConnectToHole(hole2);
        }

        // -----------------------------------------------------
        // DEBUG
        // -----------------------------------------------------

        Debug.Log(
            "================================="
        );

        Debug.Log(
            "RESISTOR TWO-PIN SNAP SUCCESS"
        );

        Debug.Log(
            "PIN1 -> " +
            hole1.holeID +
            " | NODE = " +
            hole1.nodeID
        );

        Debug.Log(
            "PIN2 -> " +
            hole2.holeID +
            " | NODE = " +
            hole2.nodeID
        );

        Debug.Log(
            "================================="
        );
    }
}