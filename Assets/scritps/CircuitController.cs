
using UnityEngine;
using TMPro;
using System.Globalization;

public class CircuitController : MonoBehaviour
{
    [Header("RPS 1 - Gate Source Voltage")]
    public RPSController rps1;
    public TMP_Text voltmeter1Text;

    [Header("RPS 2 - Drain Source Voltage")]
    public RPSController rps2;
    public TMP_Text voltmeter2Text;

    [Header("Ammeter")]
    public TMP_Text ammeterText;

    [Header("MOSFET Model Parameters")]
    [Tooltip("Threshold voltage in volts")]
    public float thresholdVoltage = 2f;

    [Tooltip("MOSFET model factor in A/V^2")]
    public float kFactor = 0.001f;

    [Tooltip("Channel length modulation in 1/V")]
    public float lambda = 0.02f;

    [Header("Live Values")]
    public float VGS;
    public float VDS;
    public float ID_mA;

    private void Update()
    {
        // Read RPS 1: VGS
        VGS = 0f;

        if (rps1 != null && rps1.isOn)
            VGS = Mathf.Max(0f, rps1.voltage);

        // Read RPS 2: VDS
        VDS = 0f;

        if (rps2 != null && rps2.isOn)
            VDS = Mathf.Max(0f, rps2.voltage);

        // Update voltmeter displays
        if (voltmeter1Text != null)
            voltmeter1Text.text = VGS.ToString(
                "F2", CultureInfo.InvariantCulture) + " V";

        if (voltmeter2Text != null)
            voltmeter2Text.text = VDS.ToString(
                "F2", CultureInfo.InvariantCulture) + " V";

        // Calculate MOSFET drain current
        ID_mA = CalculateDrainCurrent(VGS, VDS);

        // Update ammeter display
        if (ammeterText != null)
            ammeterText.text = ID_mA.ToString(
                "F3", CultureInfo.InvariantCulture) + " mA";

        // Update RPS current displays
        if (rps1 != null)
            rps1.SetCurrent(ID_mA / 1000f);

        if (rps2 != null)
            rps2.SetCurrent(ID_mA / 1000f);
    }

    private float CalculateDrainCurrent(float vgs, float vds)
    {
        if (vgs <= thresholdVoltage || vds <= 0f)
            return 0f;

        float overdrive = vgs - thresholdVoltage;
        float idAmps;

        if (vds < overdrive)
        {
            // Linear / triode region
            idAmps = kFactor *
                (overdrive * vds - 0.5f * vds * vds);
        }
        else
        {
            // Saturation region
            idAmps = 0.5f * kFactor *
                overdrive * overdrive *
                (1f + lambda * vds);
        }

        return Mathf.Max(0f, idAmps * 1000f);
    }
}
