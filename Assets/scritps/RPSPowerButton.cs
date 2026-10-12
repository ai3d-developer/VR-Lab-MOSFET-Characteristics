using UnityEngine;

public class RPSPowerButton : MonoBehaviour
{
    public RPSController rpsController;

    private void OnTriggerEnter(Collider other)
    {
        if (rpsController == null) return;

        // OFF -> ON
        if (!rpsController.isOn)
        {
            rpsController.TogglePower();
            Debug.Log("RPS TURNED ON");
        }
    }
}