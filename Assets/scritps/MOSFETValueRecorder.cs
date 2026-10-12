
using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class MOSFETValueRecorder : MonoBehaviour
{
    [Header("Live Values")]
    public float currentVGS;
    public float currentVDS;
    public float currentID;

    [Header("Display Texts")]
    public TMP_Text vgsDisplay;
    public TMP_Text vdsDisplay;
    public TMP_Text idDisplay;

    [Header("Table")]
    public Transform tableContent;
    public GameObject tableRowPrefab;

    [Header("Message")]
    public TMP_Text messageText;

    private int rowNumber = 0;

    // Reference characteristic data
    private readonly float[] vdsPoints =
        { 1f, 3f, 5f, 7f, 9f, 11f, 15f, 18f, 23f, 25f };

    private readonly float[] id35 =
        { 0.2f, 0.9f, 0.9f, 0.9f, 0.9f,
          0.9f, 0.9f, 0.9f, 0.9f, 0.9f };

    private readonly float[] id36 =
        { 0.3f, 0.9f, 1.6f, 2.2f, 2.6f,
          2.6f, 2.6f, 2.6f, 2.6f, 2.6f };

    private readonly float[] id37 =
        { 0.3f, 0.9f, 1.6f, 2.2f, 2.7f,
          3.4f, 4.6f, 5.2f, 5.4f, 5.8f };

    // Call whenever the actual voltage controls change
    public void UpdateValues(float vgs, float vds, float id)
    {
        currentVGS = vgs;
        currentVDS = vds;

        // Use the reference characteristic model for ID
        currentID = CalculateDrainCurrent(currentVGS, currentVDS);

        UpdateDisplay();
    }

    float CalculateDrainCurrent(float vgs, float vds)
    {
        // These are reference-table curves, not a physical
        // IRF340 transistor simulation.
        if (vds < 1f || vgs < 3.45f)
            return 0f;

        float[] currents;

        if (vgs < 3.55f)
            currents = id35;
        else if (vgs < 3.65f)
            currents = id36;
        else
            currents = id37;

        if (vds <= vdsPoints[0])
            return currents[0];

        int last = vdsPoints.Length - 1;

        if (vds >= vdsPoints[last])
            return currents[last];

        for (int i = 0; i < last; i++)
        {
            if (vds >= vdsPoints[i] &&
                vds <= vdsPoints[i + 1])
            {
                float t = (vds - vdsPoints[i]) /
                          (vdsPoints[i + 1] - vdsPoints[i]);

                return Mathf.Lerp(currents[i], currents[i + 1], t);
            }
        }

        return 0f;
    }

    void UpdateDisplay()
    {
        if (vgsDisplay != null)
            vgsDisplay.text = currentVGS.ToString("0.0") + " V";

        if (vdsDisplay != null)
            vdsDisplay.text = currentVDS.ToString("0.0") + " V";

        if (idDisplay != null)
            idDisplay.text = currentID.ToString("0.0") + " mA";
    }

    // Record Value button
    public void RecordValue()
    {
        if (tableRowPrefab == null || tableContent == null)
        {
            Debug.LogError(
                "Table Row Prefab or Table Content is missing!");
            return;
        }

        GameObject newRow = Instantiate(
            tableRowPrefab, tableContent);

        TMP_Text[] texts =
            newRow.GetComponentsInChildren<TMP_Text>(true);

        if (texts.Length < 4)
        {
            Debug.LogError(
                "Table row prefab needs at least 4 TMP text fields.");
            Destroy(newRow);
            return;
        }

        rowNumber++;

        texts[0].text = rowNumber.ToString();
        texts[1].text = currentVGS.ToString("0.0") + " V";
        texts[2].text = currentVDS.ToString("0.0") + " V";
        texts[3].text = currentID.ToString("0.0") + " mA";

        if (messageText != null)
            messageText.text = "Value Recorded";

        Debug.Log(
            $"Recorded: VGS={currentVGS:0.0} V, " +
            $"VDS={currentVDS:0.0} V, ID={currentID:0.0} mA");
    }

    public void ClearTable()
    {
        if (tableContent != null)
        {
            foreach (Transform child in tableContent)
                Destroy(child.gameObject);
        }

        rowNumber = 0;

        if (messageText != null)
            messageText.text = "Table Cleared";
    }
}
