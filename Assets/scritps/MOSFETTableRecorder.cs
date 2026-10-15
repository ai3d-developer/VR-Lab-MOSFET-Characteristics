
using UnityEngine;
using TMPro;
using System.Globalization;

public class MOSFETTableRecorder : MonoBehaviour
{
    [Header("Live Meter Displays")]
    public TMP_Text vgsMeter;   // Circuit Controller: v1
    public TMP_Text vdsMeter;   // Circuit Controller: v2
    public TMP_Text ammeter;    // Ammeter display

    [Header("VGS Column Headings")]
    public TMP_Text vgsHeading1;
    public TMP_Text vgsHeading2;
    public TMP_Text vgsHeading3;

    [Header("VDS Table Column")]
    public TMP_Text[] vdsColumn;

    [Header("Drain Current Columns")]
    public TMP_Text[] vgs1Column;
    public TMP_Text[] vgs2Column;
    public TMP_Text[] vgs3Column;

    [Header("Settings")]
    public float vdsTolerance = 0.2f;
    public float vgsTolerance = 0.05f;

    private readonly float[] columnVGS = {
        float.NaN, float.NaN, float.NaN
    };

    private void Start()
    {
        // Restore existing headings if they contain voltage values.
        ReadExistingHeading(0, vgsHeading1);
        ReadExistingHeading(1, vgsHeading2);
        ReadExistingHeading(2, vgsHeading3);
    }

    public void RecordReadings()
    {
        if (vgsMeter == null || vdsMeter == null || ammeter == null)
        {
            Debug.LogError("Assign VGS, VDS and Ammeter TMP Text displays.");
            return;
        }

        if (!TryGetValue(vgsMeter.text, out float vgs) ||
            !TryGetValue(vdsMeter.text, out float vds) ||
            !TryGetValue(ammeter.text, out float id))
        {
            Debug.LogWarning("Invalid meter reading.");
            return;
        }

        // Find a column already assigned to this VGS,
        // or allocate the next empty VGS column.
        int series = FindOrCreateVGSColumn(vgs);

        if (series == -1)
        {
            Debug.LogWarning(
                "All 3 VGS columns are occupied. Clear the table to restart.");
            return;
        }

        TMP_Text[] targetColumn = GetIDColumn(series);

        if (!ValidColumn(vdsColumn) || !ValidColumn(targetColumn))
        {
            Debug.LogError("Assign all VDS and ID table cells in Inspector.");
            return;
        }

        // Reuse the same row when the VDS value already exists.
        int row = FindMatchingVDSRow(vds);

        if (row == -1)
            row = FindEmptyRow();

        if (row == -1)
        {
            Debug.LogWarning("Table is full.");
            return;
        }

        vdsColumn[row].text = Format(vds);
        targetColumn[row].text = Format(id);

        Debug.Log(
            $"Recorded VGS={vgs:0.###} V, " +
            $"VDS={vds:0.###} V, ID={id:0.###} mA, " +
            $"VGS column={series + 1}, row={row + 1}");
    }

    private int FindOrCreateVGSColumn(float vgs)
    {
        // Reuse the column if this VGS has already been recorded.
        for (int i = 0; i < 3; i++)
        {
            if (!float.IsNaN(columnVGS[i]) &&
                Mathf.Abs(columnVGS[i] - vgs) <= vgsTolerance)
            {
                return i;
            }
        }

        // Otherwise allocate the next available VGS column.
        for (int i = 0; i < 3; i++)
        {
            if (float.IsNaN(columnVGS[i]))
            {
                columnVGS[i] = vgs;
                SetHeading(i, vgs);
                return i;
            }
        }

        return -1;
    }

    private void SetHeading(int index, float vgs)
    {
        TMP_Text heading = GetHeading(index);

        if (heading != null)
            heading.text = "VGS = " + Format(vgs) + " V";
    }

    private void ReadExistingHeading(int index, TMP_Text heading)
    {
        if (heading == null) return;

        string text = heading.text;

        if (TryGetValue(text, out float value) &&
            text.Contains("="))
        {
            columnVGS[index] = value;
        }
    }

    private TMP_Text GetHeading(int index)
    {
        if (index == 0) return vgsHeading1;
        if (index == 1) return vgsHeading2;
        return vgsHeading3;
    }

    private TMP_Text[] GetIDColumn(int index)
    {
        if (index == 0) return vgs1Column;
        if (index == 1) return vgs2Column;
        return vgs3Column;
    }

    private int FindMatchingVDSRow(float vds)
    {
        int count = vdsColumn != null ? vdsColumn.Length : 0;

        int bestRow = -1;
        float bestDifference = float.MaxValue;

        for (int i = 0; i < count; i++)
        {
            if (vdsColumn[i] == null ||
                string.IsNullOrWhiteSpace(vdsColumn[i].text))
                continue;

            if (!TryGetValue(vdsColumn[i].text, out float oldVds))
                continue;

            float difference = Mathf.Abs(oldVds - vds);

            if (difference <= vdsTolerance &&
                difference < bestDifference)
            {
                bestDifference = difference;
                bestRow = i;
            }
        }

        return bestRow;
    }

    private int FindEmptyRow()
    {
        if (vdsColumn == null) return -1;

        for (int i = 0; i < vdsColumn.Length; i++)
        {
            if (vdsColumn[i] != null &&
                string.IsNullOrWhiteSpace(vdsColumn[i].text))
            {
                return i;
            }
        }

        return -1;
    }

    private bool ValidColumn(TMP_Text[] column)
    {
        if (column == null || column.Length == 0)
            return false;

        foreach (TMP_Text cell in column)
        {
            if (cell == null) return false;
        }

        return true;
    }

    private bool TryGetValue(string text, out float value)
    {
        value = 0f;

        if (string.IsNullOrWhiteSpace(text))
            return false;

        string cleaned = text.Trim()
            .Replace("mA", "")
            .Replace("MA", "")
            .Replace("V", "")
            .Replace("v", "")
            .Trim();

        return float.TryParse(
            cleaned,
            NumberStyles.Float,
            CultureInfo.InvariantCulture,
            out value);
    }

    private string Format(float value)
    {
        return value.ToString("0.###", CultureInfo.InvariantCulture);
    }

    public void ClearTable()
    {
        ClearColumn(vdsColumn);
        ClearColumn(vgs1Column);
        ClearColumn(vgs2Column);
        ClearColumn(vgs3Column);

        for (int i = 0; i < 3; i++)
        {
            columnVGS[i] = float.NaN;

            TMP_Text heading = GetHeading(i);

            if (heading != null)
                heading.text = "VGS =";
        }
    }

    private void ClearColumn(TMP_Text[] column)
    {
        if (column == null) return;

        foreach (TMP_Text cell in column)
        {
            if (cell != null)
                cell.text = "";
        }
    }
}
