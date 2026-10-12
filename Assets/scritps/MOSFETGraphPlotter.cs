using UnityEngine;
using TMPro;
using XCharts.Runtime;

public class MOSFETGraphPlotter : MonoBehaviour
{
    public LineChart chart;

    [Header("Table Data")]
    public TMP_Text[] vdsColumn;
    public TMP_Text[] vgs35Column;
    public TMP_Text[] vgs36Column;
    public TMP_Text[] vgs37Column;

    public void PlotGraph()
    {
        if (chart == null)
        {
            Debug.LogError("LineChart is not assigned!");
            return;
        }

        // Remove previous graph data
        chart.RemoveData();

        // Create 3 series
        chart.AddSerie<Line>("VGS 3.5V");
        chart.AddSerie<Line>("VGS 3.6V");
        chart.AddSerie<Line>("VGS 3.7V");

        // Read table and plot
        for (int i = 0; i < vdsColumn.Length; i++)
        {
            float vds;

            if (!float.TryParse(vdsColumn[i].text, out vds))
                continue;

            // VGS 3.5V
            if (float.TryParse(vgs35Column[i].text, out float id35))
            {
                chart.AddData(
                    0,
                    vds,
                    id35
                );
            }

            // VGS 3.6V
            if (float.TryParse(vgs36Column[i].text, out float id36))
            {
                chart.AddData(
                    1,
                    vds,
                    id36
                );
            }

            // VGS 3.7V
            if (float.TryParse(vgs37Column[i].text, out float id37))
            {
                chart.AddData(
                    2,
                    vds,
                    id37
                );
            }
        }

        chart.RefreshChart();

        Debug.Log("MOSFET Graph Plotted!");
    }

    public void ClearGraph()
    {
        chart.RemoveData();
        chart.RefreshChart();
    }
}