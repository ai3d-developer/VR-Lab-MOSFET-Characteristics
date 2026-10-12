using UnityEngine;
using TMPro;

public class MOSFETTableRecorder : MonoBehaviour
{
    [Header("Meter Displays")]
    public TMP_Text vgsMeter;
    public TMP_Text vdsMeter;
    public TMP_Text ammeter;

    [Header("Table")]
    public TMP_Text[] vdsColumn;
    public TMP_Text[] vgs35Column;
    public TMP_Text[] vgs36Column;
    public TMP_Text[] vgs37Column;

    private int currentRow = 0;

    public void RecordReadings()
    {
        float vgs = GetValue(vgsMeter.text);
        float vds = GetValue(vdsMeter.text);
        float id = GetValue(ammeter.text);

        if (currentRow >= vdsColumn.Length)
        {
            Debug.Log("Table is full!");
            return;
        }

        // VDS column
        vdsColumn[currentRow].text = vds.ToString("0.0");

        // ID according to VGS
        if (Mathf.Abs(vgs - 3.5f) < 0.05f)
        {
            vgs35Column[currentRow].text = id.ToString("0.0");
        }
        else if (Mathf.Abs(vgs - 3.6f) < 0.05f)
        {
            vgs36Column[currentRow].text = id.ToString("0.0");
        }
        else if (Mathf.Abs(vgs - 3.7f) < 0.05f)
        {
            vgs37Column[currentRow].text = id.ToString("0.0");
        }

        currentRow++;

        Debug.Log(
            "Recorded: VGS=" + vgs +
            " VDS=" + vds +
            " ID=" + id
        );
    }

    float GetValue(string text)
    {
        string number = "";

        foreach (char c in text)
        {
            if (char.IsDigit(c) || c == '.')
                number += c;
        }

        if (float.TryParse(number, out float value))
            return value;

        return 0f;
    }

    public void ClearTable()
    {
        for (int i = 0; i < vdsColumn.Length; i++)
        {
            vdsColumn[i].text = "";
            vgs35Column[i].text = "";
            vgs36Column[i].text = "";
            vgs37Column[i].text = "";
        }

        currentRow = 0;
    }
}