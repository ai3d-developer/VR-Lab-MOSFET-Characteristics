using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class MosfetSocketGuide : MonoBehaviour
{
    public GameObject placeHereArrow;

    private XRSocketInteractor socket;

    private void Awake()
    {
        socket = GetComponent<XRSocketInteractor>();
    }

    private void Start()
    {
        // Start-la arrow visible
        if (placeHereArrow != null)
        {
            placeHereArrow.SetActive(true);
        }
    }

    private void Update()
    {
        if (socket == null || placeHereArrow == null)
            return;

        // Socket-la MOSFET successfully placed
        if (socket.hasSelection)
        {
            placeHereArrow.SetActive(false);
        }
        else
        {
            // MOSFET not placed
            placeHereArrow.SetActive(true);
        }
    }
}