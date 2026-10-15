
using UnityEngine;
using TMPro;
using XCharts.Runtime;
using System.Collections.Generic;
using System.Globalization;

public class MOSFETGraphPlotterSafe : MonoBehaviour
{
    [Header("Existing XCharts LineChart")]
    public LineChart chart;

    [Header("Table Columns")]
    public TMP_Text[] vdsColumn;
    public TMP_Text[] vgs35Column;
    public TMP_Text[] vgs36Column;
    public TMP_Text[] vgs37Column;

    private readonly List<Vector2>[] plottedPoints =
    {
        new List<Vector2>(),
        new List<Vector2>(),
        new List<Vector2>()
    };

    private readonly string[] seriesNames =
    {
        "VGS 3.5V",
        "VGS 3.6V",
        "VGS 3.7V"
    };

    public void PlotGraph()
    {
        if (chart == null)
        {
            Debug.LogError("Assign the existing LineChart!");
            return;
        }

        if (vdsColumn == null || vgs35Column == null ||
            vgs36Column == null || vgs37Column == null)
        {
            Debug.LogError("Assign all four table columns!");
            return;
        }

        if (chart.series.Count < 3)
        {
            Debug.LogError(
                "Create the 3 Line series in the Inspector first.");
            return;
        }

        int rowCount = Mathf.Min(
            vdsColumn.Length,
            Mathf.Min(vgs35Column.Length,
            Mathf.Min(vgs36Column.Length, vgs37Column.Length)));

        List<Vector2>[] newPoints =
        {
            new List<Vector2>(),
            new List<Vector2>(),
            new List<Vector2>()
        };

        // Read saved table values.
        for (int i = 0; i < rowCount; i++)
        {
            if (vdsColumn[i] == null ||
                !ReadNumber(vdsColumn[i].text, out float vds))
                continue;

            AddPoint(newPoints[0], vds, vgs35Column[i]);
            AddPoint(newPoints[1], vds, vgs36Column[i]);
            AddPoint(newPoints[2], vds, vgs37Column[i]);
        }

        // Do not modify chart until valid data is available.
        int total = newPoints[0].Count +
                    newPoints[1].Count +
                    newPoints[2].Count;

        if (total == 0)
        {
            Debug.LogWarning(
                "No recorded readings found. Graph unchanged.");
            return;
        }

        for (int s = 0; s < 3; s++)
        {
            newPoints[s].Sort((a, b) => a.x.CompareTo(b.x));

            // Preserve the existing series and symbol settings.
            chart.series[s].serieName = seriesNames[s];
            chart.series[s].xAxisIndex = 0;
            chart.series[s].yAxisIndex = 0;

            // Add only new points, avoiding duplicates on repeated clicks.
            foreach (Vector2 p in newPoints[s])
            {
                bool alreadyPlotted = false;

                foreach (Vector2 oldPoint in plottedPoints[s])
                {
                    if (Mathf.Abs(oldPoint.x - p.x) < 0.001f &&
                        Mathf.Abs(oldPoint.y - p.y) < 0.001f)
                    {
                        alreadyPlotted = true;
                        break;
                    }
                }

                if (!alreadyPlotted)
                {
                    chart.AddData(s, (double)p.x, (double)p.y);
                    plottedPoints[s].Add(p);
                }
            }

            Debug.Log(
                seriesNames[s] + ": " + newPoints[s].Count + " table points");
        }

        chart.RefreshChart();
        Debug.Log("Graph updated from table. Total points: " + total);
    }

    private void AddPoint(
        List<Vector2> list, float vds, TMP_Text currentText)
    {
        if (currentText == null ||
            !ReadNumber(currentText.text, out float id))
            return;

        list.Add(new Vector2(vds, id));
    }

    private bool ReadNumber(string text, out float value)
    {
        value = 0f;

        if (string.IsNullOrWhiteSpace(text))
            return false;

        string number = "";
        bool dotFound = false;

        foreach (char c in text)
        {
            if (char.IsDigit(c))
                number += c;
            else if (c == '.' && !dotFound)
            {
                number += c;
                dotFound = true;
            }
        }

        return number.Length > 0 &&
            float.TryParse(
                number,
                NumberStyles.Float,
                CultureInfo.InvariantCulture,
                out value);
    }
}
