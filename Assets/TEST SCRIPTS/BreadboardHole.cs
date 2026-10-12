using UnityEngine;

public class BreadboardHole : MonoBehaviour
{
    public string holeID;
    public int nodeID;

    [HideInInspector]
    public CircuitPin connectedPin;

    private GameObject redIndicator;

    public bool IsOccupied()
    {
        return connectedPin != null;
    }

    public void ConnectPin(CircuitPin pin)
    {
        connectedPin = pin;
    }

    public void DisconnectPin()
    {
        connectedPin = null;
    }

    private void Start()
    {
        CreateRedIndicator();
        HideRed();
    }

    private void CreateRedIndicator()
    {
        if (redIndicator != null)
            return;

        redIndicator = GameObject.CreatePrimitive(
            PrimitiveType.Sphere
        );

        redIndicator.name = "RED_PREVIEW";

        redIndicator.transform.SetParent(transform);

        redIndicator.transform.localPosition =
            new Vector3(0f, 0.001f, 0f);

        redIndicator.transform.localRotation =
            Quaternion.identity;

        redIndicator.transform.localScale =
            Vector3.one * 0.004f;

        Collider col =
            redIndicator.GetComponent<Collider>();

        if (col != null)
            Destroy(col);

        Renderer renderer =
            redIndicator.GetComponent<Renderer>();

        if (renderer != null)
        {
            renderer.material.color = Color.red;
        }
    }

    public void ShowRed()
    {
        if (redIndicator == null)
            CreateRedIndicator();

        redIndicator.SetActive(true);
    }

    public void HideRed()
    {
        if (redIndicator != null)
            redIndicator.SetActive(false);
    }
}