using UnityEngine;
using TMPro;

public class CircuitController : MonoBehaviour
{
    [Header("RPS 1")]
    public RPSController rps1;
    public TMP_Text voltmeter1Text;

    [Header("RPS 2")]
    public RPSController rps2;
    public TMP_Text voltmeter2Text;

    [Header("Resistance")]
    public float resistance1 = 220f;
    public float resistance2 = 3300f;

    [Header("Ammeter")]
    public TMP_Text ammeterText;

    void Update()
    {
        // Get RPS 1 voltage
        float voltage1 = 0f;

        if (rps1 != null && rps1.isOn)
        {
            voltage1 = rps1.voltage;
        }

        // Get RPS 2 voltage
        float voltage2 = 0f;

        if (rps2 != null && rps2.isOn)
        {
            voltage2 = rps2.voltage;
        }

        // =====================================
        // VOLTMETER 1
        // =====================================

        if (voltmeter1Text != null)
        {
            voltmeter1Text.text = voltage1.ToString("F2") + " V";
        }

        // =====================================
        // VOLTMETER 2
        // =====================================

        if (voltmeter2Text != null)
        {
            voltmeter2Text.text = voltage2.ToString("F2") + " V";
        }

        // =====================================
        // TOTAL RESISTANCE
        // Series connection:
        // R = R1 + R2
        // =====================================

        float totalResistance = resistance1 + resistance2;

        // =====================================
        // TOTAL VOLTAGE
        // =====================================

        float totalVoltage = voltage1 + voltage2;

        // =====================================
        // CURRENT
        // I = V / R
        // =====================================

        float current = 0f;

        if (totalResistance > 0f)
        {
            current = totalVoltage / totalResistance;
        }

        // =====================================
        // UPDATE RPS CURRENT DISPLAYS
        // =====================================

        if (rps1 != null)
        {
            rps1.SetCurrent(current);
        }

        if (rps2 != null)
        {
            rps2.SetCurrent(current);
        }

        // =====================================
        // AMMETER
        // Convert Ampere to milliampere
        // =====================================

        if (ammeterText != null)
        {
            float currentMilliAmp = current * 1000f;

            ammeterText.text =
                currentMilliAmp.ToString("F2") + " mA";
        }
    }
}