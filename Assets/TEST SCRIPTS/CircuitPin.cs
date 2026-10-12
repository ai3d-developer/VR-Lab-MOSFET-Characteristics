using UnityEngine;

public class CircuitPin : MonoBehaviour
{
    public string pinName;

    [HideInInspector]
    public BreadboardHole connectedHole;

    [HideInInspector]
    public int connectedNodeID = -1;

    public void ConnectToHole(BreadboardHole hole)
    {
        if (hole == null)
            return;

        connectedHole = hole;
        connectedNodeID = hole.nodeID;

        hole.ConnectPin(this);

        Debug.Log(
            pinName +
            " connected to " +
            hole.holeID +
            " | Node = " +
            hole.nodeID
        );
    }

    public void Disconnect()
    {
        if (connectedHole != null)
        {
            connectedHole.DisconnectPin();
        }

        connectedHole = null;
        connectedNodeID = -1;
    }

    public bool IsConnected()
    {
        return connectedHole != null;
    }
}