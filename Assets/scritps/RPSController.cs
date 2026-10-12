using UnityEngine;
using TMPro;

public class RPSController : MonoBehaviour
{
    [Header("Power")]
    public bool isOn = false;

    [Header("Voltage")]
    [Range(0f, 30f)]
    public float voltage = 0f;

    public float maxVoltage = 30f;

    [Header("RPS Display")]
    public TMP_Text voltageDisplay;
    public TMP_Text currentDisplay;

    private float current = 0f;

    void Start()
    {
        isOn = false;
        voltage = 0f;
        current = 0f;

        UpdateDisplay();
    }

    public void TogglePower()
    {
        isOn = !isOn;

        // OFF -> voltage/current zero
        if (!isOn)
        {
            voltage = 0f;
            current = 0f;
        }

        UpdateDisplay();
    }

    public void SetPower(bool power)
    {
        isOn = power;

        if (!isOn)
        {
            voltage = 0f;
            current = 0f;
        }

        UpdateDisplay();
    }

    public void SetVoltage(float newVoltage)
    {
        if (!isOn)
            return;

        voltage = Mathf.Clamp(newVoltage, 0f, maxVoltage);
        UpdateDisplay();
    }

    public void SetCurrent(float newCurrent)
    {
        if (!isOn)
        {
            current = 0f;
        }
        else
        {
            current = Mathf.Max(0f, newCurrent);
        }

        UpdateDisplay();
    }

    public void UpdateDisplay()
    {
        if (!isOn)
        {
            if (voltageDisplay != null)
                voltageDisplay.text = "OFF";

            if (currentDisplay != null)
                currentDisplay.text = "OFF";

            return;
        }

        if (voltageDisplay != null)
            voltageDisplay.text = voltage.ToString("F2") + " V";

        if (currentDisplay != null)
            currentDisplay.text = (current * 1000f).ToString("F2") + " mA";
    }
}