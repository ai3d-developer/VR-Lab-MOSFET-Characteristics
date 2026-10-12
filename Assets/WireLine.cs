using UnityEngine;

public class WireLine : MonoBehaviour
{
    public Transform startPoint;
    public Transform endPoint;
    public Color wireColor = Color.black;

    private LineRenderer line;

    void Awake()
    {
        line = GetComponent<LineRenderer>();

        line.positionCount = 2;
        line.startColor = wireColor;
        line.endColor = wireColor;
    }

    void Update()
    {
        line.SetPosition(0, startPoint.position);
        line.SetPosition(1, endPoint.position);

        // Color continuously set
        line.startColor = wireColor;
        line.endColor = wireColor;
    }
}