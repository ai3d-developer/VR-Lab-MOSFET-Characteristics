using UnityEngine;

public class GuideManager : MonoBehaviour
{
    public static GuideManager Instance;

    public GameObject[] allGuides;

    private void Awake()
    {
        Instance = this;
    }

    public void HideAllGuides()
    {
        foreach (GameObject guide in allGuides)
        {
            if (guide != null)
                guide.SetActive(false);
        }
    }

    public void ShowOnly(GameObject guide)
    {
        HideAllGuides();

        if (guide != null)
            guide.SetActive(true);
    }
}