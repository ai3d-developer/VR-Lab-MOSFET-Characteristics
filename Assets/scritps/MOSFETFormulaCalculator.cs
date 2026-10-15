
using UnityEngine;
using TMPro;
using System.Globalization;

public class MOSFETFormulaCalculator : MonoBehaviour
{
    [Header("Meter Texts")]
    public TMP_Text vgsText;
    public TMP_Text vdsText;

    [Header("Output Texts")]
    public TMP_Text idDisplay;
    public TMP_Text messageDisplay;

    [Header("MOSFET Parameters")]
    public float thresholdVoltage = 2f;
    public float kFactor = 0.001f;
    public float lambda = 0.02f;

    [Header("Calculated Values")]
    public float currentVGS;
    public float currentVDS;
    public float currentID_mA;

    private void Update()
    {
        CalculateCurrent();
    }

    public void CalculateCurrent()
    {
        if (vgsText == null || vdsText == null)
        {
            ShowMessage("Assign VGS and VDS TMP Texts.");
            return;
        }

        if (!TryReadNumber(vgsText.text, out currentVGS))
        {
            ShowMessage("Invalid VGS meter reading.");
            return;
        }

        if (!TryReadNumber(vdsText.text, out currentVDS))
        {
            ShowMessage("Invalid VDS meter reading.");
            return;
        }

        currentID_mA = CalculateDrainCurrent(
            currentVGS, currentVDS);

        if (idDisplay != null)
        {
            idDisplay.text = currentID_mA.ToString(
                "0.###", CultureInfo.InvariantCulture) + " mA";
        }

        ShowMessage("Calculation updated.");
    }

    private float CalculateDrainCurrent(float vgs, float vds)
    {
        if (vgs <= thresholdVoltage || vds <= 0f)
            return 0f;

        float vOverdrive = vgs - thresholdVoltage;
        float idAmps;

        if (vds < vOverdrive)
        {
            idAmps = kFactor *
                (vOverdrive * vds - 0.5f * vds * vds);
        }
        else
        {
            idAmps = 0.5f * kFactor *
                vOverdrive * vOverdrive *
                (1f + lambda * vds);
        }

        return Mathf.Max(0f, idAmps * 1000f);
    }

    private bool TryReadNumber(string text, out float value)
    {
        value = 0f;

        if (string.IsNullOrWhiteSpace(text))
            return false;

        string cleaned = text.Trim()
            .Replace("mA", "")
            .Replace("V", "")
            .Replace("v", "")
            .Trim();

        return float.TryParse(
            cleaned,
            NumberStyles.Float,
            CultureInfo.InvariantCulture,
            out value);
    }

    private void ShowMessage(string message)
    {
        if (messageDisplay != null)
            messageDisplay.text = message;
    }

    public void CalculateButton()
    {
        CalculateCurrent();
    }
}
